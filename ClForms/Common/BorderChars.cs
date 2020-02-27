using System.Linq;

namespace ClForms.Common
{
    /// <summary>
    /// Defines border characters
    /// </summary>
    public class BorderChars
    {
        /// <summary>
        /// Returns the upper left corner character
        /// </summary>
        public char TopLeft { get; }

        /// <summary>
        /// Returns the upper middle character
        /// </summary>
        public char TopMiddle { get; }

        /// <summary>
        /// Returns the upper right corner character
        /// </summary>
        public char TopRight { get; }

        /// <summary>
        /// Returns the middle left character
        /// </summary>
        public char MiddleLeft { get; }

        /// <summary>
        /// Returns the middle right character
        /// </summary>
        public char MiddleRight { get; }

        /// <summary>
        /// Returns the bottom left corner character
        /// </summary>
        public char BottomLeft { get; }

        /// <summary>
        /// Returns the bottom middle character
        /// </summary>
        public char BottomMiddle { get; }

        /// <summary>
        /// Returns the bottom right corner character
        /// </summary>
        public char BottomRight { get; }

        /// <summary>
        /// Initialize a new instance <see cref="BorderChars"/>
        /// </summary>
        public BorderChars(char topLeft, char topMiddle, char topRight, char middleLeft,
            char middleRight, char bottomLeft, char bottomMiddle, char bottomRight)
        {
            TopLeft = topLeft;
            TopMiddle = topMiddle;
            TopRight = topRight;
            MiddleLeft = middleLeft;
            MiddleRight = middleRight;
            BottomLeft = bottomLeft;
            BottomMiddle = bottomMiddle;
            BottomRight = bottomRight;
        }

        /// <summary>
        /// Compare two <see cref="BorderChars" /> for equality
        /// </summary>
        public static bool operator ==(BorderChars borderChars1, BorderChars borderChars2)
        {
            if (ReferenceEquals(borderChars1, borderChars2))
            {
                return true;
            }
            if (ReferenceEquals(borderChars1, null) ||
                ReferenceEquals(borderChars2, null))
            {
                return false;
            }

            return CompareParams(borderChars1, borderChars2);
        }

        /// <summary>
        /// Compare two <see cref="BorderChars" /> for inequality
        /// </summary>
        public static bool operator !=(BorderChars borderChars1, BorderChars borderChars2)
            => !(borderChars1 == borderChars2);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is BorderChars bChar)
            {
                return this == bChar;
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = 17;
            unchecked
            {
                hash = new[]
                    {
                        TopLeft.GetHashCode(),
                        TopMiddle.GetHashCode(),
                        TopRight.GetHashCode(),
                        MiddleLeft.GetHashCode(),
                        MiddleRight.GetHashCode(),
                        BottomLeft.GetHashCode(),
                        BottomMiddle.GetHashCode(),
                        BottomRight.GetHashCode(),
                    }
                    .Aggregate(hash, (current, value) => current * 23 + value);
            }

            return hash;
        }

        internal static bool CompareParams(BorderChars borderChars1, BorderChars borderChars2)
            => borderChars1.GetHashCode() == borderChars2.GetHashCode();
    }
}
