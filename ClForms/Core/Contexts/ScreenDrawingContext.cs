using System;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Themes;

namespace ClForms.Core.Contexts
{
    internal sealed class ScreenDrawingContext: IDrawingContextDescriptor
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

        public IGraphicsDevice<Color> Background => background;

        public IGraphicsDevice<Color> Foreground => foreground;

        public IGraphicsDevice<char> Chars => chars;

        public Rect ContextBounds { get; }

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

        internal void SetCursorPos(int x, int y) => SetCursorPos(new Point(x, y));

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

        internal void DrawText(char @char, Color foregroundColor)
            => DrawText(@char.ToString(), foregroundColor);

        internal void DrawText(string text, Color foregroundColor)
        {
            var right = Math.Min(cursorPosition.X + text.Length, renderArea.Width);
            for (var i = cursorPosition.X; i < right; i++)
            {
                foreground[i, cursorPosition.Y] = foregroundColor;
                chars[i, cursorPosition.Y] = text[i - cursorPosition.X];
            }
            cursorPosition = new Point(right, cursorPosition.Y);
        }

        public void SetValues(int col, int row, Color backColor, Color foreColor, char @char, bool ignoreEmpty)
        {
            background[col, row] = ignoreEmpty && backColor == Color.NotSet ? background[col, row] : backColor;
            foreground[col, row] = ignoreEmpty && foreColor == Color.NotSet ? foreground[col, row] : foreColor;
            chars[col, row] = ignoreEmpty && @char == '\0' ? chars[col, row] : @char;
        }

        public void SetBackgroundValues(Rect rect, Color backColor) => SetValuesInternal(background, rect, backColor);

        public void SetForegroundValues(Rect rect, Color foreColor) => SetValuesInternal(foreground, rect, foreColor);

        private void SetValuesInternal(ArrayDevice<Color> device, Rect rect, Color value)
        {
            for (var row = rect.Top; row < rect.Bottom; row++)
            {
                for (var col = rect.Left; col < rect.Right; col++)
                {
                    device[col, row] = value;
                }
            }
        }
    }
}
