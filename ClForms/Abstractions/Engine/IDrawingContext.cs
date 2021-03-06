using System;
using ClForms.Common;
using ClForms.Themes;

namespace ClForms.Abstractions.Engine
{
    /// <summary>
    /// Drawing context
    /// </summary>
    public interface IDrawingContext: IPaintContext
    {
        /// <summary>
        /// Gets <see cref="IGraphicsDevice{Color}"/> of background
        /// </summary>
        IGraphicsDevice<Color> Background { get; }

        /// <summary>
        /// Gets <see cref="IGraphicsDevice{Color}"/> of text color
        /// </summary>
        IGraphicsDevice<Color> Foreground { get; }

        /// <summary>
        /// Gets <see cref="IGraphicsDevice{Color}"/> of text
        /// </summary>
        IGraphicsDevice<char> Chars { get; }

        /// <summary>
        /// Gets the rectangular drawing context area
        /// </summary>
        Rect ContextBounds { get; }

        /// <summary>
        /// Gets the identifier of the component that represents current drawing context
        /// </summary>
        Guid ControlId { get; }

        /// <summary>
        /// Gets parent context reference
        /// </summary>
        IDrawingContext Parent { get; }

        /// <summary>
        /// Prepare context for drawing
        /// </summary>
        void Release(Color backgroundColor, Color foregroundColor, char @char);

        /// <summary>
        /// Prepare context for drawing
        /// </summary>
        void Release(Color backgroundColor, Color foregroundColor);

        /// <summary>
        /// Creates a shallow copy of the <see cref="IDrawingContext"/>
        /// </summary>
        IDrawingContext Clone(Guid controlId);
    }
}
