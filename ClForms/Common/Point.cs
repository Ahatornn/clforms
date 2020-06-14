using System;
using ClForms.Helpers;

namespace ClForms.Common
{
    /// <summary>
    /// Describes the location of an element
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Zero-sized <see cref="Point"/>
        /// </summary>
        public static Point Empty => new Point(0, 0);

        /// <summary>
        /// Initialize a new instance <see cref="Point"/>
        /// </summary>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Compare two <see cref="Point" /> for equality
        /// </summary>
        public static bool operator ==(Point point1, Point point2)
        {
            if (point1.X == point2.X)
            {
                return point1.Y == point2.Y;
            }
            return false;
        }

        /// <summary>
        /// Compare two <see cref="Rect" /> for inequality
        /// </summary>
        public static bool operator !=(Point point1, Point point2)
            => !(point1 == point2);

        /// <summary>
        /// Performs addition two <see cref="Point" /> objects
        /// </summary>
        public static Point operator +(Point point1, Point point2)
            => new Point(point1.X + point2.X, point1.Y + point2.Y);

        /// <summary>
        /// Compare two <see cref="Point" /> for equality
        /// </summary>
        public static bool Equals(Point point1, Point point2)
        {
            if (point1.X.Equals(point2.X))
            {
                return point1.Y.Equals(point2.Y);
            }
            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object o)
        {
            if (o == null || !(o is Point))
            {
                return false;
            }
            return Point.Equals(this, (Point) o);
        }

        /// <summary>
        /// Compares the value of the <see cref="Point" /> instance to equality
        /// </summary>
        public bool Equals(Point value) => Point.Equals(this, value);

        /// <inheritdoc />
        public override int GetHashCode() => GetHashCodeHelper.CalculateHashCode(X, Y);

        /// <inheritdoc />
        public override string ToString() => $"{{{X}:{Y}}}";

        /// <summary>
        /// Point offset <see cref="Point.X" /> and <see cref="Point.Y" /> by specified values
        /// </summary>
        public void Offset(int offsetX, int offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        /// <summary>
        /// Explicitly converts an instance of <see cref="Point" /> to an instance of <see cref="Size" />.
        /// </summary>
        public static explicit operator Size(Point point)
            => new Size(Math.Abs(point.X), Math.Abs(point.Y));

        /// <summary>
        ///  Gets the x-axis value
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets the y-axis value
        /// </summary>
        public int Y { get; set; }
    }
}
