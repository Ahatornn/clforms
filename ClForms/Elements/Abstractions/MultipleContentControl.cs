using System;
using System.Collections.Generic;
using System.Linq;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Represents a control with a multiple pieces of content of any control
    /// </summary>
    public abstract class MultipleContentControl: ContentControl
    {
        private readonly IList<Control> contents;

        /// <summary>
        /// Initialize a new instance <see cref="MultipleContentControl"/>
        /// </summary>
        protected MultipleContentControl()
        {
            contents = new List<Control>();
        }

        /// <inheritdoc cref="ContentControl.GetEnumerator"/>
        public override IEnumerator<Control> GetEnumerator() => contents.GetEnumerator();

        /// <inheritdoc cref="ContentControl.AddContent"/>
        public override void AddContent(Control content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (contents.Contains(content))
            {
                return;
            }

            contents.Add(content);
            AddContentInterceptor(content);
            content.Parent = this;
            InvalidateMeasure();
        }

        /// <inheritdoc cref="ContentControl.RemoveContent"/>
        public override void RemoveContent(Control content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (content.Parent == null)
            {
                return;
            }

            if (!contents.Contains(content))
            {
                throw new InvalidOperationException($"Can't remove {content} from this. This control isn't owner for specified content");
            }

            contents.Remove(content);
            RemoveContentInterceptor(content);
            content.Parent = null;
            InvalidateMeasure();
        }

        /// <summary>
        /// Gets the number of elements contained in the current control
        /// </summary>
        public int ContentCount => contents.Count;

        /// <summary>
        /// Interceptor of an action of the add content
        /// </summary>
        protected virtual void AddContentInterceptor(Control content) { }

        /// <summary>
        /// Interceptor of an action of the remove content
        /// </summary>
        protected virtual void RemoveContentInterceptor(Control content) { }

        /// <summary>
        /// Retrieves all components of a specific type
        /// </summary>
        protected IEnumerable<T> GetAllControls<T>() => this.Where(x => x is T).Cast<T>();
    }
}
