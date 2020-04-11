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
    /// Enables the user to select a single option from a group of choices when paired with other <see cref="RadioButton"/> controls
    /// </summary>
    public class RadioButton: ButtonBase, IElementStyle<RadioButton>
    {
        private bool isChecked;

        /// <summary>
        /// Initialize a new instance <see cref="RadioButton"/>
        /// </summary>
        public RadioButton()
        {
            Init();
        }

        /// <summary>
        /// Initialize a new instance <see cref="RadioButton"/> with value of <see cref="ButtonBase.Text"/> property
        /// </summary>
        public RadioButton(string text)
            : base(text)
        {
            Init();
        }

        #region Properties

        #region Checked

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked
        /// </summary>
        public bool Checked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    OnCheckedChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(isChecked, value));
                    isChecked = value;
                    PerformAutoUpdates();
                    InvalidateVisual();
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<RadioButton> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="ButtonBase.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);

            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (!reducedArea.HasEmptyDimension() ||
                string.IsNullOrWhiteSpace(GetTextPresenter()))
            {
                var foreColor = IsDisabled
                    ? DisabledForeground
                    : IsFocus
                        ? FocusForeground
                        : Foreground;
                context.SetCursorPos(RenderTextPosition.X + Padding.Left, RenderTextPosition.Y + Padding.Top);
                context.DrawText(GetGlyph(), foreColor);
            }
        }

        /// <inheritdoc cref="ButtonBase.GetRenderBackColor"/>
        protected override Color GetRenderBackColor() => Background;

        /// <inheritdoc cref="ButtonBase.GetRenderForeColor"/>
        protected override Color GetRenderForeColor()
            => IsDisabled
                ? DisabledForeground
                : Foreground;

        /// <inheritdoc cref="ButtonBase.Click"/>
        protected override void Click(EventArgs e)
        {
            Checked = true;
            base.Click(e);
        }

        /// <inheritdoc cref="ButtonBase.GetTextPresenter"/>
        protected override string GetTextPresenter()
            => $"{GetGlyph()}  {Text}";

        private void PerformAutoUpdates()
        {
            if (isChecked && Parent != null)
            {
                foreach (var item in Parent)
                {
                    if (item != this && item is RadioButton radioButton)
                    {
                        radioButton.Checked = false;
                    }
                }
            }
        }

        private char GetGlyph() => Application.Environment.GetRadioCheckChar(isChecked);

        private void Init()
        {
            Padding = Thickness.Empty;
            Background = Color.NotSet;
            Foreground = Application.SystemColors.ButtonText;
            DisabledForeground = Application.SystemColors.ButtonInactiveFace;
            FocusForeground = Application.SystemColors.ButtonFocusedFace;
            TextAlignment = TextAlignment.Left;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Checked" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnCheckedChanged;

        #endregion
    }
}
