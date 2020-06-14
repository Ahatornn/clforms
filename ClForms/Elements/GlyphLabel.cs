using System;
using System.Linq;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common.EventArgs;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a label with specified text of glyph
    /// </summary>
    public class GlyphLabel: Label, IElementStyle<GlyphLabel>
    {
        private string glyph;
        private Color glyphForeground;

        /// <summary>
        /// Initialize a new instance <see cref="GlyphLabel"/>
        /// </summary>
        public GlyphLabel()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize a new instance <see cref="GlyphLabel"/> with text
        /// </summary>
        public GlyphLabel(string text)
            : base(text)
        {
            Initialize();
        }

        /// <summary>
        /// Initialize a new instance <see cref="GlyphLabel"/> with glyph and text
        /// </summary>
        public GlyphLabel(string glyph, string text)
            : base(text)
        {
            Initialize();
            this.glyph = glyph;
        }

        #region Properties

        #region Glyph

        /// <summary>
        /// Gets or sets the text of glyph associated with this control
        /// </summary>
        public string Glyph
        {
            get => glyph;
            set
            {
                if (glyph != value)
                {
                    OnGlyphChanged?.Invoke(this, new PropertyChangedEventArgs<string>(glyph, value));
                    glyph = value;
                }
            }
        }

        #endregion
        #region GlyphForeground

        /// <summary>
        /// Gets or sets a brush that describes the foreground of a glyph
        /// </summary>
        public Color GlyphForeground
        {
            get => glyphForeground;
            set
            {
                if (glyphForeground != value)
                {
                    OnGlyphForegroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(glyphForeground, value));
                    glyphForeground = value;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<GlyphLabel> styleAction) => styleAction?.Invoke(this);

        protected override void OnRender(IDrawingContext context)
        {
            context.Release(Background, Foreground, '\0');

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
                if (row == 0 && !string.IsNullOrWhiteSpace(glyph))
                {
                    var glyphForeColor = glyphForeground != Color.NotSet
                        ? glyphForeground
                        : Foreground;
                    var firstRowText = TextHelper.GetTextWithAlignment(paragraph[row], reducedArea.Width, TextAlignment);
                    context.DrawText(glyph, Background, glyphForeColor);
                    context.DrawText(firstRowText.Substring(glyph.Length));
                }
                else
                {
                    context.DrawText(TextHelper.GetTextWithAlignment(paragraph[row], reducedArea.Width, TextAlignment));
                }
            }
        }

        protected override string GetTextPresenter()
            => string.IsNullOrWhiteSpace(glyph)
                ? Text
                : $"{Glyph} {Text}";

        private void Initialize()
        {
            glyph = string.Empty;
            glyphForeground = Color.NotSet;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Glyph" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnGlyphChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="GlyphForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnGlyphForegroundChanged;

        #endregion
    }
}
