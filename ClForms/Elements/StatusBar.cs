using System;
using System.Linq;
using ClForms.Abstractions;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows status bar control
    /// </summary>
    public class StatusBar: DockPanel, IElementStyle<StatusBar>
    {
        private Color mnemonicForeground;

        /// <summary>
        /// Initialize a new instance <see cref="StatusBar"/>
        /// </summary>
        public StatusBar()
        {
            Background = Application.SystemColors.MenuBackground;
            Foreground = Application.SystemColors.MenuForeground;
            mnemonicForeground = Application.SystemColors.MnemonicForeground;
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating the color of the assigned character associated with accessing the buttons
        /// </summary>
        public Color MnemonicForeground
        {
            get => mnemonicForeground;
            set
            {
                if (mnemonicForeground != value)
                {
                    OnMnemonicForegroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(mnemonicForeground, value));
                    mnemonicForeground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<StatusBar> styleAction) => styleAction?.Invoke(this);

        /// <summary>
        /// Adds a label with default values and the specified text
        /// </summary>
        public void AddLabel(string text) => AddLabel(text, Dock.Left);

        /// <summary>
        /// Adds a label with default values and the specified text and the <see cref="Dock"/> property value
        /// </summary>
        public void AddLabel(string text, Dock dock)
            => AddContent(new Label(text) { Margin = new Thickness(1, 0) }, dock);

        /// <summary>
        /// Adds a button to the status bar panel with function key access
        /// </summary>
        public void AddButton(string text, StatusBarShortcutKey shortcutKey, Action action)
            => AddButton(text, shortcutKey, action, Dock.Left);

        /// <summary>
        /// Adds a button to the status bar panel with function key access and the <see cref="Dock"/> property value
        /// </summary>
        public void AddButton(string text, StatusBarShortcutKey shortcutKey, Action action, Dock dock)
        {
            var sbButton = new StatusBarButton(this, shortcutKey, text)
            {
                Margin = new Thickness(1, 0)
            };
            sbButton.OnClick += (sender, args) => action.Invoke();
            AddContent(sbButton, dock);
        }

        /// <summary>
        /// Adds a help button to the status bar panel, with a standard help window
        /// </summary>
        public void AddHelpButton(string text = "Help") => AddButton(text, StatusBarShortcutKey.F1, HelpWindow.ShowHelp);

        /// <inheritdoc cref="MultipleContentControl.AddContent"/>
        public override void AddContent(Control content, Dock dock)
        {
            if (dock != Dock.Left && dock != Dock.Right)
            {
                throw new InvalidOperationException("Only left or right of Dock value can use in StatusBar control");
            }

            if (content.Margin == Thickness.Empty)
            {
                content.Margin = new Thickness(1, 0);
            }
            base.AddContent(content, dock);
        }

        /// <inheritdoc cref="DockPanel.Measure"/>
        public override void Measure(Size availableSize) => base.Measure(new Size(availableSize.Width, 1));

        /// <summary>
        /// Trying to find an item associated with the specified <see cref="ConsoleKeyInfo"/>
        /// </summary>
        public bool FindAndClick(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key >= ConsoleKey.F1 && keyInfo.Key <= ConsoleKey.F24 &&
                !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt) &&
                !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control) &&
                !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                var shortcutKey = (StatusBarShortcutKey) (int) keyInfo.Key;
                var sbButton = GetAllControls<StatusBarButton>().FirstOrDefault(x => x.StatusBarShortcutKey == shortcutKey);
                if (sbButton != null)
                {
                    sbButton.Click();
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc cref="MultipleContentControl.AddContentInterceptor"/>
        protected override void AddContentInterceptor(Control content)
        {
            SetDock(content, Dock.Left);
            if (content.Margin == Thickness.Empty)
            {
                content.Margin = new Thickness(1, 0);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="MnemonicForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnMnemonicForegroundChanged;

        #endregion
    }
}
