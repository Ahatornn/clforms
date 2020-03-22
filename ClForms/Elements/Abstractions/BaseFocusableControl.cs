using System;
using ClForms.Abstractions;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Themes;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Specifies the basic functionality common to focusable controls
    /// </summary>
    public abstract class BaseFocusableControl: Control, IFocusableControl
    {
        private bool isDisabled;
        private bool isFocus;
        private Color focusBackground;
        private Color focusForeground;
        private Color disabledBackground;
        private Color disabledForeground;
        private int tabIndex;
        private bool tabStop;

        /// <summary>
        /// Initialize a new instance <see cref="BaseFocusableControl"/>
        /// </summary>
        protected BaseFocusableControl()
        {
            Background = Application.SystemColors.ButtonFace;
            Foreground = Application.SystemColors.ButtonText;
            disabledBackground = Application.SystemColors.ButtonInactiveFace;
            disabledForeground = Application.SystemColors.ButtonInactiveText;
            focusBackground = Application.SystemColors.ButtonFocusedFace;
            focusForeground = Application.SystemColors.ButtonFocusedText;
        }

        #region Properties

        #region IsFocus

        /// <inheritdoc cref="IFocusableControl.IsFocus"/>
        public bool IsFocus
        {
            get => isFocus;
            set
            {
                if (isFocus != value)
                {
                    OnFocusChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(isFocus, value));
                    isFocus = value;
                    if (isFocus)
                    {
                        OnEnter?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        OnLeave?.Invoke(this, EventArgs.Empty);
                    }
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region FocusBackground

        /// <inheritdoc cref="IFocusableControl.FocusBackground"/>
        public Color FocusBackground
        {
            get => focusBackground;
            set
            {
                if (focusBackground != value)
                {
                    OnFocusBackgroundChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(focusBackground, value));
                    focusBackground = value;
                    if (isFocus)
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion
        #region FocusForeground

        /// <inheritdoc cref="IFocusableControl.FocusForeground"/>
        public Color FocusForeground
        {
            get => focusForeground;
            set
            {
                if (focusForeground != value)
                {
                    OnFocusForegroundChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(focusForeground, value));
                    focusForeground = value;
                    if (isFocus)
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion
        #region IsDisabled

        /// <summary>
        /// Gets or sets a value indicating whether the control cannot respond to user interaction
        /// </summary>
        public bool IsDisabled
        {
            get => isDisabled;
            set
            {
                if (isDisabled != value)
                {
                    OnDisabledChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(isDisabled, value));
                    isDisabled = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region DisabledBackground

        /// <summary>
        /// Gets or sets a value to display of background when the control is disabled
        /// </summary>
        public Color DisabledBackground
        {
            get => disabledBackground;
            set
            {
                if (disabledBackground != value)
                {
                    OnDisabledBackgroundChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(disabledBackground, value));
                    disabledBackground = value;
                    if (isDisabled)
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion
        #region DisabledForeground

        /// <summary>
        /// Gets or sets a value to display of foreground when the control is disabled
        /// </summary>
        public Color DisabledForeground
        {
            get => disabledForeground;
            set
            {
                if (disabledForeground != value)
                {
                    OnDisabledForegroundChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(disabledForeground, value));
                    disabledForeground = value;
                    if (isDisabled)
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion
        #region TabIndex

        /// <inheritdoc cref="IFocusableControl.TabIndex"/>
        public int TabIndex
        {
            get => tabIndex;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("");
                }

                if (tabIndex != value)
                {
                    OnTabIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(tabIndex, value));
                    tabIndex = value;
                }
            }
        }

        #endregion
        #region TabStop

        /// <inheritdoc cref="IFocusableControl.TabStop"/>
        public bool TabStop
        {
            get => tabStop;
            set
            {
                if (tabStop != value)
                {
                    OnTabStopChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(tabStop, value));
                    tabStop = value;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="IFocusableControl.CanFocus"/>
        public bool CanFocus() => !(isDisabled || tabStop);

        /// <summary>
        /// Handles a keystroke
        /// </summary>
        /// <param name="keyInfo"></param>
        public void InputAction(ConsoleKeyInfo keyInfo) => InputActionInternal(keyInfo);

        /// <inheritdoc cref="IFocusableControl.SetFocus"/>
        public void SetFocus()
        {
            var wnd = ParentWindow();
            wnd.TrySetFocus(this);
        }

        /// <summary>
        /// Internal keystroke processing
        /// </summary>
        protected abstract void InputActionInternal(ConsoleKeyInfo keyInfo);

        protected virtual Color GetRenderBackColor()
            => isDisabled
                ? disabledBackground
                : isFocus
                    ? focusBackground
                    : Background;

        protected virtual Color GetRenderForeColor()
            => isDisabled
                ? disabledForeground
                : isFocus
                    ? focusForeground
                    : Foreground;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="IsFocus" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnFocusChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsDisabled" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnDisabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FocusBackground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnFocusBackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FocusForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnFocusForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="DisabledBackground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnDisabledBackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="DisabledForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnDisabledForegroundChanged;

        /// <inheritdoc cref="IFocusableControl.OnEnter"/>
        public event EventHandler<EventArgs> OnEnter;

        /// <inheritdoc cref="IFocusableControl.OnLeave"/>
        public event EventHandler<EventArgs> OnLeave;

        /// <inheritdoc cref="IFocusableControl.OnTabIndexChanged"/>
        public event EventHandler<PropertyChangedEventArgs<int>> OnTabIndexChanged;

        /// <inheritdoc cref="IFocusableControl.OnTabStopChanged"/>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnTabStopChanged;

        #endregion
    }
}
