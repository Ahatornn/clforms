using ClForms.Common;
using ClForms.Core.Contexts;
using ClForms.Elements;

namespace ClForms.Core.Models
{
    /// <summary>
    /// Parameters of the window
    /// </summary>
    internal class WindowParameters
    {
        /// <summary>
        /// Target window
        /// </summary>
        internal Window Window { get; }

        internal Rect WindowRect { get; set; }

        internal ScreenDrawingContext ParentContext { get; set; }

        internal ScreenDrawingContext CurrentBuffer { get; set; }

        /// <summary>
        /// Initialize a new instance <see cref="WindowParameters"/>
        /// </summary>
        public WindowParameters(Window window, ScreenDrawingContext parentContext)
        {
            Window = window;
            ParentContext = parentContext;
        }
    }
}
