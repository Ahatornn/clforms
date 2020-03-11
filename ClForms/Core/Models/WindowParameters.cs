using ClForms.Abstractions.Engine;
using ClForms.Elements;
using System.Collections.Concurrent;

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
        /// The parameters of the update components window
        /// </summary>
        internal ConcurrentDictionary<long, InvalidateParameters> ControlContextHash { get; }

        /// <summary>
        /// Context-snapshot of the drawn window
        /// </summary>
        internal IDrawingContext Context { get; private set; }

        /// <summary>
        /// Initialize a new instance <see cref="WindowParameters"/>
        /// </summary>
        public WindowParameters(Window window, IDrawingContext context)
        {
            Window = window;
            Context = context;
            ControlContextHash = new ConcurrentDictionary<long, InvalidateParameters>();
        }

        /// <summary>
        /// Sets the new context of the drawn window. 
        /// This is required when the screen size has changed
        /// </summary>
        internal void SetContext(IDrawingContext context)
        {
            Context = context;
        }
    }
}
