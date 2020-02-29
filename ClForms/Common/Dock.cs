namespace ClForms.Common
{
    /// <summary>
    /// Specifies the position and manner in which a control is docked
    /// </summary>
    public enum Dock
    {
        /// <summary>
        /// All the control's edges are docked to the all edges of its containing control and sized appropriately
        /// </summary>
        Left,

        /// <summary>
        /// The control's top edge is docked to the top of its containing control
        /// </summary>
        Top,

        /// <summary>
        /// The control's right edge is docked to the right edge of its containing control
        /// </summary>
        Right,

        /// <summary>
        /// The control's bottom edge is docked to the bottom of its containing control
        /// </summary>
        Bottom,
    }
}
