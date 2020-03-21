using System;
using ClForms.Abstractions.Core;

namespace ClForms.Loader
{
    /// <inheritdoc cref="IControlLifeCycle"/>
    internal class ControlLifeCycle: IControlLifeCycle
    {
        private long currentControlId;

        /// <summary>
        /// Initialize a new instance <see cref="ControlLifeCycle"/>
        /// </summary>
        public ControlLifeCycle()
        {
            currentControlId = DateTime.UtcNow.Ticks;
        }

        /// <inheritdoc cref="IControlLifeCycle.GetId"/>
        public long GetId() => currentControlId++;
    }
}
