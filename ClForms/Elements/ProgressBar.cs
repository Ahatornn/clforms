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
    /// Represents a Windows progress bar control
    /// </summary>
    public class ProgressBar: Control, IElementStyle<ProgressBar>
    {
        private int maximum;
        private int minimum;
        private int value;
        private bool showValue;
        private bool valueAsPerCent;
        private Color selectedBackground;
        private Color selectedForeground;

        /// <summary>
        /// Initialize a new instance <see cref="ProgressBar"/>
        /// </summary>
        public ProgressBar()
        {
            maximum = 100;
            minimum = 0;
            value = 0;
            Background = Application.SystemColors.ButtonFace;
            Foreground = Application.SystemColors.ButtonText;
            selectedBackground = Application.SystemColors.Highlight;
            selectedForeground = Application.SystemColors.HighlightText;
            Height = 1;
            showValue = false;
            valueAsPerCent = true;
            AutoSize = false;
        }

        #region Properties

        #region Maximum

        /// <summary>
        /// Gets or sets the maximum value of the range of the control
        /// </summary>
        public int Maximum
        {
            get => maximum;
            set
            {
                if (maximum != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Maximum));
                    }
                    if (minimum > value)
                    {
                        OnMinimumChanged?.Invoke(this, new PropertyChangedEventArgs<int>(minimum, value));
                        minimum = value;
                    }
                    OnMaximumChanged?.Invoke(this, new PropertyChangedEventArgs<int>(maximum, value));
                    maximum = value;
                    if (this.value > maximum)
                    {
                        OnValueChanged?.Invoke(this, new PropertyChangedEventArgs<int>(this.value, value));
                        this.value = maximum;
                    }
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region Minimum

        /// <summary>
        /// Gets or sets the minimum value of the range of the control
        /// </summary>
        public int Minimum
        {
            get => minimum;
            set
            {
                if (minimum != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Minimum));
                    }
                    if (maximum < value)
                    {
                        OnMaximumChanged?.Invoke(this, new PropertyChangedEventArgs<int>(maximum, value));
                        maximum = value;
                    }
                    OnMinimumChanged?.Invoke(this, new PropertyChangedEventArgs<int>(minimum, value));
                    minimum = value;
                    if (this.value < minimum)
                    {
                        OnValueChanged?.Invoke(this, new PropertyChangedEventArgs<int>(this.value, value));
                        this.value = minimum;
                    }
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region Value

        /// <summary>
        /// Gets or sets the current position of the progress bar
        /// </summary>
        public int Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    if (value < minimum || value > maximum)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Value));
                    }
                    OnValueChanged?.Invoke(this, new PropertyChangedEventArgs<int>(this.value, value));
                    this.value = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region ShowValue

        /// <summary>
        /// Gets or sets whether the value should be reflected inside the component
        /// </summary>
        public bool ShowValue
        {
            get => showValue;
            set
            {
                if (showValue != value)
                {
                    OnShowValueChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(showValue, value));
                    showValue = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region ValueAsPerCent

        /// <summary>
        /// Gets or sets whether the value should be displayed inside the component as a percentage,
        /// provided that the <see cref="ShowValue"/> is <see langword="true" />
        /// </summary>
        public bool ValueAsPerCent
        {
            get => valueAsPerCent;
            set
            {
                if (valueAsPerCent != value)
                {
                    OnValueAsPerCentChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(valueAsPerCent, value));
                    valueAsPerCent = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region SelectedBackground

        /// <summary>
        /// Gets or sets the background color of the progress indicator
        /// </summary>
        public Color SelectedBackground
        {
            get => selectedBackground;
            set
            {
                if (selectedBackground != value)
                {
                    OnSelectedBackgroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(selectedBackground, value));
                    selectedBackground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region SelectedForeground

        /// <summary>
        /// Gets or sets the text color of the progress indicator
        /// </summary>
        public Color SelectedForeground
        {
            get => selectedForeground;
            set
            {
                if (selectedForeground != value)
                {
                    OnSelectedForegroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(selectedForeground, value));
                    selectedForeground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<ProgressBar> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            if (AutoSize)
            {
                base.Measure(new Size(Math.Min((Width ?? Padding.Horizontal) + Margin.Horizontal, availableSize.Width),
                    Math.Min((Height ?? Padding.Vertical) + Margin.Vertical, availableSize.Height)));
                return;
            }

            base.Measure(new Size(Math.Min(Width ?? availableSize.Width, availableSize.Width),
                Math.Min(Height ?? availableSize.Height, availableSize.Height)));
        }

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension())
            {
                return;
            }

            var leftPart = ((value - minimum) * reducedArea.Width) / (maximum - minimum);
            for (var row = 0; row < reducedArea.Height; row++)
            {
                context.SetCursorPos(Padding.Left, Padding.Top + row);
                context.DrawText(new string(' ', leftPart), selectedBackground, selectedForeground);
            }

            if (showValue)
            {
                var text = valueAsPerCent
                    ? $"{(value - minimum) * 100 / (maximum - minimum)}%"
                    : value.ToString();

                var leftIndent = (reducedArea.Width - text.Length) / 2;
                var topIndent = (reducedArea.Height - 1) / 2;
                context.SetCursorPos(Padding.Left + leftIndent, Padding.Top + topIndent);
                if (leftIndent < leftPart)
                {
                    context.DrawText(text.Substring(0, Math.Min(leftPart - leftIndent, text.Length)), selectedBackground, selectedForeground);
                    if (leftPart - leftIndent < text.Length)
                    {
                        context.DrawText(text.Substring(leftPart - leftIndent), Background, Foreground);
                    }
                }
                else
                {
                    context.DrawText(text, Background, Foreground);
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnMaximumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Minimum" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnMinimumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Value" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnValueChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ShowValue" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnShowValueChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ValueAsPerCent" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnValueAsPerCentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedBackground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnSelectedBackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnSelectedForegroundChanged;

        #endregion
    }
}
