using System;
using System.Collections;
using System.Collections.Generic;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Represents a collection of any objects for <see cref="ListBox"/>
    /// </summary>
    public abstract class ListBoxCollection<T>: IList<T>, IList
    {
        protected readonly List<T> Items;

        protected ListBoxCollection()
        {
            Items = new List<T>();
        }

        /// <inheritdoc cref="IList{T}.GetEnumerator"/>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc cref="IList.GetEnumerator"/>
        public IEnumerator GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc cref="IList.CopyTo"/>
        public void CopyTo(Array array, int index)
        {
            for (var virtualIndex = 0; virtualIndex < Items.Count; ++virtualIndex)
            {
                array.SetValue(Items[virtualIndex], virtualIndex + index);
            }
        }

        /// <inheritdoc cref="IList{T}.CopyTo"/>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => CopyTo(array, arrayIndex);

        /// <inheritdoc cref="IList.Add"/>
        public int Add(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            BeginUpdateInterceptor();
            Items.Add(value);
            EndUpdateInterceptor();
            return Items.Count - 1;
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list
        /// </summary>
        public void AddRange(IEnumerable<T> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            BeginUpdateInterceptor();
            Items.AddRange(value);
            EndUpdateInterceptor();
        }

        /// <inheritdoc cref="IList{T}.Add"/>
        void ICollection<T>.Add(T item) => Add(item);

        /// <inheritdoc cref="IList{T}.Add"/>
        public int Add(object value) => Add((T) value);

        /// <inheritdoc cref="IList.Clear"/>
        public void Clear() => ClearInternal();

        public bool Contains(object value) => Contains((T) value);

        public int IndexOf(object value) => IndexOf((T) value);

        public void Insert(int index, object value) => Insert(index, (T) value);

        public void Remove(object value) => Remove((T) value);

        protected virtual void ClearInternal()
        {
            BeginUpdateInterceptor();
            Items.Clear();
            EndUpdateInterceptor();
        }

        /// <inheritdoc cref="IList.Contains"/>
        public bool Contains(T value) => IndexOf(value) != -1;

        /// <inheritdoc cref="IList.IndexOf"/>
        public int IndexOf(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Items.IndexOf(value, 0);
        }

        /// <inheritdoc cref="IList.Insert"/>
        public void Insert(int index, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            BeginUpdateInterceptor();
            Items.Insert(index, value);
            EndUpdateInterceptor();
        }

        /// <inheritdoc cref="IList.Remove"/>
        public void Remove(T value)
        {
            var index = Items.IndexOf(value, 0);
            if (index == -1)
            {
                return;
            }

            RemoveAt(index);
        }

        /// <inheritdoc cref="IList{T}.Remove"/>
        bool ICollection<T>.Remove(T item)
        {
            var index = Items.IndexOf(item, 0);
            if (index == -1)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        /// <inheritdoc cref="IList.RemoveAt"/>
        public void RemoveAt(int index) => RemoveAtInternal(index);

        protected virtual void RemoveAtInternal(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            BeginUpdateInterceptor();
            Items.RemoveAt(index);
            EndUpdateInterceptor();
        }

        /// <inheritdoc cref="IList.Count"/>
        public int Count => Items.Count;

        /// <inheritdoc cref="IList.IsSynchronized"/>
        public bool IsSynchronized => false;

        /// <inheritdoc cref="IList.SyncRoot"/>
        public object SyncRoot => this;

        /// <inheritdoc cref="IList.IsFixedSize"/>
        public bool IsFixedSize => false;

        /// <inheritdoc cref="IList.IsReadOnly"/>
        public bool IsReadOnly => false;

        /// <inheritdoc cref="IList.this"/>
        object IList.this[int index]
        {
            get => Items[index];
            set
            {
                if (!Items[index].Equals(value))
                {
                    BeginUpdateInterceptor();
                    Items[index] = (T) value;
                    EndUpdateInterceptor();
                }
            }
        }

        /// <inheritdoc cref="IList.this"/>
        public T this[int index]
        {
            get => Items[index];
            set
            {
                if (!Items[index].Equals(value))
                {
                    BeginUpdateInterceptor();
                    Items[index] = value;
                    EndUpdateInterceptor();
                }
            }
        }

        protected abstract void BeginUpdateInterceptor();

        protected abstract void EndUpdateInterceptor();
    }
}
