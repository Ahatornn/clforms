using ClForms.Common;
using ClForms.Core.Contexts;
using ClForms.Themes;

namespace ClForms.Helpers
{
    internal static class DrawingContextHelper
    {
        internal static ScreenDrawingContext Merge(this ScreenDrawingContext parentContext,
            Point location,
            ScreenDrawingContext childContext)
        {
            var childRect = new Rect(location.X,
                location.Y,
                childContext.ContextBounds.Width,
                childContext.ContextBounds.Height);
            var result = new ScreenDrawingContext(parentContext.ContextBounds);
            for (var row = 0; row < parentContext.ContextBounds.Height; row++)
            {
                for (var col = 0; col < parentContext.ContextBounds.Width; col++)
                {
                    if (col >= childRect.X && col < childRect.Width + location.X &&
                        row >= childRect.Y && row < childRect.Height + location.Y)
                    {
                        result.SetValues(col,
                            row,
                            childContext.Background[col - location.X, row - location.Y] != Color.NotSet
                                ? childContext.Background[col - location.X, row - location.Y]
                                : parentContext.Background[col, row],
                            childContext.Foreground[col - location.X, row - location.Y] != Color.NotSet
                                ? childContext.Foreground[col - location.X, row - location.Y]
                                : parentContext.Foreground[col, row],
                            childContext.Chars[col - location.X, row - location.Y] != '\0'
                                ? childContext.Chars[col - location.X, row - location.Y]
                                : ' ');
                    }
                    else
                    {
                        result.SetValues(col,
                            row,
                            parentContext.Background[col, row],
                            parentContext.Foreground[col, row],
                            parentContext.Chars[col, row]);
                    }
                }
            }

            return result;
        }
    }
}
