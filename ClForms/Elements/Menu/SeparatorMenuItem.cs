using ClForms.Abstractions.Engine;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements.Menu
{
    /// <summary>
    /// Control that is used to separate items in menu controls
    /// </summary>
    public class SeparatorMenuItem: MenuItemBase
    {
        /// <summary>
        /// Initialize a new instance <see cref="SeparatorMenuItem"/>
        /// </summary>
        public SeparatorMenuItem()
        {
            Enabled = false;
            Visible = true;
        }

        /// <inheritdoc />
        public override int MenuItemLength => 1;

        /// <inheritdoc />
        public override void OnRender(IDrawingContext context,
            Color backColor,
            Color foreColor,
            MainMenu mainMenu,
            int itemAreaWidth) => context.DrawText(
            new string(Application.Environment.MenuSeparatorBorder, itemAreaWidth),
            backColor,
            mainMenu.Foreground);
    }
}
