using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows text box control
    /// </summary>
    public class TextBox: BaseFocusableControl, IElementStyle<TextBox>, ICursorAdmit
    {
        private string text;
        private int maxLength;
        private char passwordChar;
        private bool readOnly;
        private CharacterCasing characterCasing;

        /// <summary>
        /// Initialize a new instance <see cref="TextBox"/>
        /// </summary>
        public TextBox()
        {
            Height = 1;
            Background = Application.SystemColors.ControlFace;
            Foreground = Application.SystemColors.ControlText;
            FocusBackground = Application.SystemColors.ControlFocusedFace;
            FocusForeground = Application.SystemColors.ControlFocusedText;
            DisabledBackground = Application.SystemColors.ControlInactiveFace;
            DisabledForeground = Application.SystemColors.ControlInactiveText;
            CursorPosition = Point.Empty;
            passwordChar = char.MinValue;
            readOnly = false;
            maxLength = 0;
            AutoSize = false;
            characterCasing = CharacterCasing.Normal;
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
                    OnTextChanged?.Invoke(this, new PropertyChangedEventArgs<string>(text, NormalizeText(value)));
                    text = NormalizeText(value);
                    InvalidateMeasureIfAutoSize();
                }
            }
        }

        #endregion
        #region MaxLength

        /// <summary>
        /// Gets or sets the maximum number of characters that can be manually entered into the text box
        /// </summary>
        public int MaxLength
        {
            get => maxLength;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Invalid low bound argument for property TextBox.MaxLength");
                }
                if (maxLength != value)
                {
                    OnMaxLengthChanged?.Invoke(this, new PropertyChangedEventArgs<int>(maxLength, value));
                    maxLength = value;
                }
            }
        }

        #endregion
        #region PasswordChar

        /// <summary>
        /// Gets or sets the character used to mask characters of a password in the control
        /// </summary>
        public char PasswordChar
        {
            get => passwordChar;
            set
            {
                if (passwordChar != value)
                {
                    OnPasswordCharChanged?.Invoke(this, new PropertyChangedEventArgs<char>(passwordChar, value));
                    passwordChar = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region ReadOnly

        /// <summary>
        /// Gets or sets a value indicating whether the contents of the <see cref="TextBox"/> control can be changed
        /// </summary>
        public bool ReadOnly
        {
            get => readOnly;
            set
            {
                if (readOnly != value)
                {
                    OnReadOnlyChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(readOnly, value));
                    readOnly = value;
                }
            }
        }

        #endregion
        #region CharacterCasing

        /// <summary>
        /// Gets or sets whether the <see cref="TextBox"/> control modifies the case of characters as they are typed
        /// </summary>
        public CharacterCasing CharacterCasing
        {
            get => characterCasing;
            set
            {
                if (characterCasing != value)
                {
                    OnCharacterCasingChanged?.Invoke(this, new PropertyChangedEventArgs<CharacterCasing>(characterCasing, value));
                    characterCasing = value;
                    if (!string.IsNullOrWhiteSpace(text) && text != NormalizeText(text))
                    {
                        OnTextChanged?.Invoke(this, new PropertyChangedEventArgs<string>(text, NormalizeText(text)));
                        text = NormalizeText(text);
                    }
                    if (passwordChar != char.MinValue)
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion

        /// <inheritdoc cref="ICursorAdmit.CursorPosition"/>
        public Point CursorPosition { get; private set; }

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<TextBox> styleAction) => styleAction?.Invoke(this);

        /// <summary>
        /// Clears all the content from the text box
        /// </summary>
        public void Clear() => Text = string.Empty;

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            if (!AutoSize)
            {
                base.Measure(new Size(Math.Min(Width ?? availableSize.Width, availableSize.Width),
                    Math.Min(Height ?? availableSize.Height, availableSize.Height)));
                return;
            }
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;

            if (!string.IsNullOrWhiteSpace(text) && !contentArea.HasEmptyDimension())
            {
                var size = text.Length <= contentArea.Width
                    ? new Size(text.Length, 1)
                    : new Size(contentArea.Width, 1);

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
            var backColor = GetRenderBackColor();
            var foreColor = GetRenderForeColor();
            context.Release(backColor, foreColor);
            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension() || string.IsNullOrWhiteSpace(text))
            {
                CursorPosition = new Point(Padding.Left, Padding.Top);
                return;
            }
            context.SetCursorPos(Padding.Left, Padding.Top);
            if (IsFocus && text.Length > reducedArea.Size.Width - 1)
            {
                var drawingText = text.Substring(text.Length - reducedArea.Size.Width + 1, reducedArea.Size.Width - 1);
                context.DrawText(PasswordChar == char.MinValue
                    ? drawingText
                    : new string(PasswordChar, drawingText.Length));
                CursorPosition = new Point(Padding.Left + reducedArea.Size.Width - 1, Padding.Top);
            }
            else
            {
                var drawingText = text.Substring(0, Math.Min(reducedArea.Size.Width, text.Length));
                context.DrawText(PasswordChar == char.MinValue
                    ? drawingText
                    : new string(PasswordChar, drawingText.Length));
                CursorPosition = new Point(Padding.Left + drawingText.Length, Padding.Top);
            }
        }

        /// <inheritdoc cref="BaseFocusableControl.InputActionInternal"/>
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            if (ReadOnly)
            {
                return;
            }

            var charPressEventArgs = new CharPressEventArgs(keyInfo.KeyChar);
            OnCharPress?.Invoke(this, charPressEventArgs);
            if (charPressEventArgs.Handled)
            {
                return;
            }

            if ((char.IsLetterOrDigit(charPressEventArgs.KeyChar) ||
                 char.IsPunctuation(charPressEventArgs.KeyChar) ||
                 "№|`~+=^$ ".Contains(charPressEventArgs.KeyChar)) &&
                (maxLength == 0 || text.Length < maxLength))
            {
                Text += NormalizeText(charPressEventArgs.KeyChar.ToString());
            }

            if (charPressEventArgs.KeyChar == (char) ConsoleKey.Backspace &&
                text.Length > 0)
            {
                Text = text.Remove(text.Length - 1);
            }
        }

        private string NormalizeText(string targetText)
        {
            switch (characterCasing)
            {
                case CharacterCasing.Lower:
                    return targetText.ToLower();

                case CharacterCasing.Upper:
                    return targetText.ToUpper();

                default:
                    return targetText;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Text" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnTextChanged;

        /// <summary>
        /// Raises the CharPress event
        /// </summary>
        public event EventHandler<CharPressEventArgs> OnCharPress;

        /// <summary>
        /// Occurs when the value of the <see cref="ReadOnly" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnReadOnlyChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CharacterCasing" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<CharacterCasing>> OnCharacterCasingChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MaxLength" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnMaxLengthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="PasswordChar" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<char>> OnPasswordCharChanged;

        #endregion
    }
}
