using ClForms.Abstractions.Engine;
using ClForms.Core;

namespace ClForms.Elements.Menu
{
    internal class PopupMenuContentBox: GroupBox
    {
        private readonly MenuItemCollection items;

        internal PopupMenuContentBox(MenuItemCollection items)
        {
            this.items = items;
        }

        /// <inheritdoc />
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i] is SeparatorMenuItem)
                {
                    context.SetCursorPos(BorderThickness.Left - 1, BorderThickness.Top + i);
                    context.DrawText(Application.Environment.MenuSeparatorLeftBorder, BorderColor);

                    context.SetCursorPos(context.ContextBounds.Width - BorderThickness.Right, BorderThickness.Top + i);
                    context.DrawText(Application.Environment.MenuSeparatorRightBorder, BorderColor);
                }
            }
        }
    }
}
