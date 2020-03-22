using System.Collections.Generic;
using System.Linq;
using ClForms.Abstractions.Engine;
using ClForms.Elements.Abstractions;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace ClForms.Helpers
{
    internal static class MainMenuHelper
    {
        internal static int DrawMenuItem(this MainMenu mainMenu,
            MenuItemBase item,
            IDrawingContext context,
            Color backColor,
            Color foreColor)
        {
            if (item.Text.IndexOf('&') == -1 || !item.Enabled)
            {
                context.DrawText($" {item.Text.Replace("&", string.Empty)} ",
                    item.Enabled
                        ? backColor
                        : mainMenu.DisabledBackground,
                    item.Enabled
                        ? foreColor
                        : mainMenu.DisabledForeground);
                return item.Text.Length;
            }

            var leftText = " " + item.Text.Substring(0, item.Text.IndexOf('&'));
            var middleText = item.Text.Substring(item.Text.IndexOf('&') + 1, 1);
            var rightText = item.Text.Substring(item.Text.IndexOf('&') + 2) + " ";

            context.DrawText(leftText, backColor, foreColor);
            context.DrawText(middleText, backColor, mainMenu.MnemonicForeground);
            context.DrawText(rightText, backColor, foreColor);

            return leftText.Length + middleText.Length + rightText.Length;
        }

        internal static int GetItemTextLength(IEnumerable<MenuItemBase> items)
            => items.Select(GetItemTextLength).Max();

        internal static int GetItemTextLength(MenuItemBase item) => item.MenuItemLength;
    }
}
