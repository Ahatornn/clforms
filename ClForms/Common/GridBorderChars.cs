using ClForms.Helpers;

namespace ClForms.Common
{
    /// <summary>
    /// Defines border characters of table
    /// </summary>
    public class GridBorderChars: BorderChars
    {
        /// <summary>
        /// Returns the intersection character
        /// </summary>
        public char MiddleCenter { get; }

        /// <summary>
        /// Returns the character of vertical lines
        /// </summary>
        public char VerticalLine { get; }

        /// <summary>
        /// Returns the character of horizontal lines
        /// </summary>
        public char HorizontalLine { get; }

        /// <summary>
        /// Initialize a new instance <see cref="GridBorderChars"/>
        /// </summary>
        public GridBorderChars(BorderChars borderChars, char middleCenter, char vertLine, char horzLine)
            : this(borderChars.TopLeft, borderChars.TopMiddle, borderChars.TopRight, borderChars.MiddleLeft, middleCenter,
                borderChars.MiddleRight, borderChars.BottomLeft, borderChars.BottomMiddle, borderChars.BottomRight,
                vertLine, horzLine)
        { }

        /// <summary>
        /// Initialize a new instance <see cref="GridBorderChars"/>
        /// </summary>
        public GridBorderChars(char topLeft, char topMiddle, char topRight, char middleLeft, char middleCenter,
            char middleRight, char bottomLeft, char bottomMiddle, char bottomRight, char vertLine, char horzLine)
            : base(topLeft, topMiddle, topRight, middleLeft, middleRight, bottomLeft, bottomMiddle, bottomRight)
        {
            MiddleCenter = middleCenter;
            VerticalLine = vertLine;
            HorizontalLine = horzLine;
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

            if (CompareParams(gridBorderChars1, gridBorderChars2))
            {
                return gridBorderChars1.MiddleCenter == gridBorderChars2.MiddleCenter &&
                       gridBorderChars1.VerticalLine == gridBorderChars2.VerticalLine &&
                       gridBorderChars1.HorizontalLine == gridBorderChars2.HorizontalLine;
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
                MiddleCenter.GetHashCode(),
                VerticalLine.GetHashCode(),
                HorizontalLine.GetHashCode());
    }
}
