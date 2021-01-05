using ClForms.Common.Grid.Abstractions;
using ClForms.Elements.Abstractions;

namespace ClForms.Common.Grid
{
    /// <summary>
    /// Collection of <see cref="ColumnDefinition"/>
    /// </summary>
    public class GridColumnDefinitionCollection: GridLayoutDefinitionCollection<ColumnDefinition>
    {
        /// <summary>
        /// Initialize a new instance <see cref="GridColumnDefinitionCollection"/>
        /// </summary>
        internal GridColumnDefinitionCollection(MultipleContentControl owner) : base(owner)
        {
        }

        /// <summary>
        /// Adds an auto size column to collection
        /// </summary>
        public void AddColumn() => Add(new ColumnDefinition(SizeType.AutoSize));
    }
}
