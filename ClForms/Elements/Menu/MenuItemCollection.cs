using System;
using System.Collections;
using System.Collections.Generic;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements.Menu
{
    /// <summary>
    /// Represents a collection of <see cref="MenuItemBase"/> objects
    /// </summary>
    public class MenuItemCollection: IList<MenuItemBase>
    {
        private readonly List<MenuItemBase> items;
        private readonly MenuItemBase parent;

        /// <summary>
        /// Initialize a new instance <see cref="MenuItemCollection"/>
        /// </summary>
        public MenuItemCollection(MenuItemBase parent)
        {
            items = new List<MenuItemBase>();
            this.parent = parent;
        }

        /// <inheritdoc />
        public IEnumerator<MenuItemBase> GetEnumerator() => items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(MenuItemBase item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.SetParent(parent);
            items.Add(item);
        }

        /// <inheritdoc />
        public void Clear() => items.Clear();

        /// <inheritdoc />
        public bool Contains(MenuItemBase item) => items.Contains(item);

        /// <inheritdoc />
        public void CopyTo(MenuItemBase[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(MenuItemBase item) => items.Remove(item);

        /// <inheritdoc />
        public int Count => items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int IndexOf(MenuItemBase item) => items.IndexOf(item);

        /// <inheritdoc />
        public void Insert(int index, MenuItemBase item) => items.Insert(index, item);

        /// <inheritdoc />
        public void RemoveAt(int index) => items.RemoveAt(index);

        /// <inheritdoc />
        public MenuItemBase this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }
    }
}
