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
    /// Represents a control to give the user an option, such as true/false or yes/no
    /// </summary>
    public class CheckBox: ButtonBase, IElementStyle<CheckBox>
    {
        private CheckState checkState;
        private bool threeState;

        /// <summary>
        /// Initialize a new instance <see cref="CheckBox"/>
        /// </summary>
        public CheckBox()
        {
            checkState = CheckState.Unchecked;
            Padding = Thickness.Empty;
            Background = Color.NotSet;
            Foreground = Application.SystemColors.ButtonText;
            DisabledForeground = Application.SystemColors.ButtonInactiveFace;
            FocusForeground = Application.SystemColors.ButtonFocusedFace;
            TextAlignment = TextAlignment.Left;
            threeState = false;
        }

        /// <summary>
        /// Initialize a new instance <see cref="CheckBox"/> with specified text
        /// </summary>
        public CheckBox(string text)
        : this()
        {
            Text = text;
        }

        #region Properties

        #region Checked

        /// <summary>
        /// Gets or set a value indicating whether the <see cref="CheckBox"/> is in the checked state
        /// </summary>
        public bool Checked
        {
            get => checkState == CheckState.Checked ||
                   checkState == CheckState.Indeterminate;
            set
            {
                if (value != Checked)
                {
                    CheckState = value ? CheckState.Checked : CheckState.Unchecked;
                }
            }
        }

        #endregion
        #region CheckState

        /// <summary>
        /// Gets or sets the state of the <see cref="CheckBox"/>
        /// </summary>
        public CheckState CheckState
        {
            get => checkState;
            set
            {
                if (checkState != value)
                {
                    OnCheckStateChanged?.Invoke(this, new PropertyChangedEventArgs<CheckState>(checkState, value));
                    var flag = Checked;
                    checkState = value;
                    if (flag != Checked)
                    {
                        OnCheckedChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(flag, Checked));
                    }
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region ThreeState

        /// <summary>
        /// Gets or sets a value indicating whether the CheckBox will allow three check states rather than two
        /// </summary>
        public bool ThreeState
        {
            get => threeState;
            set
            {
                if (threeState != value)
                {
                    OnThreeStateChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(threeState, value));
                    threeState = value;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

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
                        : FindParentInternal(x => x.Foreground != Color.NotSet)?.Foreground
                          ?? Color.NotSet;
                context.SetCursorPos(RenderTextPosition.X + Padding.Left, RenderTextPosition.Y + Padding.Top);
                context.DrawText(GetGlyph(), foreColor);
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<CheckBox> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="ButtonBase.Click"/>
        protected override void Click(EventArgs e)
        {
            switch (CheckState)
            {
                case CheckState.Unchecked:
                    CheckState = CheckState.Checked;
                    break;
                case CheckState.Checked:
                    if (ThreeState)
                    {
                        CheckState = CheckState.Indeterminate;
                        break;
                    }
                    CheckState = CheckState.Unchecked;
                    break;
                default:
                    CheckState = CheckState.Unchecked;
                    break;
            }
            base.Click(e);
        }

        /// <inheritdoc cref="ButtonBase.GetTextPresenter"/>
        protected override string GetTextPresenter() => $"{GetGlyph()}  {Text}";

        private char GetGlyph() => Application.Environment.GetCheckStateChar(CheckState);

        /// <inheritdoc cref="ButtonBase.GetRenderForeColor"/>
        protected override Color GetRenderForeColor() => Foreground;

        /// <inheritdoc cref="ButtonBase.GetRenderBackColor"/>
        protected override Color GetRenderBackColor() => Background;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Checked" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnCheckedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckState" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<CheckState>> OnCheckStateChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ThreeState" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnThreeStateChanged;

        #endregion
    }
}
