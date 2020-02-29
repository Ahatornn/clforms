namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Contains property change event data
    /// </summary>
    public class PropertyChangedEventArgs<T>: System.EventArgs
    {
        /// <summary>
        /// Previous property value
        /// </summary>
        public T OldValue { get; }

        /// <summary>
        /// New property value
        /// </summary>
        public T NewValue { get; }

        /// <summary>
        /// Creates property change event arguments
        /// </summary>
        public PropertyChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
