using System;
using System.Collections;
using System.Collections.Generic;
using ClForms.Common;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Represents a control with a single piece of content of any control
    /// </summary>
    public abstract class SingleContentControl: ContentControl
    {
        /// <summary>
        /// Child control
        /// </summary>
        public Control Content { get; private set; }

        /// <inheritdoc />
        public override void AddContent(Control content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Content == content)
            {
                return;
            }

            if (Content != null)
            {
                Content.Parent = null;
            }

            Content = content;
            content.Parent = this;
            InvalidateMeasure();
        }

        /// <inheritdoc />
        public override void RemoveContent(Control content)
        {
            if (content.Parent == null || Content == null)
            {
                return;
            }

            if (Content != content)
            {
                throw new InvalidOperationException($"Can't remove {content} from this. This control isn't owner for specified content");
            }

            Content = null;
            content.Parent = null;
            InvalidateMeasure();
        }

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            if (Content?.IsMeasureValid != true)
            {
                Content?.Measure(DesiredSize);
            }
            base.Measure(availableSize);
        }

        /// <see cref="ContentControl.GetEnumerator"/>
        public override IEnumerator<Control> GetEnumerator() => new SingleEnumerable(Content);
    }

    internal class SingleEnumerable: IEnumerator<Control>
    {
        private bool wasReset;
        public SingleEnumerable(Control currentElement)
        {
            Current = currentElement;
            wasReset = true;
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (wasReset && Current != null)
            {
                wasReset = false;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public void Reset() => wasReset = true;

        /// <inheritdoc />
        public Control Current { get; }

        /// <inheritdoc />
        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose() => Current?.Dispose();
    }
}
