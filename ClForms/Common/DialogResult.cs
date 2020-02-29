namespace ClForms.Common
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a dialog box
    /// </summary>
    public enum DialogResult
    {
        /// <summary>
        /// <see langword="Nothing" /> returns from the dialog box.
        /// This means that the modal dialog continues
        /// </summary>
        None,

        /// <summary>
        /// The return value of the dialog box is <see langword="OK" />
        /// </summary>
        OK,

        /// <summary>
        /// The return value of the dialog box is <see langword="Cancel" />
        /// </summary>
        Cancel,

        /// <summary>
        /// The return value of the dialog box is <see langword="Abort" />
        /// </summary>
        Abort,

        /// <summary>
        /// The return value of the dialog box is <see langword="Retry" />
        /// </summary>
        Retry,

        /// <summary>
        /// The return value of the dialog box is <see langword="Ignore" />
        /// </summary>
        Ignore,

        /// <summary>
        /// The return value of the dialog box is <see langword="Yes" />
        /// </summary>
        Yes,

        /// <summary>
        /// The return value of the dialog box is <see langword="No" />
        /// </summary>
        No,
    }
}
