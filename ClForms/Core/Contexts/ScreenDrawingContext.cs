using System;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Themes;

namespace ClForms.Core.Contexts
{
    internal sealed class ScreenDrawingContext
    {
        private readonly ArrayDevice<Color> background;
        private readonly ArrayDevice<Color> foreground;
        private readonly ArrayDevice<char> chars;
        private Point cursorPosition;
        private Rect renderArea;

        internal ScreenDrawingContext(Rect rect)
        {
            ContextBounds = rect;
            renderArea = new Rect(rect.Size);
            background = new ColorDevice(rect.Size);
            foreground = new ColorDevice(rect.Size);
            chars = new CharDevice(rect.Size);
            cursorPosition = Point.Empty;
        }

        internal IGraphicsDevice<Color> Background => background;

        internal IGraphicsDevice<Color> Foreground => foreground;

        internal IGraphicsDevice<char> Chars => chars;

        internal Rect ContextBounds { get; }

        internal void Release(Color backgroundColor, Color foregroundColor)
        {
            background.Release(backgroundColor);
            foreground.Release(foregroundColor);
            chars.Release('\0');
        }

        internal void SetCursorPos(Point point)
        {
            if (!renderArea.Contains(point))
            {
                throw new ArgumentException($"The point [{point}] does not contains in work area {renderArea}");
            }

            cursorPosition = point;
        }

        internal ContextColorPoint GetColorPoint(int col, int row)
            => new ContextColorPoint(background[col, row], foreground[col, row]);

        internal void DrawText(char @char, Color backgroundColor, Color foregroundColor)
            => DrawText(@char.ToString(), backgroundColor, foregroundColor);

        internal void DrawText(string text, Color backgroundColor, Color foregroundColor)
        {
            var right = Math.Min(cursorPosition.X + text.Length, renderArea.Width);
            for (var i = cursorPosition.X; i < right; i++)
            {
                background[i, cursorPosition.Y] = backgroundColor;
                foreground[i, cursorPosition.Y] = foregroundColor;
                chars[i, cursorPosition.Y] = text[i - cursorPosition.X];
            }
            cursorPosition = new Point(right, cursorPosition.Y);
        }
    }
}
