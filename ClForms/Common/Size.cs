using System;
using ClForms.Helpers;

namespace ClForms.Common
{
    /// <summary>
    /// Implements a structure that is used to describe the size of an object
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// Zero-sized <see cref="Size"/>
        /// </summary>
        public static Size Empty => new Size(0, 0);

        private int width;
        private int height;

        /// <summary>
        /// Initialize a new instance <see cref="Size"/>
        /// </summary>
        public Size(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Width and height cannot be negative");
            }
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Compare two <see cref="Size" /> for equality
        /// </summary>
        public static bool operator ==(Size size1, Size size2)
        {
            if (size1.Width == size2.Width)
            {
                return size1.Height == size2.Height;
            }
            return false;
        }

        /// <summary>
        /// Compare two <see cref="Size" /> for inequality
        /// </summary>
        public static bool operator !=(Size size1, Size size2) => !(size1 == size2);

        /// <summary>
        /// Performs addition two <see cref="Size" /> objects
        /// </summary>
        public static Size operator +(Size s1, Size s2) => new Size(s1.Width + s2.Width, s1.Height + s2.Height);

        /// <summary>
        /// Explicitly converts an instance of <see cref="Size" /> to an instance of <see cref="Point" />.
        /// </summary>
        public static explicit operator Point(Size size) => new Point(size.Width, size.Height);

        /// <summary>
        /// Explicitly converts an instance of <see cref="Size" /> to an instance of <see cref="Rect" />.
        /// </summary>
        public static explicit operator Rect(Size size) => new Rect(size);

        /// <summary>
        /// Compare two <see cref="Size" /> for equality
        /// </summary>
        public static bool Equals(Size size1, Size size2)
        {
            if (size1.Width.Equals(size2.Width))
            {
                return size1.Height.Equals(size2.Height);
            }
            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object o)
        {
            if (o == null || !(o is Size))
            {
                return false;
            }
            return Size.Equals(this, (Size) o);
        }

        /// <summary>
        /// Compares the value of the <see cref="Size" /> instance to equality
        /// </summary>
        public bool Equals(Size value) => Size.Equals(this, value);

        /// <inheritdoc />
        public override int GetHashCode()
            => GetHashCodeHelper.CalculateHashCode(Width, Height);

        /// <inheritdoc />
        public override string ToString() => $"{{{width}, {height}}}";

        /// <summary>
        /// Gets or sets the width
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
        /// Gets or sets the height
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
        /// Indicates an empty dimension along any of the axes. <see cref="Width" /> или <see cref="Height" />
        /// </summary>
        public bool HasEmptyDimension() => width == 0 || height == 0;
    }
}
