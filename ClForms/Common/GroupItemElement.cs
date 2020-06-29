using ClForms.Elements.Abstractions;

namespace ClForms.Common
{
    /// <summary>
    /// Element of <see cref="GroupItemControl"/>
    /// </summary>
    public class GroupItemElement
    {
        /// <summary>
        /// Gets or sets the text associated with this element
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the element cannot respond to user interaction
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
