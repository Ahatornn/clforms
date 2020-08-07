using System.Collections;
using System.Collections.Generic;
using ClForms.Elements;

namespace ClForms.Common
{
    /// <summary>
    /// Represents the collection of items in a <see cref="ListView"/> control
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListViewItemCollection<T>: IList<T>
    {
        private readonly ListView<T> owner;
        private readonly List<T> items;

        internal ListViewItemCollection(ListView<T> owner)
        {
            this.owner = owner;
            items = new List<T>();
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(T item)
        {
            items.Add(item);
            owner.InvalidateVisualIfItemVisible(Count);
        }

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(IEnumerable<T> item)
        {
            items.AddRange(item);
            owner.InvalidateVisualIfItemVisible(Count);
        }

        /// <inheritdoc />
        public void Clear()
        {
            items.Clear();
            owner.InvalidateVisual();
        }

        /// <inheritdoc />
        public bool Contains(T item) => items.Contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(T item)
        {
            owner.InvalidateVisualIfItemVisible(IndexOf(item));
            return items.Remove(item);
        }

        /// <inheritdoc />
        public int Count => items.Count;

        /// <inheritdoc />
        public bool IsReadOnly { get; } = false;

        /// <inheritdoc />
        public int IndexOf(T item) => items.IndexOf(item);

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            items.Insert(index, item);
            owner.InvalidateVisualIfItemVisible(index);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            owner.InvalidateVisualIfItemVisible(index);
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }
    }
}
