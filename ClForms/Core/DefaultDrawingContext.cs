using System;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Core
{
    /// <summary>
    /// Default <see cref="IDrawingContext"/> resolver
    /// </summary>
    internal class DefaultDrawingContext: IDrawingContext, IPaintContext
    {
        /// <summary>
        /// Empty drawing context
        /// </summary>
        public static DefaultDrawingContext Empty => new DefaultDrawingContext(Rect.Empty, 0, 23, Guid.Empty, null);

        private readonly ArrayDevice<Color> background;
        private readonly ArrayDevice<Color> foreground;
        private readonly ArrayDevice<char> chars;
        private Point cursorPosition;
        private Rect renderArea;
        private Color currentBackground;
        private Color currentForeground;
        private readonly int childrenIdHash;
        private readonly Lazy<int> contextHashCode;

        /// <summary>
        /// Initialize a new instance <see cref="DefaultDrawingContext"/>
        /// </summary>
        public DefaultDrawingContext(Rect rect,
            long controlId,
            int childrenIdHash,
            Guid renderSessionId,
            IDrawingContext parent)
        {
            ContextBounds = rect;
            renderArea = new Rect(rect.Size);
            background = new ArrayDevice<Color>(rect.Size);
            foreground = new ArrayDevice<Color>(rect.Size);
            chars = new ArrayDevice<char>(rect.Size);
            cursorPosition = Point.Empty;
            ControlId = controlId;
            RenderSessionId = renderSessionId;
            Parent = parent;
            this.childrenIdHash = childrenIdHash;
            contextHashCode = new Lazy<int>(GetHashCodeInternal);
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

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() => contextHashCode.Value;

        /// <inheritdoc cref="IPaintContext.GetColorPoint"/>
        public ContextColorPoint GetColorPoint(int col, int row)
            => new ContextColorPoint(background[col, row], foreground[col, row]);

        /// <inheritdoc cref="IDrawingContext.RenderSessionId"/>
        public Guid RenderSessionId { get; }

        /// <inheritdoc cref="IDrawingContext.Parent"/>
        public IDrawingContext Parent { get; }

        /// <summary>
        /// Create a copy <see cref="DefaultDrawingContext"/> from current context
        /// </summary>
        public DefaultDrawingContext Clone(long controlId, Guid renderSessionId)
        {
            var result = new DefaultDrawingContext(ContextBounds, controlId, childrenIdHash, renderSessionId, this);
            result.background.Release(background);
            result.foreground.Release(foreground);
            result.chars.Release(chars);

            return result;
        }

        private int GetHashCodeInternal()
            => GetHashCodeHelper.CalculateHashCode(Parent?.ControlId.GetHashCode() ?? GetHashCodeHelper.PrimeNumber,
                childrenIdHash,
                background.GetHashCode(),
                foreground.GetHashCode(),
                chars.GetHashCode(),
                ContextBounds.GetHashCode(),
                ControlId.GetHashCode());
    }
}
