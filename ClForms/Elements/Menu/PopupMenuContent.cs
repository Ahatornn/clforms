using System.Collections.Generic;
using ClForms.Abstractions.Engine;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;

namespace ClForms.Elements.Menu
{
    internal class PopupMenuContent: ListBoxBase<MenuItemBase>
    {
        private readonly MainMenu mainMenu;
        public PopupMenuContent(IEnumerable<MenuItemBase> items, MainMenu mainMenu)
        {
            this.mainMenu = mainMenu;
            Background = mainMenu.Background;
            Foreground = mainMenu.Foreground;
            FocusBackground = mainMenu.Background;
            FocusForeground = mainMenu.Foreground;
            SelectedBackground = mainMenu.SelectedBackground;
            SelectedForeground = mainMenu.SelectedForeground;
            DisabledBackground = mainMenu.DisabledBackground;
            DisabledForeground = mainMenu.DisabledForeground;
            Items.AddRange(items);
        }

        /// <inheritdoc cref="ListBoxBase{T}.OnRenderItemInternal"/>
        protected override void OnRenderItemInternal(IDrawingContext context, MenuItemBase item, int itemAreaWidth)
        {

            var backColor = GetRenderBackColor();
            var foreColor = GetRenderForeColor();
            if (SelectedItems.Contains(item))
            {
                backColor = SelectedBackground;
                foreColor = SelectedForeground;
            }

            if (!item.Enabled)
            {
                backColor = DisabledBackground;
                foreColor = DisabledForeground;
            }

            item.OnRender(context, backColor, foreColor, mainMenu, itemAreaWidth);
        }

        /// <inheritdoc cref="ListBoxBase{T}.CanSelectItem"/>
        protected override bool CanSelectItem(MenuItemBase item) => item.Enabled;

        /// <inheritdoc cref="ListBoxBase{T}.GetItemTextLength"/>
        protected override int GetItemTextLength(MenuItemBase item) => MainMenuHelper.GetItemTextLength(item);
    }
}
