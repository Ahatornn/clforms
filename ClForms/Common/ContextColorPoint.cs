using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Common
{
    /// <summary>
    /// Describes the color of the background and text.
    /// </summary>
    public struct ContextColorPoint
    {
        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the background
        /// </summary>
        public Color Background { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the text
        /// </summary>
        public Color Foreground { get; }

        /// <summary>
        /// Initialize a new instance <see cref="ContextColorPoint"/>
        /// </summary>
        public ContextColorPoint(Color background, Color foreground)
        {
            Background = background;
            Foreground = foreground;
        }

        /// <summary>
        /// Compare two <see cref="ContextColorPoint" /> for equality
        /// </summary>
        public static bool operator ==(ContextColorPoint point1, ContextColorPoint point2)
        {
            if (point1.Background == point2.Background)
            {
                return point1.Foreground == point2.Foreground;
            }
            return false;
        }

        /// <summary>
        /// Compare two <see cref="ContextColorPoint" /> for inequality
        /// </summary>
        public static bool operator !=(ContextColorPoint point1, ContextColorPoint point2)
            => !(point1 == point2);

        /// <summary>
        /// Compare two <see cref="ContextColorPoint" /> for equality
        /// </summary>
        public static bool Equals(ContextColorPoint point1, ContextColorPoint point2)
        {
            if (point1.Background.Equals(point2.Background))
            {
                return point1.Foreground.Equals(point2.Foreground);
            }
            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object o)
        {
            if (o == null || !(o is ContextColorPoint))
            {
                return false;
            }
            return Equals(this, (ContextColorPoint) o);
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => GetHashCodeHelper.CalculateHashCode(Background.GetHashCode(), Foreground.GetHashCode());
    }
}
