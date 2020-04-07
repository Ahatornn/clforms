using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows control that displays a frame around a control with an optional caption
    /// </summary>
    public class GroupBox: SingleContentControl, IElementStyle<GroupBox>
    {
        private string text;
        private TextAlignment textAlignment;
        private Thickness borderThickness;
        private Color borderColor;
        private BorderChars borderChars;

        /// <summary>
        /// Initialize a new instance <see cref="GroupBox"/>
        /// </summary>
        public GroupBox()
        {
            Background = Color.NotSet;
            Foreground = Color.NotSet;
            borderThickness = new Thickness(1);
            borderChars = Application.Environment.BorderChars;
        }

        #region Properties

        #region Text

        /// <summary>
        /// Gets or sets the text associated with this control
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    OnTextChanged?.Invoke(this, new PropertyChangedEventArgs<string>(text, value));
                    text = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region TextAlignment

        /// <summary>
        /// Gets or sets the horizontal alignment of the text associated with this control
        /// </summary>
        public TextAlignment TextAlignment
        {
            get => textAlignment;
            set
            {
                if (textAlignment != value)
                {
                    OnTextAlignmentChanged?.Invoke(this,
                        new PropertyChangedEventArgs<TextAlignment>(textAlignment, value));
                    textAlignment = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region BorderThickness

        /// <summary>
        /// Gets or sets the relative frame <see cref="Thickness"/> of a <see cref="GroupBox"/>
        /// </summary>
        public Thickness BorderThickness
        {
            get => borderThickness;
            set
            {
                if (borderThickness != value)
                {
                    OnBorderThicknessChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Thickness>(borderThickness, value));
                    borderThickness = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region BorderColor

        /// <summary>
        /// Gets or sets the <see cref="Color"/> that draws the border color
        /// </summary>
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor != value)
                {
                    OnBorderColorChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(borderColor, value));
                    borderColor = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region BorderChars

        /// <summary>
        /// Gets or sets the <see cref="BorderChars"/> that draws the border characters
        /// </summary>
        public BorderChars BorderChars
        {
            get => borderChars;
            set
            {
                if (borderChars != value)
                {
                    OnBorderCharsChanged?.Invoke(this, new PropertyChangedEventArgs<BorderChars>(borderChars, value));
                    borderChars = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            var marginWithBorder = Margin + BorderThickness;
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(borderThickness)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;
            var desiredContentSize = contentArea.Size;

            if (Content != null && !desiredContentSize.HasEmptyDimension())
            {
                Content.Measure(desiredContentSize);
                var size = Content.DesiredSize;
                size.Width = Math.Min(Width.HasValue
                        ? Width.Value + Margin.Horizontal
                        : size.Width + (marginWithBorder + Padding).Horizontal,
                    availableSize.Width);
                size.Height = Math.Min(Height.HasValue
                        ? Height.Value + Margin.Vertical
                        : size.Height + (marginWithBorder + Padding).Vertical,
                    availableSize.Height);

                base.Measure(size);
            }
            else
            {
                base.Measure(new Size(
                    Math.Min((Width ?? Padding.Horizontal) + marginWithBorder.Horizontal, availableSize.Width),
                    Math.Min((Height ?? Padding.Vertical) + marginWithBorder.Vertical, availableSize.Height)));
            }
        }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            var offset = Margin + Padding + BorderThickness;
            var clientRect = new Rect((Padding + BorderThickness).Left,
                (Padding + BorderThickness).Top,
                finalRect.Width - offset.Horizontal,
                finalRect.Height - offset.Vertical);
            Content?.Arrange(clientRect);
            base.Arrange(finalRect);
        }

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            if (borderThickness.Top > 0)
            {
                context.SetCursorPos(borderThickness.Left - 1, borderThickness.Top - 1);
                var topStr = borderChars.TopLeft +
                             (context.ContextBounds.Width - borderThickness.Horizontal > 0
                                 ? new string(borderChars.TopMiddle,
                                     context.ContextBounds.Width - borderThickness.Horizontal)
                                 : string.Empty) +
                             borderChars.TopRight;
                context.DrawText(topStr, borderColor);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var presenterText = text.Length > context.ContextBounds.Width - borderThickness.Horizontal
                        ? text.Substring(0, context.ContextBounds.Width - borderThickness.Horizontal - 1) + "…"
                        : text;
                    switch (textAlignment)
                    {
                        case TextAlignment.Right:
                            context.SetCursorPos(
                                context.ContextBounds.Width - borderThickness.Right - presenterText.Length,
                                borderThickness.Top - 1);
                            break;
                        case TextAlignment.Center:
                            context.SetCursorPos(
                                (context.ContextBounds.Width - borderThickness.Horizontal - presenterText.Length) / 2 +
                                1, borderThickness.Top - 1);
                            break;
                        default:
                            context.SetCursorPos(borderThickness.Left, borderThickness.Top - 1);
                            break;
                    }

                    context.DrawText(presenterText, borderColor);
                }
            }

            for (var row = borderThickness.Top; row < context.ContextBounds.Height - borderThickness.Bottom; row++)
            {
                if (borderThickness.Left > 0)
                {
                    context.SetCursorPos(borderThickness.Left - 1, row);
                    context.DrawText(borderChars.MiddleLeft, borderColor);
                }

                if (borderThickness.Right > 0)
                {
                    context.SetCursorPos(context.ContextBounds.Width - borderThickness.Right, row);
                    context.DrawText(borderChars.MiddleRight, borderColor);
                }
            }

            if (borderThickness.Bottom > 0)
            {
                context.SetCursorPos(borderThickness.Left - 1,
                    context.ContextBounds.Height - borderThickness.Bottom);
                var bottomStr = borderChars.BottomLeft +
                                (context.ContextBounds.Width - borderThickness.Horizontal > 0
                                    ? new string(borderChars.BottomMiddle,
                                        context.ContextBounds.Width - borderThickness.Horizontal)
                                    : string.Empty) +
                                borderChars.BottomRight;
                context.DrawText(bottomStr, borderColor);
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<GroupBox> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => base.ToString() + ", Text: " + this.Text;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Text" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnTextChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<TextAlignment>> OnTextAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderThickness" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Thickness>> OnBorderThicknessChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderColor" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnBorderColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderChars" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<BorderChars>> OnBorderCharsChanged;

        #endregion
    }
}