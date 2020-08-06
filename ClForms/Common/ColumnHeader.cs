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
        /// Gets or sets the text displayed in the column header
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the property to display in control column
        /// </summary>
        public Func<T, string> DisplayMember { get; set; }

        /// <summary>
        /// Gets or sets the width of the column
        /// </summary>
        public int? Width { get; set; }
    }
}
