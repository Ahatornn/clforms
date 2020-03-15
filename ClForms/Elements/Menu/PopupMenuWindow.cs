using ClForms.Abstractions;
using ClForms.Common;
using System;

namespace ClForms.Elements.Menu
{
    internal class PopupMenuWindow : Window, IElementStyle<PopupMenuWindow>
    {
        internal Point PreferredLocation => throw new NotImplementedException();

        public void SetStyle(Action<PopupMenuWindow> styleAction) => throw new NotImplementedException();
    }
}
