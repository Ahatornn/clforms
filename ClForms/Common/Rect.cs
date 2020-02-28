using System;
using ClForms.Helpers;

namespace ClForms.Common
{
    /// <summary>
    /// Describes the width, height, and location of a rectangular area
    /// </summary>
    public struct Rect
    {
        /// <summary>
        /// Zero-sized <see cref="Rect"/>
        /// </summary>
        public static Rect Empty => new Rect(0, 0, 0, 0);

        private int width;
        private int height;

        /// <summary>
        /// Initialize a new instance <see cref="Rect"/>
        /// </summary>
        public Rect(int x, int y, int width, int height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Width and height cannot be negative");
            }
            X = x;
            Y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Rect" /> structure, which is large enough to hold two given points.
        /// </summary>
        public Rect(Point point1, Point point2)
        {
            X = Math.Min(point1.X, point2.X);
            Y = Math.Min(point1.Y, point2.Y);
            width = Math.Max(Math.Max(point1.X, point2.X) - X, 0);
            height = Math.Max(Math.Max(point1.Y, point2.Y) - Y, 0);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Rect" /> A structure that has the specified size and coordinates (0,0).
        /// </summary>
        public Rect(Size size)
        {
            X = Y = 0;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Compare two <see cref="Rect" /> for equality
        /// </summary>
        public static bool operator ==(Rect rect1, Rect rect2)
        {
            if (rect1.X == rect2.X && rect1.Y == rect2.Y && rect1.Width == rect2.Width)
            {
                return rect1.Height == rect2.Height;
            }
            return false;
        }

        /// <summary>
        /// Compare two <see cref="Rect" /> for inequality
        /// </summary>
        public static bool operator !=(Rect rect1, Rect rect2) => !(rect1 == rect2);

        /// <summary>
        /// Compare two <see cref="Size" /> for equality
        /// </summary>
        public static bool Equals(Rect rect1, Rect rect2)
        {
            if (rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y) && rect1.Width.Equals(rect2.Width))
            {
                return rect1.Height.Equals(rect2.Height);
            }
            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object o)
        {
            if (o == null || !(o is Rect))
            {
                return false;
            }
            return Rect.Equals(this, (Rect) o);
        }

        /// <summary>
        /// Compares the value of the <see cref="Rect" /> instance to equality
        /// </summary>
        public bool Equals(Rect value) => Rect.Equals(this, value);

        /// <inheritdoc />
        public override int GetHashCode()
            => GetHashCodeHelper.CalculateHashCode(X, Y, Width, Height);

        /// <inheritdoc />
        public override string ToString() => $"{{{X}:{Y}:{width}:{height}}}";

        /// <summary>
        /// Gets or sets the position of the upper left corner of the rectangle
        /// </summary>
        public Point Location
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the width and height of the rectangle
        /// </summary>
        public Size Size
        {
            get => new Size(this.width, this.height);
            set
            {
                width = value.Width;
                height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-axis value of the left side of the rectangle
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-axis value of the top side of the rectangle
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangle
        /// </summary>
        public int Width
        {
            get => width;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Width cannot be negative");
                }

                width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle
        /// </summary>
        public int Height
        {
            get => height;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Height cannot be negative");
                }

                height = value;
            }
        }

        /// <summary>
        /// Gets the x-axis value of the left side of the rectangle
        /// </summary>
        public int Left => X;

        /// <summary>
        /// Gets the y-axis value of the top side of the rectangle
        /// </summary>
        public int Top => Y;

        /// <summary>
        /// Gets the x-axis value of the right side of the rectangle
        /// </summary>
        public int Right => X + width;

        /// <summary>
        /// Gets the y-axis value of the bottom side of the rectangle
        /// </summary>
        public int Bottom => Y + height;

        /// <summary>
        /// Gets the position of the upper left corner of the rectangle
        /// </summary>
        public Point TopLeft => new Point(Left, Top);

        /// <summary>
        /// Gets the position of the upper right corner of the rectangle
        /// </summary>
        public Point TopRight => new Point(Right, Top);

        /// <summary>
        /// Gets the position of the bottom left corner of the rectangle
        /// </summary>
        public Point BottomLeft => new Point(Left, Bottom);

        /// <summary>
        /// Gets the position of the bottom right corner of the rectangle
        /// </summary>
        public Point BottomRight => new Point(Right, Bottom);

        /// <summary>
        /// Indicates whether the rectangle includes the specified point
        /// </summary>
        public bool Contains(Point point) => Contains(point.X, point.Y);

        /// <summary>
        /// Indicates whether the rectangle includes the specified point
        /// </summary>
        public bool Contains(int x, int y) => this.ContainsInternal(x, y);

        /// <summary>
        /// Indicates whether the rectangle includes the specified rectangle
        /// </summary>
        public bool Contains(Rect rect)
        {
            if (X > rect.X || Y > rect.Y || X + width < rect.X + rect.width)
            {
                return false;
            }
            return Y + height >= rect.Y + rect.height;
        }

        /// <summary>
        /// Returns <see cref="Rect" /> with the area reduced by <see cref="Thickness" />
        /// </summary>
        public Rect Reduce(Thickness indent)
        {
            var left = Left + indent.Left;
            var top = Top + indent.Top;
            var right = Right - indent.Right;
            var bottom = Bottom - indent.Bottom;
            return new Rect(left, top, Math.Max(0, right - left), Math.Max(0, bottom - top));
        }

        /// <summary>
        /// Indicates an empty dimension along any of the axes. <see cref="Width" /> или <see cref="Height" />
        /// </summary>
        public bool HasEmptyDimension() => Size.HasEmptyDimension();

        private bool ContainsInternal(int pointX, int pointY)
        {
            if (pointX >= X && (pointX - width + 1) <= X && pointY >= Y)
            {
                return (pointY - height + 1) <= Y;
            }
            return false;
        }
    }
}
