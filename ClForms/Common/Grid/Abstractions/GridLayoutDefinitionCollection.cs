using System.Collections;
using System.Collections.Generic;
using ClForms.Elements.Abstractions;

namespace ClForms.Common.Grid.Abstractions
{
    /// <summary>
    /// Implements basic functionality for a collection of table layout styles
    /// </summary>
    public abstract class GridLayoutDefinitionCollection<T>: IList<T> where T : GridLayoutDefinition
    {
        private readonly List<T> items;
        private readonly MultipleContentControl owner;

        /// <summary>
        /// Initialize a new instance <see cref="GridLayoutDefinitionCollection{T}"/>
        /// </summary>
        protected GridLayoutDefinitionCollection(MultipleContentControl owner)
        {
            this.owner = owner;
            items = new List<T>();
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item) => items.Add(item);

        /// <inheritdoc />
        public void Clear() => items.Clear();

        /// <inheritdoc />
        public bool Contains(T item) => items.Contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(T item) => items.Remove(item);

        /// <inheritdoc />
        public int Count => items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int IndexOf(T item) => items.IndexOf(item);

        /// <inheritdoc />
        public void Insert(int index, T item) => items.Insert(index, item);

        /// <inheritdoc />
        public void RemoveAt(int index) => items.RemoveAt(index);

        /// <inheritdoc />
        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }
    }
}
