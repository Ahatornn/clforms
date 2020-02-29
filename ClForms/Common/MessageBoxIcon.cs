namespace ClForms.Common
{
    /// <summary>
    /// Specifies constants defining which information to display
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// The message box contains no symbols
        /// </summary>
        None = 0,

        /// <summary>
        /// The message box contains a symbol consisting of white X in a box
        /// </summary>
        Error = 16,

        /// <summary>
        /// The message box contains a symbol consisting of a question mark in a box.
        /// </summary>
        Question = 32,

        /// <summary>
        /// The message box contains a symbol consisting of an exclamation point in a box
        /// </summary>
        Warning = 48,

        /// <summary>
        /// The message box contains a symbol consisting of a lowercase letter i in a box
        /// </summary>
        Information = 64
    }
}
