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

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            items.Add(item);
            owner.InvalidateVisualIfItemVisible(Count);
        }

        public void Clear()
        {
            items.Clear();
            owner.InvalidateVisual();
        }

        public bool Contains(T item) => items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            owner.InvalidateVisualIfItemVisible(IndexOf(item));
            return items.Remove(item);
        }

        public int Count => items.Count;
        public bool IsReadOnly { get; } = false;
        public int IndexOf(T item) => items.IndexOf(item);

        public void Insert(int index, T item)
        {
            items.Insert(index, item);
            owner.InvalidateVisualIfItemVisible(index);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            owner.InvalidateVisualIfItemVisible(index);
        }

        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }
    }
}
