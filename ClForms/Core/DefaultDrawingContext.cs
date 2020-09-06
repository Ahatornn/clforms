using System;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core.Contexts;
using ClForms.Themes;

namespace ClForms.Core
{
    /// <summary>
    /// Default <see cref="IDrawingContext"/> resolver
    /// </summary>
    internal class DefaultDrawingContext: IDrawingContext, IDrawingContextDescriptor
    {
        /// <summary>
        /// Empty drawing context
        /// </summary>
        public static DefaultDrawingContext Empty => new DefaultDrawingContext(Rect.Empty, default, null);

        private readonly ArrayDevice<Color> background;
        private readonly ArrayDevice<Color> foreground;
        private readonly ArrayDevice<char> chars;
        private Point cursorPosition;
        private Rect renderArea;
        private Color currentBackground;
        private Color currentForeground;

        /// <summary>
        /// Initialize a new instance <see cref="DefaultDrawingContext"/>
        /// </summary>
        public DefaultDrawingContext(Rect rect,
            long controlId,
            IDrawingContext parent)
        {
            ContextBounds = rect;
            renderArea = new Rect(rect.Size);
            background = new ColorDevice(rect.Size);
            foreground = new ColorDevice(rect.Size);
            chars = new CharDevice(rect.Size);
            cursorPosition = Point.Empty;
            ControlId = controlId;
            Parent = parent;
        }

        /// <inheritdoc cref="IDrawingContext.Background"/>
        public IGraphicsDevice<Color> Background => background;

        /// <inheritdoc cref="IDrawingContext.Foreground"/>
        public IGraphicsDevice<Color> Foreground => foreground;

        /// <inheritdoc cref="IDrawingContext.Chars"/>
        public IGraphicsDevice<char> Chars => chars;

        /// <inheritdoc cref="IDrawingContext.ContextBounds"/>
        public Rect ContextBounds { get; }

        /// <inheritdoc cref="IDrawingContext.ControlId"/>
        public long ControlId { get; }

        /// <inheritdoc cref="IDrawingContext.Release(Color,Color,char)"/>
        public void Release(Color backgroundColor, Color foregroundColor, char @char)
        {
            Release(backgroundColor, foregroundColor);
            chars.Release(@char);
        }

        /// <inheritdoc cref="IDrawingContext.Release(Color,Color)"/>
        public void Release(Color backgroundColor, Color foregroundColor)
        {
            currentBackground = backgroundColor;
            currentForeground = foregroundColor;
            background.Release(backgroundColor);
            foreground.Release(foregroundColor);
            chars.Release('\0');
        }

        /// <inheritdoc cref="IPaintContext.SetCursorPos(int,int)"/>
        public void SetCursorPos(int x, int y) => SetCursorPos(new Point(x, y));

        /// <inheritdoc cref="IPaintContext.SetCursorPos(Point)"/>
        public void SetCursorPos(Point point)
        {
            if (!renderArea.Contains(point))
            {
                throw new ArgumentException($"The point [{point}] does not contains in work area {renderArea}");
            }

            cursorPosition = point;
        }

        /// <inheritdoc cref="IPaintContext.DrawText(string)"/>
        public void DrawText(string text) => DrawText(text, currentForeground);

        /// <inheritdoc cref="IPaintContext.DrawText(char)"/>
        public void DrawText(char @char) => DrawText(@char.ToString(), currentForeground);

        /// <inheritdoc cref="IPaintContext.DrawText(string, Color)"/>
        public void DrawText(string text, Color foregroundColor)
            => DrawText(text, currentBackground, foregroundColor);

        /// <inheritdoc cref="IPaintContext.DrawText(char, Color)"/>
        public void DrawText(char @char, Color foregroundColor)
            => DrawText(@char.ToString(), currentBackground, foregroundColor);

        /// <inheritdoc cref="IPaintContext.DrawText(char, Color, Color)"/>
        public void DrawText(char @char, Color backgroundColor, Color foregroundColor)
            => DrawText(@char.ToString(), backgroundColor, foregroundColor);

        /// <inheritdoc cref="IPaintContext.DrawText(string, Color, Color)"/>
        public void DrawText(string text, Color backgroundColor, Color foregroundColor)
        {
            var right = Math.Min(cursorPosition.X + text.Length, renderArea.Width);
            for (var i = cursorPosition.X; i < right; i++)
            {
                background[i, cursorPosition.Y] = backgroundColor;
                foreground[i, cursorPosition.Y] = foregroundColor;
                chars[i, cursorPosition.Y] = text[i - cursorPosition.X];
            }

            SetCursorPos(right - (right >= renderArea.Width ? 1 : 0), cursorPosition.Y);
        }

        /// <inheritdoc cref="IPaintContext.GetColorPoint"/>
        public ContextColorPoint GetColorPoint(int col, int row)
            => new ContextColorPoint(background[col, row], foreground[col, row]);

        /// <inheritdoc cref="IDrawingContext.Parent"/>
        public IDrawingContext Parent { get; }

        /// <summary>
        /// Create a copy <see cref="DefaultDrawingContext"/> from current context
        /// </summary>
        public IDrawingContext Clone(long controlId)
        {
            var result = new DefaultDrawingContext(ContextBounds, controlId, this);
            result.background.Release(background);
            result.foreground.Release(foreground);
            result.chars.Release(chars);

            return result;
        }

        public void SetValues(int col, int row, Color backColor, Color foreColor, char @char, bool ignoreEmpty)
        {
            background[col, row] = ignoreEmpty && backColor == Color.NotSet ? background[col, row] : backColor;
            foreground[col, row] = ignoreEmpty && foreColor == Color.NotSet ? foreground[col, row] : foreColor;
            chars[col, row] = ignoreEmpty && @char == '\0' ? chars[col, row] : @char;
        }
    }
}
