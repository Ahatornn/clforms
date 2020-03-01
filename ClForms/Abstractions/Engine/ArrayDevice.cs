using System.Collections.Generic;
using ClForms.Common;
using ClForms.Helpers;

namespace ClForms.Abstractions.Engine
{
    /// <summary>
    /// Array context
    /// </summary>
    public class ArrayDevice<T>: IGraphicsDevice<T>
    {
        private readonly T[,] device;

        /// <summary>
        /// Initialize a new instance <see cref="ArrayDevice{T}"/>
        /// </summary>
        /// <param name="size">Dimension of array</param>
        public ArrayDevice(Size size)
        {
            device = new T[size.Height, size.Width];
        }

        /// <inheritdoc cref="IGraphicsDevice{T}"/>
        public T this[Point point]
        {
            get => device[point.Y, point.X];
            set => device[point.Y, point.X] = value;
        }

        /// <inheritdoc cref="IGraphicsDevice{T}"/>
        public T this[int colIndex, int rowIndex]
        {
            get => device[rowIndex, colIndex];
            set => device[rowIndex, colIndex] = value;
        }

        /// <summary>
        /// Fills the drawing context with value
        /// </summary>
        public void Release(T value)
        {
            for (var row = 0; row < device.GetLength(ArrayDimension.Row); row++)
            {
                for (var col = 0; col < device.GetLength(ArrayDimension.Column); col++)
                {
                    device[row, col] = value;
                }
            }
        }

        /// <summary>
        /// Fills a drawing context with values of an other context
        /// </summary>
        public void Release(ArrayDevice<T> source)
        {
            for (var row = 0; row < device.GetLength(ArrayDimension.Row); row++)
            {
                for (var col = 0; col < device.GetLength(ArrayDimension.Column); col++)
                {
                    device[row, col] = source[col, row];
                }
            }
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            var items = new List<int>(device.GetLength(ArrayDimension.Row) * device.GetLength(ArrayDimension.Column));
            for (var row = 0; row < device.GetLength(ArrayDimension.Row); row++)
            {
                for (var col = 0; col < device.GetLength(ArrayDimension.Column); col++)
                {
                    items.Add(device[row, col].GetHashCode());
                }
            }

            return GetHashCodeHelper.CalculateHashCode(items.ToArray());
        }
    }
}
