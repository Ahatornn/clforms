using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows control that displays a frame around a control with an optional caption
    /// </summary>
    public class GroupBox: SingleContentControl, IElementStyle<GroupBox>
    {
        private readonly GroupBase groupBase;

        /// <summary>
        /// Initialize a new instance <see cref="GroupBox"/>
        /// </summary>
        public GroupBox()
        {
            Background = Color.NotSet;
            Foreground = Color.NotSet;
            groupBase = new GroupBase(this);
        }

        #region Properties

        #region Text

        /// <summary>
        /// Gets or sets the text associated with this control
        /// </summary>
        public string Text
        {
            get => groupBase.text;
            set
            {
                if (groupBase.text != value)
                {
                    OnTextChanged?.Invoke(this, new PropertyChangedEventArgs<string>(groupBase.text, value));
                    groupBase.text = value;
                    InvalidateMeasureIfAutoSize();
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
            get => groupBase.textAlignment;
            set
            {
                if (groupBase.textAlignment != value)
                {
                    OnTextAlignmentChanged?.Invoke(this,
                        new PropertyChangedEventArgs<TextAlignment>(groupBase.textAlignment, value));
                    groupBase.textAlignment = value;
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
            get => groupBase.borderThickness;
            set
            {
                if (groupBase.borderThickness != value)
                {
                    OnBorderThicknessChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Thickness>(groupBase.borderThickness, value));
                    groupBase.borderThickness = value;
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
            get => groupBase.borderColor;
            set
            {
                if (groupBase.borderColor != value)
                {
                    OnBorderColorChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(groupBase.borderColor, value));
                    groupBase.borderColor = value;
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
            get => groupBase.borderChars;
            set
            {
                if (groupBase.borderChars != value)
                {
                    OnBorderCharsChanged?.Invoke(this, new PropertyChangedEventArgs<BorderChars>(groupBase.borderChars, value));
                    groupBase.borderChars = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
            => base.Measure(groupBase.Measure(availableSize, (contentSize) =>
                {
                    Content.Measure(contentSize);
                    return Content.DesiredSize;
                },
                Content != null));

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true) =>
            base.Arrange(groupBase.Arrange(finalRect, clientRect =>
            {
                Content.Arrange(clientRect);
            }, Content != null));

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            groupBase.OnRender(context);
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