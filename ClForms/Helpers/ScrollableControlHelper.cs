using System;
using System.Collections;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core;
using ClForms.Elements.Abstractions;

namespace ClForms.Helpers
{
    /// <summary>
    /// Helper for scrollable control
    /// </summary>
    internal static class ScrollableControlHelper
    {
        internal static int GetScrollOffset(int height, ICollection items)
            => height < items.Count ? 1 : 0;

        internal static void OnRender(Control targetControl, IDrawingContext context, Rect reducedArea, ICollection items, int startRenderedItemIndex)
        {
            var shouldScrollBar = reducedArea.Height < items.Count;
            var scrollArea = reducedArea.Height - 2;
            if (shouldScrollBar && scrollArea > 0)
            {
                var scrollHeight = Math.Max((scrollArea * (scrollArea * 100 / items.Count)) / 100, 1);
                var scrollTopOffset = ((startRenderedItemIndex * scrollArea) / items.Count) + 1;
                if (startRenderedItemIndex > 0 && scrollTopOffset == 1)
                {
                    scrollTopOffset = 2;
                }
                if (startRenderedItemIndex + reducedArea.Height >= items.Count)
                {
                     scrollTopOffset = reducedArea.Height - 1 - scrollHeight;
                }
                for (var row = 0; row < reducedArea.Height; row++)
                {
                    context.SetCursorPos(targetControl.Padding.Left + reducedArea.Width - 1, targetControl.Padding.Top + row);
                    if (row == 0 || row == reducedArea.Height - 1)
                    {
                        context.DrawText(row == 0
                                ? '▴'
                                : '▾',
                            Application.SystemColors.ScrollBackground,
                            Application.SystemColors.ScrollForeground);
                    }
                    else
                    {
                        if (row >= scrollTopOffset && row <= scrollTopOffset + scrollHeight)
                        {
                            context.DrawText('▓',
                                targetControl.Background,
                                Application.SystemColors.ScrollBackground);
                        }
                        else
                        {
                            context.DrawText('╽',
                                targetControl.Background,
                                Application.SystemColors.ScrollBackground);
                        }
                    }
                }
            }
        }
    }
}
