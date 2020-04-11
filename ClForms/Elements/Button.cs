using System;
using ClForms.Abstractions;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a common button control
    /// </summary>
    public class Button: ButtonBase, IElementStyle<Button>
    {
        private DialogResult dialogResult;

        /// <summary>
        /// Initialize a new instance <see cref="Button"/>
        /// </summary>
        public Button()
        {
            dialogResult = DialogResult.None;
        }

        /// <summary>
        /// Initialize a new instance <see cref="Button"/>
        /// </summary>
        public Button(string text)
            : this()
        {
            Text = text;
        }

        #region Properties

        #region DialogResult

        /// <summary>
        /// Gets or sets a value that is returned to the parent form when the button is clicked
        /// </summary>
        public DialogResult DialogResult
        {
            get => dialogResult;
            set
            {
                if (dialogResult != value)
                {
                    OnDialogResultChanged?.Invoke(this,
                        new PropertyChangedEventArgs<DialogResult>(dialogResult, value));
                    dialogResult = value;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Call event of the <see cref="ButtonBase.OnClick" />
        /// </summary>
        protected override void Click(EventArgs e)
        {
            var wndInternal = ParentWindow();
            if (wndInternal != null)
            {
                wndInternal.DialogResult = DialogResult;
            }
            base.Click(e);
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Button> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="ButtonBase.GetTextPresenter"/>
        protected override string GetTextPresenter() => Text;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="DialogResult" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<DialogResult>> OnDialogResultChanged;

        #endregion
    }
}
