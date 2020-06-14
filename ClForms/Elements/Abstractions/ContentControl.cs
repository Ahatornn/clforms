using System.Collections;
using System.Collections.Generic;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Base class for parent controls
    /// </summary>
    public abstract class ContentControl: Control, IEnumerable<Control>
    {
        /// <summary>
        /// Adds the specified control to the control collection
        /// </summary>
        public abstract void AddContent(Control content);

        /// <summary>
        /// Remove the specified control to the control collection
        /// </summary>
        public abstract void RemoveContent(Control content);

        /// <inheritdoc cref="IEnumerator{UIElement}"/>
        public abstract IEnumerator<Control> GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
