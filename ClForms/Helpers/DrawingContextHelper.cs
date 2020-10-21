using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core.Contexts;

namespace ClForms.Helpers
{
    internal static class DrawingContextHelper
    {
        internal static ScreenDrawingContext Merge(this ScreenDrawingContext parentContext,
            Point location,
            ScreenDrawingContext childContext)
        {
            var result = new ScreenDrawingContext(parentContext.ContextBounds);
            result.MergeWith(Point.Empty, parentContext);
            result.MergeWith(location, childContext);
            return result;
        }

        internal static void MergeWith(this IDrawingContextDescriptor parentContext,
            Point location,
            IDrawingContextDescriptor childContext,
            bool ignoreEmpty = false)
        {
            var childRect = new Rect(location.X,
                location.Y,
                childContext.ContextBounds.Width,
                childContext.ContextBounds.Height);
            for (var row = 0; row < parentContext.ContextBounds.Height; row++)
            {
                for (var col = 0; col < parentContext.ContextBounds.Width; col++)
                {
                    if (col >= childRect.X && col < childRect.Width + location.X &&
                        row >= childRect.Y && row < childRect.Height + location.Y)
                    {
                        parentContext.SetValues(col,
                            row,
                            childContext.Background[col - location.X, row - location.Y],
                            childContext.Foreground[col - location.X, row - location.Y],
                            childContext.Chars[col - location.X, row - location.Y],
                            ignoreEmpty);
                    }
                }
            }
        }
    }
}
