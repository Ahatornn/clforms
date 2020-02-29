namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Provides data for an event of char pressed
    /// </summary>
    public class CharPressEventArgs: HandledEventArgs
    {
        /// <summary>
        /// Gets or sets the character of the pressed key
        /// </summary>
        public char KeyChar { get; }

        /// <summary>
        /// Initialize a new instance <see cref="CharPressEventArgs"/>
        /// </summary>
        public CharPressEventArgs(char keyChar)
        {
            KeyChar = keyChar;
        }
    }
}
