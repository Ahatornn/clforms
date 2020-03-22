using ClForms.Abstractions.Engine;
using ClForms.Elements.Menu;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Represents an individual item that is displayed within a <see cref="MainMenu"/>
    /// </summary>
    public abstract class MenuItemBase
    {
        /// <summary>
        /// Gets or sets a value indicating the caption of the menu item
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets a value indicating the menu that contains this menu item
        /// </summary>
        public MenuItemBase Parent { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the menu item is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the menu item is visible
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a value of the item's identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user-defined data associated with the menu item
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Returns the length required to display a menu item
        /// </summary>
        public abstract int MenuItemLength { get; }

        /// <summary>
        /// Filling a pseudographics drawing context
        /// </summary>
        public abstract void OnRender(IDrawingContext context,
            Themes.Color backColor,
            Themes.Color foreColor,
            MainMenu mainMenu,
            int itemAreaWidth);

        internal void SetParent(MenuItemBase parent) => Parent = parent;
    }
}
