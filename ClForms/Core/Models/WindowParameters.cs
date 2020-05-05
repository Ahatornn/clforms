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

        /// <summary>
        /// Gets or sets the <see cref="ScreenDrawingContext"/> of the parent window
        /// </summary>
        internal ScreenDrawingContext ParentContext { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ScreenDrawingContext"/> of the current window
        /// </summary>
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
