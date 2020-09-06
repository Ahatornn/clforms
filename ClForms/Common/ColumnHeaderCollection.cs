using System;
using System.Collections;
using System.Collections.Generic;
using ClForms.Elements;

namespace ClForms.Common
{
    /// <summary>
    /// Represents the collection of column headers in a <see cref="ListView"/> control
    /// </summary>
    public class ColumnHeaderCollection<T>: IList<ColumnHeader<T>>
    {
        private readonly List<ColumnHeader<T>> items;
        private readonly ListView<T> owner;

        internal ColumnHeaderCollection(ListView<T> owner)
        {
            this.owner = owner;
            items = new List<ColumnHeader<T>>();
        }

        /// <inheritdoc />
        public IEnumerator<ColumnHeader<T>> GetEnumerator() => items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        public void Add(string text) => Add(new ColumnHeader<T> {Text = text});

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        public void Add(string text, int? width) => Add(new ColumnHeader<T> {Text = text, Width = width});

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        public void Add(string text, Func<T, string> displayMember) => Add(new ColumnHeader<T> { Text = text, DisplayMember = displayMember });

        /// <inheritdoc />
        public void Add(ColumnHeader<T> item)
        {
            items.Add(item);
            owner.InvalidateVisual();
        }

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        public void Add(string text, Func<T, string> displayMember, int? width, TextAlignment alignment = TextAlignment.Left) =>
            Add(new ColumnHeader<T> {Text = text, DisplayMember = displayMember, Alignment = alignment, Width = width});

        /// <inheritdoc />
        public void Clear()
        {
            items.Clear();
            owner.InvalidateVisual();
        }

        /// <inheritdoc />
        public bool Contains(ColumnHeader<T> item) => items.Contains(item);

        /// <inheritdoc />
        public void CopyTo(ColumnHeader<T>[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(ColumnHeader<T> item)
        {
            owner.InvalidateVisual();
            return items.Remove(item);
        }

        /// <inheritdoc />
        public int Count => items.Count;

        /// <inheritdoc />
        public bool IsReadOnly { get; } = false;

        /// <inheritdoc />
        public int IndexOf(ColumnHeader<T> item) => items.IndexOf(item);

        /// <inheritdoc />
        public void Insert(int index, ColumnHeader<T> item)
        {
            items.Insert(index, item);
            owner.InvalidateVisual();
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            owner.InvalidateVisual();
        }

        /// <inheritdoc />
        public ColumnHeader<T> this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }
    }
}
