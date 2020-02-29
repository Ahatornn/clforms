namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Provides data for the processed event
    /// </summary>
    public class HandledEventArgs: System.EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event was processed.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Initialize a new instance <see cref="HandledEventArgs" /> with value
        /// for the property <see cref="Handled" /> equals <see langword="false" />.
        /// </summary>
        public HandledEventArgs()
            : this(false)
        { }

        /// <summary>
        /// Initialize a new instance <see cref="HandledEventArgs" /> with default value
        /// for the property <see cref="Handled" />.
        /// </summary>
        public HandledEventArgs(bool handled)
        {
            Handled = handled;
        }
    }
}
