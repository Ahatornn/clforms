using System.Collections.Generic;
using System.Linq;

namespace ClForms.Elements.Abstractions
{
    public class Chunks<T>
    {
        private readonly List<List<T>> chunks;

        internal Chunks()
        {
            chunks = new List<List<T>>();
        }

        internal void Clear() => chunks.Clear();

        internal void Add(IEnumerable<T> items) => chunks.Add(items.ToList());

        internal int Count => chunks.Count;

        internal IList<T> AllList => chunks.SelectMany(x => x).ToList();

        internal IList<T> this[int index]
        {
            get => chunks[index];
            set => chunks[index] = value.ToList();
        }
    }
}
