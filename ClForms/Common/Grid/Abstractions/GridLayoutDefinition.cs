using ClForms.Elements.Abstractions;

namespace ClForms.Common.Grid.Abstractions
{
    /// <summary>
    /// Implements basic functionality that represents the appearance and behavior of a table layout
    /// </summary>
    public abstract class GridLayoutDefinition
    {
        private SizeType sizeType;
        private int size;

        /// <summary>
        /// Initialize a new instance <see cref="GridLayoutDefinition"/>
        /// </summary>
        protected GridLayoutDefinition()
        {
            sizeType = SizeType.AutoSize;
        }

        /// <summary>
        /// Gets or sets a value indicating how to resize rows or columns relative to the table containing it
        /// </summary>
        public SizeType SizeType
        {
            get => sizeType;
            set
            {
                if (sizeType != value)
                {
                    sizeType = value;
                    Owner?.InvalidateMeasure();
                }
            }
        }

        internal int Size
        {
            get => size;
            set
            {
                if (size != value)
                {
                    size = value;
                    Owner?.InvalidateMeasure();
                }
            }
        }

        internal void SetSize(int value) => size = value;

        internal MultipleContentControl Owner { get; set; }
    }
}
