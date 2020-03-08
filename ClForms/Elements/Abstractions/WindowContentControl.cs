using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Represents a window control with a single piece of content of any control
    /// </summary>
    public abstract class WindowContentControl: SingleContentControl
    {
        protected Control mainMenu;
        protected Control statusBar;

        /// <see cref="ContentControl.GetEnumerator"/>
        public override IEnumerator<Control> GetEnumerator()
            => new WindowEnumerable(mainMenu, Content, statusBar);

        /// <inheritdoc cref="ContentControl.AddContent"/>
        public override void AddContent(Control content)
        {
            /*if (content is MainMenu || content is StatusBar)
            {
                return;
            }
            */
            base.AddContent(content);
        }

        /// <inheritdoc cref="ContentControl.RemoveContent"/>
        public override void RemoveContent(Control content)
        {
            /*if (content is MainMenu || content is StatusBar)
            {
                return;
            }
            */
            base.RemoveContent(content);
        }
    }

    internal class WindowEnumerable: IEnumerator<Control>
    {
        private int currentStep;
        private readonly Control[] elements;

        internal WindowEnumerable(Control mainMenu,
            Control content,
            Control statusBar)
        {
            currentStep = -1;
            elements = new[]
            {
                mainMenu,
                content,
                statusBar,
            };
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            currentStep++;
            while (currentStep < elements.Length && elements[currentStep] == null)
            {
                currentStep++;
            }

            return currentStep < elements.Length;
        }

        /// <inheritdoc />
        public void Reset() => currentStep = -1;

        /// <inheritdoc />
        public Control Current => elements[currentStep];

        /// <inheritdoc />
        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var control in elements ?? Enumerable.Empty<Control>())
            {
                control.Dispose();
            }
        }
    }
}
