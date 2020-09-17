using ClForms.Themes;

namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Provides data for an event of drawing item
    /// </summary>
    public class ListBoxItemStyleEventArgs<T>: System.EventArgs
    {
        /// <summary>
        /// Gets the target item
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// Gets or sets the background color of target item
        /// </summary>
        public Color Background { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of target item
        /// </summary>
        public Color Foreground { get; set; }

        public ListBoxItemStyleEventArgs(T item, Color background, Color foreground)
        {
            Item = item;
            Background = background;
            Foreground = foreground;
        }
    }
}
