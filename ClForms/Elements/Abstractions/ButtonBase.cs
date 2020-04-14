using System;
using System.Linq;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Helpers;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Specifies the basic functionality common to button controls
    /// </summary>
    public abstract class ButtonBase: BaseFocusableControl
    {
        private string text;
        private bool wordWrap;
        private TextAlignment textAlignment;
        protected Point RenderTextPosition { get; private set; }

        /// <summary>
        /// Initialize a new instance <see cref="ButtonBase"/>
        /// </summary>
        protected ButtonBase()
        {
            Background = Application.SystemColors.ButtonFace;
            Foreground = Application.SystemColors.ButtonText;
            Padding = new Thickness(2, 0);
            wordWrap = false;
            textAlignment = TextAlignment.Center;
            RenderTextPosition = Point.Empty;
        }

        /// <summary>
        /// Initialize a new instance <see cref="ButtonBase"/> with value of the <see cref="Text"/>
        /// </summary>
        protected ButtonBase(string text)
            : this()
        {
            this.text = text;
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
        #region WordWrap

        /// <summary>
        /// Indicates whether a multiline text box control automatically wraps words to
        /// the beginning of the next line when necessary
        /// </summary>
        public bool WordWrap
        {
            get => wordWrap;
            set
            {
                if (wordWrap != value)
                {
                    OnWordWrapChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(wordWrap, value));
                    wordWrap = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region TextAlignment

        /// <summary>
        /// Gets or sets the horizontal alignment of the contents of the text
        /// </summary>
        public TextAlignment TextAlignment
        {
            get => textAlignment;
            set
            {
                if (textAlignment != value)
                {
                    OnTextAlignmentChanged?.Invoke(this, new PropertyChangedEventArgs<TextAlignment>(textAlignment, value));
                    textAlignment = value;
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
            var textPresenter = GetTextPresenter();
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;

            if (!string.IsNullOrWhiteSpace(textPresenter) && !contentArea.HasEmptyDimension())
            {
                Size size;
                if (textPresenter.Length <= contentArea.Width)
                {
                    size = new Size(textPresenter.Length, 1);
                }
                else
                {
                    if (!wordWrap)
                    {
                        size = new Size(contentArea.Width, 1);
                    }
                    else
                    {
                        var tempParagraph = TextHelper.GetParagraph(textPresenter, contentArea.Width).ToArray();
                        size = new Size(contentArea.Width, tempParagraph.Length);
                    }
                }

                size.Width = Math.Min(Width.HasValue
                        ? Width.Value + Margin.Horizontal
                        : size.Width + (Margin + Padding).Horizontal,
                    availableSize.Width);
                size.Height = Math.Min(Height.HasValue
                        ? Height.Value + Margin.Vertical
                        : size.Height + (Margin + Padding).Vertical,
                    availableSize.Height);

                base.Measure(size);
            }
            else
            {
                base.Measure(new Size(Math.Min((Width ?? Padding.Horizontal) + Margin.Horizontal, availableSize.Width),
                    Math.Min((Height ?? Padding.Vertical) + Margin.Vertical, availableSize.Height)));
            }
        }

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            context.Release(GetRenderBackColor(), GetRenderForeColor(), '\0');

            var textPresenter = GetTextPresenter();
            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension() ||
                string.IsNullOrWhiteSpace(textPresenter))
            {
                return;
            }

            var paragraph = TextHelper.GetParagraph(textPresenter, reducedArea.Width).ToArray();
            var rowCount = Math.Min(paragraph.Length, reducedArea.Height);

            var leftIndent = (context.ContextBounds.Width - reducedArea.Width - Padding.Horizontal) / 2;
            var topIndent = (context.ContextBounds.Height - rowCount - Padding.Vertical) / 2;

            for (var row = 0; row < rowCount; row++)
            {
                context.SetCursorPos(leftIndent + Padding.Left, topIndent + Padding.Top + row);
                var str = TextHelper.GetTextWithAlignment(paragraph[row], reducedArea.Width, textAlignment);
                if (row == 0)
                {
                    RenderTextPosition = new Point(leftIndent + Array.FindIndex(str.ToCharArray(),
                                                       x => !char.IsWhiteSpace(x)), topIndent);
                }
                context.DrawText(str);
            }
        }

        /// <inheritdoc cref="BaseFocusableControl.InputAction"/>
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Spacebar ||
                keyInfo.Key == ConsoleKey.Enter)
            {
                Click(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the text on the button
        /// </summary>
        protected abstract string GetTextPresenter();

        /// <summary>
        /// Call event of the <see cref="OnClick" />
        /// </summary>
        protected virtual void Click(EventArgs e) => OnClick?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Text" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnTextChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="WordWrap" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnWordWrapChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<TextAlignment>> OnTextAlignmentChanged;

        /// <summary>
        /// Occurs when the <see cref="ButtonBase"/> control is clicked
        /// </summary>
        public event EventHandler OnClick;

        #endregion
    }
}
