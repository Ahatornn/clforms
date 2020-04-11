using ClForms.Abstractions.Engine;

namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Custom rendering event options
    /// </summary>
    public class PaintEventArgs: System.EventArgs
    {
        /// <summary>
        /// Initialize a new instance <see cref="PaintEventArgs"/>
        /// </summary>
        public PaintEventArgs(Rect rect, IPaintContext context)
        {
            Rect = rect;
            Context = context;
        }

        /// <summary>
        /// Gets rectangular drawing context area
        /// </summary>
        public Rect Rect { get; }

        /// <summary>
        /// Gets <see cref="IPaintContext"/>
        /// </summary>
        public IPaintContext Context { get; }

    }
}
