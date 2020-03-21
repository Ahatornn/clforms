namespace ClForms.Common.Grid
{
    /// <summary>
    /// Specifies how rows or columns of user interface (UI) elements should be sized relative to their container
    /// </summary>
    public enum SizeType
    {
        /// <summary>
        /// The row or column should be automatically sized to share space with its peers
        /// </summary>
        AutoSize = 1,

        /// <summary>
        /// The row or column should be sized to an exact number of pixels
        /// </summary>
        Absolute = 2,

        /// <summary>
        /// The row or column should be sized as a percentage of the parent container
        /// </summary>
        Percent = 3,
    }
}
