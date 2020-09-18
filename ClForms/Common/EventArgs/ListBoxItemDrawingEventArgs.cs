using ClForms.Abstractions.Engine;
using ClForms.Themes;

namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Provides data for an event of custom drawing item
    /// </summary>
    public class ListBoxItemDrawingEventArgs<T> : System.EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event was handled
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets the target item
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// Gets the background color of target item
        /// </summary>
        public Color Background { get; }

        /// <summary>
        /// Gets the foreground color of target item
        /// </summary>
        public Color Foreground { get; }

        /// <summary>
        /// Gets the <see cref="Rect"/> of area for drawing target item
        /// </summary>
        public Rect DrawingArea { get; }

        /// <summary>
        /// Gets the <see cref="ColumnHeader{T}"/> of current header.
        /// </summary>
        public ColumnHeader<T> Header { get; }

        /// <summary>
        /// Gets the drawing context
        /// </summary>
        public IPaintContext Context { get; }

        /// <summary>
        /// Initialize a new instance <see cref="ListBoxItemDrawingEventArgs"/>
        /// </summary>
        public ListBoxItemDrawingEventArgs(T item, Color background, Color foreground,
            Rect drawingArea, ColumnHeader<T> header, IPaintContext context)
        {
            Handled = false;
            Item = item;
            Background = background;
            Foreground = foreground;
            DrawingArea = drawingArea;
            Header = header;
            Context = context;
        }
    }
}
