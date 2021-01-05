using ClForms.Common.Grid.Abstractions;
using ClForms.Elements.Abstractions;

namespace ClForms.Common.Grid
{
    /// <summary>
    /// Collection of <see cref="RowDefinition"/>
    /// </summary>
    public class GridRowDefinitionCollection: GridLayoutDefinitionCollection<RowDefinition>
    {
        /// <summary>
        /// Initialize a new instance <see cref="GridRowDefinitionCollection"/>
        /// </summary>
        internal GridRowDefinitionCollection(MultipleContentControl owner) : base(owner)
        {
        }

        /// <summary>
        /// Adds an auto size row to collection
        /// </summary>
        public void AddRow() => Add(new RowDefinition(SizeType.AutoSize));
    }
}
