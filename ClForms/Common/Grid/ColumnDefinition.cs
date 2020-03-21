using ClForms.Common.Grid.Abstractions;

namespace ClForms.Common.Grid
{
    /// <summary>
    /// Defines column-specific properties that apply to <see cref="Grid"/> elements
    /// </summary>
    public class ColumnDefinition: GridLayoutDefinition
    {
        /// <summary>
        /// Initialize a new instance <see cref="ColumnDefinition"/>
        /// </summary>
        public ColumnDefinition()
            : this(SizeType.AutoSize)
        {
        }

        /// <summary>
        /// Initialize a new instance <see cref="ColumnDefinition"/> with <see cref="SizeType"/>
        /// </summary>
        public ColumnDefinition(SizeType sizeType)
            : this(sizeType, 0)
        {
        }

        /// <summary>
        /// Initialize a new instance <see cref="ColumnDefinition"/> with <see cref="SizeType"/> and width value
        /// </summary>
        public ColumnDefinition(SizeType sizeType, int width)
        {
            SizeType = sizeType;
            Width = width;
        }

        /// <summary>
        /// Gets or sets the width value of the column
        /// </summary>
        public int Width
        {
            get => Size;
            set => Size = value;
        }
    }
}
