using System;
using System.Globalization;
using System.Linq;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;

namespace ClForms.Elements.Menu
{
    /// <summary>
    /// Common menu item
    /// </summary>
    public class MenuItem: MenuItemBase
    {
        private short mnemonic;

        /// <summary>
        /// Initialize a new instance <see cref="MenuItem"/>
        /// </summary>
        public MenuItem()
        : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initialize a new instance <see cref="MenuItem"/> with <see cref="MenuItemBase.Text"/> value
        /// </summary>
        public MenuItem(string text)
        : this(text, null)
        {
        }

        /// <summary>
        /// Initialize a new instance <see cref="MenuItem"/> with <see cref="MenuItemBase.Text"/> value and
        /// other properties
        /// </summary>
        public MenuItem(string text,
            Shortcut? shortcut,
            bool enabled = true,
            bool visible = true,
            bool @checked = false)
        {
            Text = text;
            Shortcut = shortcut;
            Enabled = enabled;
            Visible = visible;
            Checked = @checked;
            mnemonic = -1;
            ShowShortcut = true;
            Items = new MenuItemCollection(this);
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating the collection of <see cref="MenuItemBase"/> objects associated with the menu
        /// </summary>
        public MenuItemCollection Items { get; }

        /// <summary>
        /// Gets or sets a value indicating the shortcut key associated with the menu item
        /// </summary>
        public Shortcut? Shortcut { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the shortcut key that is associated
        /// with the menu item is displayed next to the menu item caption
        /// </summary>
        public bool ShowShortcut { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a check mark appears next to the text of the menu item
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets a value indicating the mnemonic character that is associated with this menu item
        /// </summary>
        public char Mnemonic
        {
            get
            {
                if (mnemonic == -1)
                {
                    mnemonic = (short) GetMnemonic(Text, true);
                }
                return (char) mnemonic;
            }
        }

        /// <inheritdoc />
        public override int MenuItemLength => 1 + Text.Length + 6 + (Shortcut?.ToString().Length ?? 0);

        #endregion

        #region Methods

        internal void Click() => OnClick?.Invoke(this, EventArgs.Empty);

        /// <inheritdoc />
        public override void OnRender(IDrawingContext context,
            Themes.Color backColor,
            Themes.Color foreColor,
            MainMenu mainMenu,
            int itemAreaWidth)
        {
            var firstArea = $"{(Checked ? Application.Environment.GetCheckStateChar(CheckState.Checked) : ' ')}";
            context.DrawText(firstArea, backColor, foreColor);

            var textContentLength = mainMenu.DrawMenuItem(this, context, backColor, foreColor);

            if (ShowShortcut && !Items.Any())
            {
                var shortcutText = Shortcut?.ToString() ?? string.Empty;
                context.DrawText((shortcutText + " ")
                    .PadLeft(itemAreaWidth - firstArea.Length - textContentLength -
                             (Text.IndexOf('&') > -1 ? 0 : 2)),
                    backColor,
                    foreColor);
            }
            if (Items.Any())
            {

                context.DrawText(Application.Environment.MenuSubItemsMarker
                        .ToString()
                        .PadLeft(itemAreaWidth - firstArea.Length - textContentLength - (Text.IndexOf('&') > -1 ? 0 : 2)),
                    backColor,
                    foreColor);
            }
        }

        public static char GetMnemonic(string text, bool bConvertToUpperCase)
        {
            var ch = char.MinValue;
            if (text != null)
            {
                var length = text.Length;
                for (var index = 0; index < length - 1; ++index)
                {
                    if (text[index] == '&')
                    {
                        if (text[index + 1] == '&')
                        {
                            ++index;
                        }
                        else
                        {
                            ch = !bConvertToUpperCase ? char.ToLower(text[index + 1], CultureInfo.CurrentCulture) : char.ToUpper(text[index + 1], CultureInfo.CurrentCulture);
                            break;
                        }
                    }
                }
            }
            return ch;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the menu item is clicked or selected using a shortcut key or access key defined for the menu item
        /// </summary>
        public event EventHandler OnClick;

        #endregion
    }
}
