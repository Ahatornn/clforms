using ClForms.Common;
using System;

namespace ClForms.Core.Models
{
    /// <summary>
    /// The parameters of the rendering component
    /// </summary>
    internal class InvalidateParameters
    {
        /// <summary>
        /// Hash of the component context
        /// </summary>
        public int HashValue { get; }

        /// <summary>
        /// Hash of the parent component context
        /// </summary>
        public int ParentHashValue { get; }

        /// <summary>
        /// ID of the rendering session
        /// </summary>
        public Guid RenderId { get; }

        /// <summary>
        /// Rectangular area for drawing a component
        /// </summary>
        public Rect InvalidateRect { get; }

        /// <summary>
        /// Position of the element relative to the parent
        /// </summary>
        public Point Location { get; }

        /// <summary>
        /// Initialize a new instance <see cref="InvalidateParameters"/>
        /// </summary>
        public InvalidateParameters(int hashValue,
            int parentHashValue,
            Guid renderId,
            Rect invalidateRect,
            Point location)
        {
            HashValue = hashValue;
            ParentHashValue = parentHashValue;
            RenderId = renderId;
            InvalidateRect = invalidateRect;
            Location = location;
        }
    }
}
