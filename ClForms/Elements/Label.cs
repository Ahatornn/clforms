using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a standard Windows label
    /// </summary>
    public class Label: Control, IElementStyle<Label>
    {
        private string text;
        private bool wordWrap;
        private TextAlignment textAlignment;

        /// <summary>
        /// Initialize a new instance <see cref="Label"/>
        /// </summary>
        public Label()
        {
            textAlignment = TextAlignment.Left;
            Background = Color.NotSet;
            Foreground = Application.SystemColors.WindowForeground;
            wordWrap = false;
        }

        /// <summary>
        /// Initialize a new instance <see cref="Label"/> with text
        /// </summary>
        public Label(string text)
            : this()
        {
            this.text = text;
        }

        /// <summary>
        /// Initialize a new instance <see cref="Label"/> with lines of text
        /// </summary>
        public Label(string[] lines)
            : this()
        {
            if (lines == null)
            {
                throw new ArgumentNullException();
            }

            text = GetAllText(lines);
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
                    InvalidateMeasureIfAutoSize();
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
                    InvalidateMeasureIfAutoSize();
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
        #region Lines

        /// <summary>
        /// Gets or sets the individual lines of text in the control
        /// </summary>
        public string[] Lines
        {
            get => string.IsNullOrWhiteSpace(text)
                ? Array.Empty<string>()
                : text.Split(Environment.NewLine);
            set => Text = GetAllText(value ?? Array.Empty<string>());
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;
            var presentedText = GetTextPresenter();

            if (!string.IsNullOrWhiteSpace(presentedText) && !contentArea.HasEmptyDimension())
            {
                Size size;
                if (presentedText.Length <= contentArea.Width)
                {
                    size = new Size(presentedText.Length, 1);
                }
                else
                {
                    if (!wordWrap)
                    {
                        size = new Size(contentArea.Width, 1);
                    }
                    else
                    {
                        var tempParagraph = TextHelper.GetParagraph(presentedText, contentArea.Width).ToArray();
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
            base.OnRender(context);
            var reducedArea = context.ContextBounds.Reduce(Padding);
            var presentedText = GetTextPresenter();
            if (reducedArea.HasEmptyDimension() || string.IsNullOrWhiteSpace(presentedText))
            {
                return;
            }
            var paragraph = TextHelper.GetParagraph(presentedText, reducedArea.Width).ToArray();
            var rowCount = Math.Min(paragraph.Length, reducedArea.Height);
            for (var row = 0; row < rowCount; row++)
            {
                context.SetCursorPos(Padding.Left, Padding.Top + row);
                context.DrawText(TextHelper.GetTextWithAlignment(paragraph[row], reducedArea.Width, textAlignment));
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Label> styleAction) => styleAction?.Invoke(this);

        /// <summary>
        /// Removes all characters from the <see cref="Text"/>
        /// </summary>
        public void Clear() => Text = string.Empty;

        protected virtual string GetTextPresenter() => text;

        private static string GetAllText(string[] lines)
        {
            var aggrStr = string.Empty;
            var allText = lines.Aggregate(aggrStr,
                ((current, value) => current.Length > 0 ? current + Environment.NewLine + value : current + value));
            return string.Concat(allText);
        }

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

        #endregion
    }
}
