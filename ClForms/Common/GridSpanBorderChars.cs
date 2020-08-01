using ClForms.Helpers;

namespace ClForms.Common
{
    public class GridSpanBorderChars: GridBorderChars
    {
        /// <summary>
        /// Returns the inner line with top spanned
        /// </summary>
        public char CrossSpanTopInner { get; }

        /// <summary>
        /// Returns the inner line with right spanned
        /// </summary>
        public char CrossSpanRightInner { get; }

        /// <summary>
        /// Returns the inner line with bottom spanned
        /// </summary>
        public char CrossSpanBottomInner { get; }

        /// <summary>
        /// Returns the inner line with left spanned
        /// </summary>
        public char CrossSpanLeftInner { get; }

        /// <summary>
        /// Returns the inner line of left-top spanned corner
        /// </summary>
        public char CrossSpanCornerTopInner { get; }

        /// <summary>
        /// Returns the inner line of right-top spanned corner
        /// </summary>
        public char CrossSpanCornerRightInner { get; }

        /// <summary>
        /// Returns the inner line of right-bottom spanned corner
        /// </summary>
        public char CrossSpanCornerBottomInner { get; }

        /// <summary>
        /// Returns the inner line of left-bottom spanned corner
        /// </summary>
        public char CrossSpanCornerLeftInner { get; }

        public GridSpanBorderChars(BorderChars borderChars, char topInner, char rightInner, char bottomInner, char leftInner,
            char verticalInner, char horizontalInner, char crossInner,
            char crossSpanTopInner, char crossSpanRightInner, char crossSpanBottomInner, char crossSpanLeftInner,
            char crossSpanCornerTopInner, char crossSpanCornerRightInner, char crossSpanCornerBottomInner, char crossSpanCornerLeftInner)
            : this(borderChars.TopLeft, borderChars.TopMiddle, borderChars.TopRight, borderChars.MiddleLeft, borderChars.MiddleRight,
                borderChars.BottomLeft, borderChars.BottomMiddle, borderChars.BottomRight,
                topInner, rightInner, bottomInner, leftInner, verticalInner, horizontalInner, crossInner,
                crossSpanTopInner, crossSpanRightInner, crossSpanBottomInner, crossSpanLeftInner,
                crossSpanCornerTopInner, crossSpanCornerRightInner, crossSpanCornerBottomInner, crossSpanCornerLeftInner)
        {
        }

        public GridSpanBorderChars(char topLeft, char topMiddle, char topRight, char middleLeft, char middleRight,
            char bottomLeft, char bottomMiddle, char bottomRight,
            char topInner, char rightInner, char bottomInner, char leftInner,
            char verticalInner, char horizontalInner, char crossInner,
            char crossSpanTopInner, char crossSpanRightInner, char crossSpanBottomInner, char crossSpanLeftInner,
            char crossSpanCornerTopInner, char crossSpanCornerRightInner, char crossSpanCornerBottomInner, char crossSpanCornerLeftInner)
            : base(topLeft, topMiddle, topRight, middleLeft, middleRight,
                bottomLeft, bottomMiddle, bottomRight,
                topInner, rightInner, bottomInner, leftInner,
                verticalInner, horizontalInner, crossInner)
        {
            CrossSpanTopInner = crossSpanTopInner;
            CrossSpanRightInner = crossSpanRightInner;
            CrossSpanBottomInner = crossSpanBottomInner;
            CrossSpanLeftInner = crossSpanLeftInner;
            CrossSpanCornerTopInner = crossSpanCornerTopInner;
            CrossSpanCornerRightInner = crossSpanCornerRightInner;
            CrossSpanCornerBottomInner = crossSpanCornerBottomInner;
            CrossSpanCornerLeftInner = crossSpanCornerLeftInner;
        }

        /// <summary>
        /// Compare two <see cref="BorderChars" /> for equality
        /// </summary>
        public static bool operator ==(GridSpanBorderChars gridSpanBorderChars1, GridSpanBorderChars gridSpanBorderChars2)
        {
            if (ReferenceEquals(gridSpanBorderChars1, gridSpanBorderChars2))
            {
                return true;
            }
            if (ReferenceEquals(gridSpanBorderChars1, null) ||
                ReferenceEquals(gridSpanBorderChars2, null))
            {
                return false;
            }

            if (GridBorderChars.CompareParams(gridSpanBorderChars1, gridSpanBorderChars2))
            {
                return gridSpanBorderChars1.CrossSpanTopInner == gridSpanBorderChars2.CrossSpanTopInner &&
                       gridSpanBorderChars1.CrossSpanRightInner == gridSpanBorderChars2.CrossSpanRightInner &&
                       gridSpanBorderChars1.CrossSpanBottomInner == gridSpanBorderChars2.CrossSpanBottomInner &&
                       gridSpanBorderChars1.CrossSpanLeftInner == gridSpanBorderChars2.CrossSpanLeftInner &&
                       gridSpanBorderChars1.CrossSpanCornerTopInner == gridSpanBorderChars2.CrossSpanCornerTopInner &&
                       gridSpanBorderChars1.CrossSpanCornerRightInner == gridSpanBorderChars2.CrossSpanCornerRightInner &&
                       gridSpanBorderChars1.CrossSpanCornerBottomInner == gridSpanBorderChars2.CrossSpanCornerBottomInner &&
                       gridSpanBorderChars1.CrossSpanCornerLeftInner == gridSpanBorderChars2.CrossSpanCornerLeftInner;
            }

            return false;
        }

        /// <summary>
        /// Compare two <see cref="BorderChars" /> for inequality
        /// </summary>
        public static bool operator !=(GridSpanBorderChars gridSpanBorderChars1, GridSpanBorderChars gridSpanBorderChars2)
            => !(gridSpanBorderChars1 == gridSpanBorderChars2);

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
                CrossSpanTopInner.GetHashCode(),
                CrossSpanRightInner.GetHashCode(),
                CrossSpanBottomInner.GetHashCode(),
                CrossSpanLeftInner.GetHashCode(),
                CrossSpanCornerTopInner.GetHashCode(),
                CrossSpanCornerRightInner.GetHashCode(),
                CrossSpanCornerBottomInner.GetHashCode(),
                CrossSpanCornerLeftInner.GetHashCode());

        internal static bool CompareParams(GridSpanBorderChars borderChars1, GridSpanBorderChars borderChars2)
            => borderChars1.GetHashCode() == borderChars2.GetHashCode();
    }
}
