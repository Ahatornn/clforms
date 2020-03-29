using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Elements.Menu
{
    /// <summary>
    /// Represents the menu structure of a window
    /// </summary>
    public class MainMenu: Control, IElementStyle<MainMenu>
    {
        private Color mnemonicForeground;
        private MenuItemBase selectedLevelItem;
        private Color selectedBackground;
        private Color selectedForeground;
        private Color disabledBackground;
        private Color disabledForeground;
        private readonly Stack<PopupMenuWindow> popups;

        /// <summary>
        /// Initialize a new instance <see cref="MainMenu"/>
        /// </summary>
        public MainMenu()
        {
            Background = Application.SystemColors.MenuBackground;
            Foreground = Application.SystemColors.MenuForeground;
            selectedBackground = Application.SystemColors.MenuHighlight;
            selectedForeground = Application.SystemColors.MenuHighlightText;
            disabledBackground = Application.SystemColors.MenuInactiveFace;
            disabledForeground = Application.SystemColors.MenuInactiveText;
            mnemonicForeground = Application.SystemColors.MnemonicForeground;
            Items = new MenuItemCollection(null);
            IsOpened = false;
            selectedLevelItem = null;
            popups = new Stack<PopupMenuWindow>();
        }

        #region Properties

        #region MnemonicForeground

        /// <summary>
        /// Gets or sets a value indicating the color of the assigned character associated with accessing the menu item
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
        #region SelectedBackground

        /// <summary>
        /// Gets or sets a value to display of background when the item is selected
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
        /// Gets or sets a value to display of foreground when the item is selected
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
                    OnDisabledBackgroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(disabledBackground, value));
                    disabledBackground = value;
                    InvalidateVisual();
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
                    OnDisabledForegroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(disabledForeground, value));
                    disabledForeground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating the collection of <see cref="MenuItemBase"/> objects associated with the menu
        /// </summary>
        public MenuItemCollection Items { get; }

        /// <summary>
        /// Gets a value indicating that menu is opened
        /// </summary>
        public bool IsOpened { get; private set; }

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
            => base.Measure(new Size(Math.Min(availableSize.Width,
                    2 + Items.Where(x => x.Visible)
                        .Sum(x => x.Text.Length + 2) -
                    Items.Where(x => x.Visible)
                        .Count(x => x.Text.IndexOf('&') > -1)),
                1));

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            var visibleItems = Items.Where(x => x.Visible)
                .ToList();
            if (context.ContextBounds.HasEmptyDimension() || !visibleItems.Any())
            {
                return;
            }

            var leftIndent = 1;
            for (var i = 0; i < visibleItems.Count; i++)
            {
                var item = visibleItems[i];
                var backColor = Background;
                var foreColor = Foreground;
                if (item == selectedLevelItem)
                {
                    backColor = SelectedBackground;
                    foreColor = SelectedForeground;
                }
                context.SetCursorPos(leftIndent, 0);
                var itemText = $" {item.Text.Replace("&", string.Empty)} ";
                if (itemText.Length + leftIndent > context.ContextBounds.Width - 1 && i != visibleItems.Count - 1)
                {
                    context.DrawText(" … ", backColor, foreColor);
                    break;
                }

                this.DrawMenuItem(item, context, backColor, foreColor);
                leftIndent += itemText.Length;
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<MainMenu> styleAction) => styleAction?.Invoke(this);

        /// <summary>
        /// Manually open a <see cref="MainMenu"/>
        /// </summary>
        public void Open()
        {
            OnOpen?.Invoke(this, EventArgs.Empty);
            selectedLevelItem = Items.FirstOrDefault(x => x.Enabled && x.Visible);
            IsOpened = true;
            InvalidateVisual();
        }

        /// <summary>
        /// Manually closes a <see cref="MainMenu"/>
        /// </summary>
        public void Close()
        {
            OnClose?.Invoke(this, EventArgs.Empty);
            IsOpened = false;
            selectedLevelItem = null;
            InvalidateVisual();
            popups.Clear();
        }

        /// <summary>
        /// Manually handles keystrokes
        /// </summary>
        public void InputAction(ConsoleKeyInfo keyInfo)
        {
            var shouldOpen = false;
            MenuItemBase nextItem = null;
            if (keyInfo.Key == ConsoleKey.RightArrow ||
               keyInfo.Key == ConsoleKey.Tab && !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                if (popups.Any())
                {
                    var wnd = popups.Peek();
                    if (wnd.SelectedMenuItem != null)
                    {
                        if (wnd.SelectedMenuItem.Items.Any())
                        {
                            CreatePopupWindow(wnd.SelectedMenuItem.Items);
                            return;
                        }

                        CloseAllPopups();
                        Application.DoEvents();
                        popups.Clear();
                        shouldOpen = true;
                    }
                }
                var index = Items.IndexOf(selectedLevelItem);
                nextItem = Items.Skip(index > -1 ? index + 1 : 0).FirstOrDefault(x => x.Enabled && x.Visible);
            }

            if (keyInfo.Key == ConsoleKey.LeftArrow ||
                keyInfo.Key == ConsoleKey.Tab && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                if (popups.Any())
                {
                    var wnd = popups.Peek();
                    PopupClosed(this, EventArgs.Empty);
                    wnd.Close();
                    Application.DoEvents();
                    if (popups.Any())
                    {
                        return;
                    }

                    shouldOpen = true;
                }
                var index = Items.IndexOf(selectedLevelItem);
                nextItem = index == -1
                    ? Items.FirstOrDefault(x => x.Enabled && x.Visible)
                    : Items.Take(index).LastOrDefault(x => x.Enabled && x.Visible);
            }

            if (selectedLevelItem != nextItem && nextItem is MenuItem menuItem)
            {
                selectedLevelItem = menuItem;
                InvalidateVisual();
                if (shouldOpen && menuItem.Items.Any())
                {
                    Application.DoEvents();
                    CreatePopupWindow(menuItem.Items);
                }
            }

            if ((keyInfo.Key == ConsoleKey.Spacebar ||
                 keyInfo.Key == ConsoleKey.Enter ||
                 keyInfo.Key == ConsoleKey.DownArrow) && selectedLevelItem is MenuItem item)
            {
                if (!item.Items.Any() && keyInfo.Key != ConsoleKey.DownArrow)
                {
                    Close();
                    Application.DoEvents();
                    item.Click();
                }
                else
                {
                    CreatePopupWindow(item.Items);
                }
            }
        }

        /// <summary>
        /// Trying to find an item associated with the specified <see cref="ConsoleKeyInfo"/>
        /// </summary>
        public bool FindAndClick(ConsoleKeyInfo keyInfo)
        {
            MenuItem menuItem = null;
            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                menuItem = GetMenuItemByMnemonic(char.ToUpper(keyInfo.KeyChar, CultureInfo.CurrentCulture), Items);
            }

            if (menuItem == null)
            {
                menuItem = GetMenuItemByShortcut(new Shortcut(keyInfo.Key,
                    keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift),
                    keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt),
                    keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control)), Items);
            }

            if (menuItem?.Enabled == true &&
                menuItem?.Visible == true)
            {
                menuItem.Click();
                if (IsOpened)
                {
                    Close();
                }
                return true;
            }

            return false;
        }

        internal Point GetPopupPosition(PopupMenuWindow wndPopup)
        {
            var wnds = popups.ToArray();
            if (wnds.All(x => x != wndPopup))
            {
                throw new ArgumentException("Current main window hasn't specified popup");
            }

            if (selectedLevelItem == null)
            {
                throw new InvalidOperationException("Can't show popup menu, because hasn't selected menu item");
            }

            var result = Bounds.Location + Parent.Bounds.Location + new Point(0, 1);
            foreach (var item in Items.Where(x => x.Visible))
            {
                if (item == selectedLevelItem)
                {
                    break;
                }

                result.X += item.Text.Replace("&", string.Empty).Length + 2;
            }

            var preItemWidth = 0;
            foreach (var popup in popups.ToArray().Reverse())
            {
                if (popup == wndPopup)
                {
                    break;
                }

                var rect = popup.SelectedItemBounds;
                result.X += rect.Width + 4;
                result.Y += rect.Top - 1;
                ActualizePosition(ref result, popup, preItemWidth);
                preItemWidth = popup.GetMaxItemLength() + 2;
            }
            var itemWidth = wndPopup.GetMaxItemLength() + 2;
            ActualizePosition(ref result, wndPopup, itemWidth);
            return result;
        }

        private void ActualizePosition(ref Point rect, PopupMenuWindow popup, int itemWidth)
        {
            var maxWidth = popup.GetMaxItemLength() + 2;
            if (rect.X + maxWidth > Application.Environment.WindowWidth)
            {
                rect.X = Math.Max(0, rect.X - maxWidth - itemWidth - 4);
            }

            var height = popup.ItemCount + 2;
            if (rect.Y + height > Application.Environment.WindowHeight)
            {
                rect.Y = Math.Max(0, Application.Environment.WindowHeight - height);
            }
        }

        private void CreatePopupWindow(MenuItemCollection items)
        {
            if (items?.Any(x => x.Visible) != true)
            {
                return;
            }

            OnPopup?.Invoke(this, items);
            var popup = new PopupMenuWindow(items, this);
            popup.OnClosed += PopupClosed;
            popup.OnSelectedItemClick += PopupSelectedItemClick;
            popups.Push(popup);
            popup.Show();
        }

        private void PopupSelectedItemClick(object sender, MenuItem e)
        {
            CloseAllPopups();
            Close();
            Application.DoEvents();
            e.Click();
        }

        private void PopupClosed(object sender, EventArgs e)
        {
            var popup = popups.Pop();
            popup.OnClosed -= PopupClosed;
            popup.OnSelectedItemClick -= PopupSelectedItemClick;
        }

        private void CloseAllPopups()
        {
            foreach (var wnd in popups.ToArray())
            {
                wnd.OnClosed -= PopupClosed;
                wnd.OnSelectedItemClick -= PopupSelectedItemClick;
                wnd.Close();
            }
        }

        private static MenuItem GetMenuItemByMnemonic(char mnemonicChar, MenuItemCollection items)
        {
            if (items == null || items.Count == 0)
            {
                return null;
            }
            foreach (var item in items.Where(x => x is MenuItem).Cast<MenuItem>())
            {
                if (item.Mnemonic == mnemonicChar)
                {
                    return item;
                }
                var result = GetMenuItemByMnemonic(mnemonicChar, item.Items);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private static MenuItem GetMenuItemByShortcut(Shortcut shortcut, MenuItemCollection items)
        {
            if (items == null || items.Count == 0)
            {
                return null;
            }
            foreach (var item in items.Where(x => x is MenuItem).Cast<MenuItem>())
            {
                if (item.Shortcut.HasValue &&
                    item.Shortcut.Value == shortcut)
                {
                    return item;
                }
                var result = GetMenuItemByShortcut(shortcut, item.Items);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the main menu open
        /// </summary>
        public event EventHandler OnOpen;

        /// <summary>
        /// Occurs when the main menu collapses
        /// </summary>
        public event EventHandler OnClose;

        /// <summary>
        /// Occurs before the main menu open sub menu
        /// </summary>
        public event EventHandler<MenuItemCollection> OnPopup;

        /// <summary>
        /// Occurs when the value of the <see cref="MnemonicForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnMnemonicForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedBackground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnSelectedBackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnSelectedForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="DisabledBackground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnDisabledBackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="DisabledForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnDisabledForegroundChanged;

        #endregion
    }
}
