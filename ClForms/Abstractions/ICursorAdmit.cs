using ClForms.Common;

namespace ClForms.Abstractions
{
    /// <summary>
    /// Accepts a cursor for keyboard input
    /// </summary>
    public interface ICursorAdmit
    {
        /// <summary>
        /// Returns <see cref="Point" /> determines the coordinates of the cursor in the component
        /// </summary>
        Point CursorPosition { get; }
    }
}
