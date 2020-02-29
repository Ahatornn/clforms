namespace ClForms.Common
{
    /// <summary>
    /// Indicates the status of a control, such as a check box, that can be checked, unchecked, or set to undefined
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// Control unchecked
        /// </summary>
        Unchecked = 0,

        /// <summary>
        /// Control was checked
        /// </summary>
        Checked = 1,

        /// <summary>
        /// Control indeterminate
        /// </summary>
        Indeterminate = 2,
    }
}
