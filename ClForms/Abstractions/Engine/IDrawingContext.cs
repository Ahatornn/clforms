using System;
using ClForms.Common;
using ClForms.Themes;

namespace ClForms.Abstractions.Engine
{
    /// <summary>
    /// Drawing context
    /// </summary>
    public interface IDrawingContext
    {
        /// <summary>
        /// Gets <see cref="IGraphicsDevice{Color}"/> of background
        /// </summary>
        IGraphicsDevice<Color> Background { get; }

        /// <summary>
        /// Gets <see cref="IGraphicsDevice{Color}"/> of foreground
        /// </summary>
        IGraphicsDevice<Color> Foreground { get; }

        /// <summary>
        /// Gets <see cref="IGraphicsDevice{Color}"/> of text
        /// </summary>
        IGraphicsDevice<char> Chars { get; }

        /// <summary>
        /// Rectangular drawing context area
        /// </summary>
        Rect ContextBounds { get; }

        /// <summary>
        /// The identifier of the component that represents current drawing context
        /// </summary>
        long ControlId { get; }

        /// <summary>
        /// Gets <see cref="ContextColorPoint"/> in point
        /// </summary>
        ContextColorPoint GetColorPoint(int col, int row);

        /// <summary>
        /// Item rendering session id
        /// <remarks> Allows to determine session when the element was drawn</remarks>
        /// </summary>
        Guid RenderSessionId { get; }

        /// <summary>
        /// Parent context reference
        /// </summary>
        IDrawingContext Parent { get; }
    }
}
