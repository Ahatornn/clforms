using System;
using ClForms.Common;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements.Menu
{
    /// <summary>
    /// Represents the menu structure of a window
    /// </summary>
    public class MainMenu: Control
    {
        public Color DisabledBackground { get; set; }

        public Color DisabledForeground { get; set; }

        public Color MnemonicForeground { get; set; }

        public Color SelectedBackground { get; set; }

        public Color SelectedForeground { get; set; }

        internal Point GetPopupPosition(PopupMenuWindow wndPopup) => throw new NotImplementedException();

        public void InputAction(ConsoleKeyInfo keyInfo) => throw new NotImplementedException();
    }
}
