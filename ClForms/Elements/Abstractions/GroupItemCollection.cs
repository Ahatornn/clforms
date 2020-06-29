using System.Collections;
using System.Collections.Generic;
using ClForms.Common;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Represents a collection of elements in <see cref="GroupItemControl" />
    /// </summary>
    public class GroupItemCollection: IList<GroupItemElement>
    {
        private readonly GroupItemControl owner;
        private readonly List<GroupItemElement> items;

        /// <summary>
        /// Initialize a new instance <see cref="ItemCollection{T}"/>
        /// </summary>
        public GroupItemCollection(GroupItemControl owner)
        {
            this.owner = owner;
            items = new List<GroupItemElement>();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<GroupItemElement> GetEnumerator() => items.GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds an element to the end of the list with specified text
        /// </summary>
        public void Add(string text) => Add(new GroupItemElement { Text = text });

        /// <summary>
        /// Adds an element to the end of the list with specified text and disabled value
        /// </summary>
        public void Add(string text, bool isDisabled) =>
            Add(new GroupItemElement { Text = text, IsDisabled = isDisabled });

        /// <inheritdoc cref="ICollection{T}.Add"/>
        public void Add(GroupItemElement item)
        {
            items.Add(item);
            owner.InvalidateMeasure();
        }

        /// <inheritdoc cref="ICollection{T}.Clear"/>
        public void Clear() => items.Clear();

        /// <inheritdoc cref="ICollection{T}.Contains"/>
        public bool Contains(GroupItemElement item) => items.Contains(item);

        /// <inheritdoc cref="ICollection{T}.CopyTo"/>
        public void CopyTo(GroupItemElement[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        /// <inheritdoc cref="ICollection{T}.Remove"/>
        public bool Remove(GroupItemElement item) => items.Remove(item);

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => items.Count;

        /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
        public bool IsReadOnly => false;

        /// <inheritdoc cref="IList{T}.IndexOf"/>
        public int IndexOf(GroupItemElement item) => items.IndexOf(item);

        /// <inheritdoc cref="IList{T}.Insert"/>
        public void Insert(int index, GroupItemElement item)
        {
            items.Insert(index, item);
            owner.InvalidateMeasure();
        }

        /// <summary>
        /// Inserts an element with specified text into the list at the specified index
        /// </summary>
        public void Insert(int index, string text)
            => Insert(index, new GroupItemElement {Text = text});

        /// <summary>
        /// Inserts an element with specified text and disabled value into the list at the specified index
        /// </summary>
        public void Insert(int index, string text, bool isDisabled)
            => Insert(index, new GroupItemElement {Text = text, IsDisabled = isDisabled});

        /// <inheritdoc cref="IList{T}.RemoveAt"/>
        public void RemoveAt(int index) => items.RemoveAt(index);

        /// <inheritdoc cref="IList{T}.this"/>
        public GroupItemElement this[int index]
        {
            get => items[index];
            set
            {
                items[index] = value;
                owner.InvalidateMeasure();
            }
        }
    }
}
