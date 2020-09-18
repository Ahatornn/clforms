using System;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Themes;

namespace ClForms.Core.Contexts
{
    internal sealed class ScreenDrawingContext: IDrawingContextDescriptor, IPaintContext
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

        /// <see cref="IDrawingContextDescriptor.Background"/>
        public IGraphicsDevice<Color> Background => background;

        /// <see cref="IDrawingContextDescriptor.Foreground"/>
        public IGraphicsDevice<Color> Foreground => foreground;

        /// <see cref="IDrawingContextDescriptor.Chars"/>
        public IGraphicsDevice<char> Chars => chars;

        /// <see cref="IDrawingContextDescriptor.ContextBounds"/>
        public Rect ContextBounds { get; }

        internal void Release(Color backgroundColor, Color foregroundColor)
        {
            background.Release(backgroundColor);
            foreground.Release(foregroundColor);
            chars.Release('\0');
        }

        /// <see cref="IPaintContext.SetCursorPos(Point)"/>
        public void SetCursorPos(Point point)
        {
            if (!renderArea.Contains(point))
            {
                throw new ArgumentException($"The point [{point}] does not contains in work area {renderArea}");
            }
            cursorPosition = point;
        }

        /// <see cref="IPaintContext.SetCursorPos(int, int)"/>
        public void SetCursorPos(int x, int y) => SetCursorPos(new Point(x, y));

        /// <see cref="IPaintContext.GetColorPoint(int, int)"/>
        public ContextColorPoint GetColorPoint(int col, int row)
            => new ContextColorPoint(background[col, row], foreground[col, row]);

        /// <see cref="IPaintContext.DrawText(char, Color, Color)"/>
        public void DrawText(char @char, Color backgroundColor, Color foregroundColor)
            => DrawText(@char.ToString(), backgroundColor, foregroundColor);

        /// <see cref="IPaintContext.DrawText(string, Color, Color)"/>
        public void DrawText(string text, Color backgroundColor, Color foregroundColor)
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

        /// <see cref="IPaintContext.DrawText(char, Color)"/>
        public void DrawText(char @char, Color foregroundColor)
            => DrawText(@char.ToString(), foregroundColor);

        /// <see cref="IPaintContext.DrawText(string, Color)"/>
        public void DrawText(string text, Color foregroundColor)
        {
            var right = Math.Min(cursorPosition.X + text.Length, renderArea.Width);
            for (var i = cursorPosition.X; i < right; i++)
            {
                foreground[i, cursorPosition.Y] = foregroundColor;
                chars[i, cursorPosition.Y] = text[i - cursorPosition.X];
            }
            cursorPosition = new Point(right, cursorPosition.Y);
        }

        /// <see cref="IPaintContext.DrawText(string)"/>
        public void DrawText(string text)
        {
            var right = Math.Min(cursorPosition.X + text.Length, renderArea.Width);
            for (var i = cursorPosition.X; i < right; i++)
            {
                chars[i, cursorPosition.Y] = text[i - cursorPosition.X];
            }
            cursorPosition = new Point(right, cursorPosition.Y);
        }

        /// <see cref="IPaintContext.DrawText(char)"/>
        public void DrawText(char @char) => DrawText(@char.ToString());

        /// <see cref="IDrawingContextDescriptor.SetValues(int, int, Color, Color, char, bool)"/>
        public void SetValues(int col, int row, Color backColor, Color foreColor, char @char, bool ignoreEmpty)
        {
            background[col, row] = ignoreEmpty && backColor == Color.NotSet ? background[col, row] : backColor;
            foreground[col, row] = ignoreEmpty && foreColor == Color.NotSet ? foreground[col, row] : foreColor;
            chars[col, row] = ignoreEmpty && @char == '\0' ? chars[col, row] : @char;
        }

        internal void SetBackgroundValues(Rect rect, Color backColor) => SetValuesInternal(background, rect, backColor);

        internal void SetForegroundValues(Rect rect, Color foreColor) => SetValuesInternal(foreground, rect, foreColor);

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
