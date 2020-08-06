using ClForms.Abstractions.Engine;
using ClForms.Elements;

namespace ClForms.Common
{
    internal class ListViewGroupBase<T>: GroupBase
    {
        private readonly ListView<T> ownerListView;

        internal ListViewGroupBase(ListView<T> targetControl)
            : base(targetControl)
        {
            ownerListView = targetControl;
        }

        /// <inheritdoc cref="GroupBase.DrawHeaderText"/>
        internal override void DrawHeaderText(IDrawingContext context, string presenterText, int x, int y)
        {
            var foreColor = ownerListView.IsDisabled
                ? ownerListView.DisabledForeground
                : ownerListView.IsFocus
                    ? ownerListView.FocusForeground
                    : ownerListView.Foreground;

            var backColor = ownerListView.IsDisabled
                ? ownerListView.DisabledBackground
                : ownerListView.IsFocus
                    ? ownerListView.FocusBackground
                    : ownerListView.Background;

            context.SetCursorPos(x, y);
            context.DrawText(presenterText, backColor, foreColor);
        }
    }
}
