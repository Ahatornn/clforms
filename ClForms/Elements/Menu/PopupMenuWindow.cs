using ClForms.Abstractions;
using ClForms.Common;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using System;
using System.Linq;

namespace ClForms.Elements.Menu
{
    internal class PopupMenuWindow : Window, IElementStyle<PopupMenuWindow>
    {
        private MainMenu ownerMenu;
        private PopupMenuContent popupContent;
        internal Point PreferredLocation => ownerMenu.GetPopupPosition(this);

        public PopupMenuWindow(MenuItemCollection items,
            MainMenu ownerMenu)
        {
            this.ownerMenu = ownerMenu;
            Background = ownerMenu.Background;
            Foreground = ownerMenu.Foreground;
            HideTitle = true;
            WindowState = ControlState.Normal;
            BorderThickness = Thickness.Empty;
            Height = Math.Min(items.Count(x => x.Visible) + 2, Application.Environment.WindowHeight);
            CreateContent(items);
        }

        #region Properties

        /// <summary>
        /// Gets the number of items in a list
        /// </summary>
        public int ItemCount => popupContent.Items.Count;

        /// <summary>
        /// Gets the selected item in a list
        /// </summary>
        public MenuItem SelectedMenuItem => popupContent.SelectedIndex == -1
            ? null
            : popupContent.Items[popupContent.SelectedIndex] as MenuItem;

        /// <summary>
        /// Gets the <see cref="Rect"/> of selected item area
        /// </summary>
        public Rect SelectedItemBounds => popupContent.SelectedItemBounds;

        /// <summary>
        /// Returns the maximum length of an item in a list
        /// </summary>
        public int GetMaxItemLength() => MainMenuHelper.GetItemTextLength(popupContent.Items);

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<PopupMenuWindow> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc />
        public override void Close()
        {
            ownerMenu = null;
            popupContent.OnSelectedItemClick -= SelectedItemClick;
            base.Close();
        }

        private void SelectedItemClick(object sender, MenuItemBase e) => OnSelectedItemClick?.Invoke(this, e as MenuItem);

        private void CreateContent(MenuItemCollection items)
        {
            popupContent = new PopupMenuContent(items, ownerMenu)
            {
                Height = this.Height - 2,
            };
            popupContent.OnSelectedItemClick += SelectedItemClick;
            var selectedItem = items.FirstOrDefault(x => x.Visible && x.Enabled);
            if (selectedItem != null)
            {
                popupContent.SelectedIndex = items.IndexOf(selectedItem);
            }
            var border = new PopupMenuContentBox(items)
            {
                BorderThickness = new Thickness(2, 1),
            };
            border.AddContent(popupContent);
            AddContent(border);
        }

        /// <inheritdoc cref="Window.InputActionInternal"/>
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Escape ||
                keyInfo.Key == ConsoleKey.UpArrow &&
                popupContent.SelectedIndex == 0)
            {
                Close();
                return;
            }

            var passedKey = keyInfo;
            if (keyInfo.Key == ConsoleKey.Tab)
            {
                passedKey = keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
                    ? new ConsoleKeyInfo((char)(int) ConsoleKey.UpArrow, ConsoleKey.UpArrow, false, false, false)
                    : new ConsoleKeyInfo((char)(int) ConsoleKey.DownArrow, ConsoleKey.DownArrow, false, false, false);
            }
            base.InputActionInternal(passedKey);
            if (keyInfo.Key == ConsoleKey.LeftArrow ||
                keyInfo.Key == ConsoleKey.RightArrow)
            {
                ownerMenu?.InputAction(keyInfo);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the selected item is clicked
        /// </summary>
        public event EventHandler<MenuItem> OnSelectedItemClick;

        #endregion
    }
}
