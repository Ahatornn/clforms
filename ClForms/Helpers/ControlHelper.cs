using ClForms.Common;
using ClForms.Elements.Abstractions;

namespace ClForms.Helpers
{
    /// <summary>
    /// Helper for controls
    /// </summary>
    internal static class ControlHelper
    {
        /// <summary>
        /// Gets Location of target control basement screen
        /// </summary>
        internal static Point GetScreenLocation(this Control control)
        {
            var result = new Point(control.Location.X, control.Location.Y);
            var parentControl = control.Parent;
            while (parentControl != null)
            {
                result += parentControl.Location;
                parentControl = parentControl.Parent;
            }

            return result;
        }
    }
}
