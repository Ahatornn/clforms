using ClForms.Helpers;

namespace ClForms.Common
{
    /// <summary>
    /// Defines border characters of table
    /// </summary>
    public class GridBorderChars: BorderChars
    {
        /// <summary>
        /// Returns the inner line from top
        /// </summary>
        public char TopInner { get; }

        /// <summary>
        /// Returns the inner line from right
        /// </summary>
        public char RightInner { get; }

        /// <summary>
        /// Returns the inner line from bottom
        /// </summary>
        public char BottomInner { get; }

        /// <summary>
        /// Returns the inner line from left
        /// </summary>
        public char LeftInner { get; }

        /// <summary>
        /// Returns the vertical inner line
        /// </summary>
        public char VerticalInner { get; }

        /// <summary>
        /// Returns the horizontal inner line
        /// </summary>
        public char HorizontalInner { get; }

        /// <summary>
        /// Returns the cross of inner lines
        /// </summary>
        public char CrossInner { get; }

        /// <summary>
        /// Initialize a new instance <see cref="GridBorderChars"/>
        /// </summary>
        public GridBorderChars(BorderChars borderChars, char topInner, char rightInner, char bottomInner,
            char leftInner, char verticalInner, char horizontalInner, char crossInner)
            : this(borderChars.TopLeft, borderChars.TopMiddle, borderChars.TopRight, borderChars.MiddleLeft,
                borderChars.MiddleRight, borderChars.BottomLeft, borderChars.BottomMiddle, borderChars.BottomRight,
                topInner, rightInner, bottomInner, leftInner, verticalInner, horizontalInner, crossInner)
        { }

        /// <summary>
        /// Initialize a new instance <see cref="GridBorderChars"/>
        /// </summary>
        public GridBorderChars(char topLeft, char topMiddle, char topRight, char middleLeft,
            char middleRight, char bottomLeft, char bottomMiddle, char bottomRight,
            char topInner, char rightInner, char bottomInner, char leftInner,
            char verticalInner, char horizontalInner, char crossInner)
            : base(topLeft, topMiddle, topRight, middleLeft, middleRight, bottomLeft, bottomMiddle, bottomRight)
        {
            TopInner = topInner;
            RightInner = rightInner;
            BottomInner = bottomInner;
            LeftInner = leftInner;
            VerticalInner = verticalInner;
            HorizontalInner = horizontalInner;
            CrossInner = crossInner;
        }

        /// <summary>
        /// Compare two <see cref="BorderChars" /> for equality
        /// </summary>
        public static bool operator ==(GridBorderChars gridBorderChars1, GridBorderChars gridBorderChars2)
        {
            if (ReferenceEquals(gridBorderChars1, gridBorderChars2))
            {
                return true;
            }
            if (ReferenceEquals(gridBorderChars1, null) ||
                ReferenceEquals(gridBorderChars2, null))
            {
                return false;
            }

            if (BorderChars.CompareParams(gridBorderChars1, gridBorderChars2))
            {
                return gridBorderChars1.TopInner == gridBorderChars2.TopInner &&
                       gridBorderChars1.RightInner == gridBorderChars2.RightInner &&
                       gridBorderChars1.BottomInner == gridBorderChars2.BottomInner &&
                       gridBorderChars1.LeftInner == gridBorderChars2.LeftInner &&
                       gridBorderChars1.VerticalInner == gridBorderChars2.VerticalInner &&
                       gridBorderChars1.HorizontalInner == gridBorderChars2.HorizontalInner &&
                       gridBorderChars1.CrossInner == gridBorderChars2.CrossInner;
            }

            return false;
        }

        /// <summary>
        /// Compare two <see cref="BorderChars" /> for inequality
        /// </summary>
        public static bool operator !=(GridBorderChars gridBorderChars1, GridBorderChars gridBorderChars2)
            => !(gridBorderChars1 == gridBorderChars2);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is GridBorderChars bChar)
            {
                return this == bChar;
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => GetHashCodeHelper.CalculateHashCode(base.GetHashCode(),
                TopInner.GetHashCode(),
                RightInner.GetHashCode(),
                BottomInner.GetHashCode(),
                LeftInner.GetHashCode(),
                VerticalInner.GetHashCode(),
                HorizontalInner.GetHashCode(),
                CrossInner.GetHashCode());

        internal static bool CompareParams(GridBorderChars borderChars1, GridBorderChars borderChars2)
            => borderChars1.GetHashCode() == borderChars2.GetHashCode();
    }
}
