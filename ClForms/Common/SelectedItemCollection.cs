using ClForms.Elements.Abstractions;

namespace ClForms.Common
{
    /// <summary>
    /// This class represent the collection of SelectedItems in control
    /// </summary>
    public class SelectedItemCollection<T>: ListBoxCollection<T>
    {
        private readonly ListBoxBase<T> owner;

        public SelectedItemCollection(ListBoxBase<T> owner)
        {
            this.owner = owner;
        }

        /// <inheritdoc cref="ListBoxCollection{T}.BeginUpdateInterceptor"/>
        protected override void BeginUpdateInterceptor() { }

        /// <inheritdoc cref="ListBoxCollection{T}.EndUpdateInterceptor"/>
        protected override void EndUpdateInterceptor()
        {
            if (!owner.IsInUpdateState)
            {
                owner.InvalidateVisual();
            }
        }
    }
}
