namespace ClForms.Abstractions.Core
{
    /// <summary>
    /// Represents the life cycle of controls
    /// </summary>
    public interface IControlLifeCycle
    {
        /// <summary>
        /// Gets a value of the control's identifier
        /// </summary>
        long GetId();
    }
}
