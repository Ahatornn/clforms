using ClForms.Common;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// stub-class
    /// </summary>
    public class Button: Control
    {
        public bool IsDisabled { get; set; }

        public DialogResult DialogResult { get; set; }
    }
}
