using ClForms.Elements;
using System.Collections.Concurrent;
using System.Linq;

namespace ClForms.Core.Models
{
    internal sealed class WindowCollection : ConcurrentStack<WindowParameters>
    {
        /// <summary>
        /// Getting parameters of specified window
        /// </summary>
        internal WindowParameters GetWindowParameters(Window wnd)
            => this.Single(x => x.Window.Id == wnd.Id);
    }
}
