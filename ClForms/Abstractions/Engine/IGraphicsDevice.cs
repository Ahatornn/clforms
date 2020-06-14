using ClForms.Common;

namespace ClForms.Abstractions.Engine
{
    /// <summary>
    /// Defines the value of <see langword="T" /> at a drawing context point
    /// </summary>
    public interface IGraphicsDevice<out T>
    {
        /// <summary>
        /// Defines a value at a point based on the coordinates
        /// </summary>
        T this[int colIndex, int rowIndex]
        {
            get;
        }

        /// <summary>
        /// Defines a value at a point based on the <see cref="Point"/>
        /// </summary>
        T this[Point point]
        {
            get;
        }
    }
}
