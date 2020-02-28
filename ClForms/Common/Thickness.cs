using ClForms.Helpers;

namespace ClForms.Common
{
    /// <summary>
    /// Represents information about internal fields and fields associated with a user interface (UI) element.
    /// </summary>
    public struct Thickness
    {
        /// <summary>
        /// Returns <see cref="Thickness" /> with default property values
        /// </summary>
        public static readonly Thickness Empty = new Thickness(0);

        private bool uniformLength;
        private int top;
        private int left;
        private int right;
        private int bottom;

        /// <summary>
        /// Initialize a new instance <see cref="Thickness"/>
        /// </summary>
        public Thickness(int uniformLength)
        {
            this.uniformLength = true;
            top = left = right = bottom = uniformLength;
        }

        /// <summary>
        /// Initialize a new instance <see cref="Thickness"/>
        /// </summary>
        public Thickness(int left, int top, int right, int bottom)
        {
            this.top = top;
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            uniformLength = this.top == this.left && this.top == this.right && this.top == this.bottom;
        }

        /// <summary>
        /// Initialize a new instance <see cref="Thickness"/>
        /// </summary>
        public Thickness(int leftAndRight, int topAndBottom)
        {
            left = right = leftAndRight;
            top = bottom = topAndBottom;
            uniformLength = top == left && top == right && top == bottom;
        }

        /// <summary>
        /// Gets or sets the value of internal fields for all edges
        /// </summary>
        public int All
        {
            get
            {
                if (!uniformLength)
                {
                    return -1;
                }
                return top;
            }
            set
            {
                if (uniformLength && top == value)
                {
                    return;
                }
                uniformLength = true;
                top = left = right = bottom = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the inner margins for the bottom edge
        /// </summary>
        public int Bottom
        {
            get => uniformLength ? top : bottom;
            set
            {
                if (!uniformLength && bottom == value)
                {
                    return;
                }
                uniformLength = false;
                bottom = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the inner margins for the left edge
        /// </summary>
        public int Left
        {
            get => uniformLength ? top : left;
            set
            {
                if (!uniformLength && left == value)
                {
                    return;
                }
                uniformLength = false;
                left = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the inner margins for the right edge
        /// </summary>
        public int Right
        {
            get => uniformLength ? top : right;
            set
            {
                if (!uniformLength && right == value)
                {
                    return;
                }
                uniformLength = false;
                right = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the inner margins for the top edge
        /// </summary>
        public int Top
        {
            get => top;
            set
            {
                if (!uniformLength && top == value)
                {
                    return;
                }
                uniformLength = false;
                top = value;
            }
        }

        /// <summary>
        /// Gets the merged internal margins for the right and left edges.
        /// </summary>
        public int Horizontal => Left + Right;

        /// <summary>
        /// Gets the merged internal margins for the top and bottom edges.
        /// </summary>
        public int Vertical => Top + Bottom;

        /// <summary>
        /// Returns information in the form <see cref="Size" />.
        /// </summary>
        public Size Size => new Size(Horizontal, Vertical);

        /// <summary>
        /// Calculates the sum of two given <see cref="Thickness" /> values
        /// </summary>
        public static Thickness Add(Thickness p1, Thickness p2) => p1 + p2;

        /// <summary>
        /// Subtracts one specified value of type <see cref="Thickness" /> from another
        /// </summary>
        public static Thickness Subtract(Thickness p1, Thickness p2) => p1 - p2;

        /// <summary>
        /// Determines whether the value of the specified object is equivalent to the value of the current <see cref="Thickness" />
        /// </summary>
        public override bool Equals(object other)
        {
            if (other is Thickness thickness)
            {
                return thickness == this;
            }
            return false;
        }

        /// <summary>
        /// Performs addition two <see cref="Thickness" /> objects
        /// </summary>
        public static Thickness operator +(Thickness p1, Thickness p2) => new Thickness(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);

        /// <summary>
        /// Subtracts two <see cref="Thickness" /> objects
        /// </summary>
        public static Thickness operator -(Thickness p1, Thickness p2) => new Thickness(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);

        /// <summary>
        /// Compare two <see cref="Thickness" /> for equality
        /// </summary>
        public static bool operator ==(Thickness p1, Thickness p2)
        {
            if (p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right)
            {
                return p1.Bottom == p2.Bottom;
            }
            return false;
        }

        /// <summary>
        /// Compare two <see cref="Thickness" /> for inequality
        /// </summary>
        public static bool operator !=(Thickness p1, Thickness p2) => !(p1 == p2);

        /// <inheritdoc />
        public override int GetHashCode()
        => GetHashCodeHelper.CalculateHashCode(Left,
            Top,
            Right,
            Bottom);

        /// <inheritdoc />
        public override string ToString() => $"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";
    }
}
