using System;
using ClForms.Elements;

namespace ClForms.Common
{
    /// <summary>
    /// Displays a single column header in a <see cref="ListView"/> control
    /// </summary>
    public class ColumnHeader<T>
    {
        /// <summary>
        /// Initialize a new instance <see cref="ColumnHeader{T}"/>
        /// </summary>
        public ColumnHeader(string text, Func<T, string> displayMember, int? width, TextAlignment alignment)
        {
            Text = text;
            DisplayMember = displayMember;
            Width = width;
            Alignment = alignment;
        }

        /// <summary>
        /// Gets or sets the text displayed in the column header
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets or sets the property to display in control column
        /// </summary>
        public Func<T, string> DisplayMember { get; }

        /// <summary>
        /// Gets or sets the width of the column
        /// </summary>
        public int? Width { get; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the content
        /// </summary>
        public TextAlignment Alignment { get; }
    }
}
