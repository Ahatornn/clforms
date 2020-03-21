using ClForms.Common.Grid.Abstractions;

namespace ClForms.Common.Grid
{
    /// <summary>
    /// Defines row-specific properties that apply to <see cref="Grid"/> elements
    /// </summary>
    public class RowDefinition: GridLayoutDefinition
    {
        /// <summary>
        /// Initialize a new instance <see cref="RowDefinition"/>
        /// </summary>
        public RowDefinition()
            : this(SizeType.AutoSize)
        {
        }

        /// <summary>
        /// Initialize a new instance <see cref="RowDefinition"/> with <see cref="SizeType"/>
        /// </summary>
        public RowDefinition(SizeType sizeType)
            : this(sizeType, 0)
        {
        }

        /// <summary>
        /// Initialize a new instance <see cref="RowDefinition"/> with <see cref="SizeType"/> and height value
        /// </summary>
        public RowDefinition(SizeType sizeType, int height)
        {
            SizeType = sizeType;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the height value of the row
        /// </summary>
        public int Height
        {
            get => Size;
            set => Size = value;
        }
    }
}
