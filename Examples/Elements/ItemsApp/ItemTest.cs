using System;

namespace ItemsApp
{
    internal class ItemTest
    {
        public DateTime Value { get; }

        internal ItemTest()
        {
            Value = DateTime.Now;
        }

        public override string ToString() => $"{Value.ToShortDateString()} {Value.ToLongTimeString()}";

        public override int GetHashCode() => Value.GetHashCode();
    }
}
