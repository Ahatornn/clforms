using System;
using ClForms.Elements.Abstractions;

namespace ClForms.Common
{
    /// <summary>
    /// Represents a collection of elements in <see cref="Elements.ListBox" />.
    /// </summary>
    public class ItemCollection<T>: ListBoxCollection<T>
    {
        private readonly ListBoxBase<T> owner;

        public ItemCollection(ListBoxBase<T> owner)
        {
            this.owner = owner;
        }

        /// <inheritdoc cref="ListBoxCollection{T}.BeginUpdateInterceptor"/>
        protected override void BeginUpdateInterceptor() => owner.BeginUpdate();

        /// <inheritdoc cref="ListBoxCollection{T}.EndUpdateInterceptor"/>
        protected override void EndUpdateInterceptor() => owner.EndUpdate();

        /// <inheritdoc cref="ListBoxCollection{T}.ClearInternal"/>
        protected override void ClearInternal()
        {
            BeginUpdateInterceptor();
            Items.Clear();
            owner.ClearSelected();
            EndUpdateInterceptor();
        }

        /// <inheritdoc cref="ListBoxCollection{T}.RemoveAtInternal"/>
        protected override void RemoveAtInternal(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            BeginUpdateInterceptor();
            owner.ClearSelected(Items[index]);
            Items.RemoveAt(index);
            EndUpdateInterceptor();
        }
    }
}
