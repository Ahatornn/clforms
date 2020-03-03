using System;
using ClForms.Abstractions.Core;

namespace ClForms.Core
{
    /// <summary>
    /// Represents a configured Command Line Forms
    /// </summary>
    internal class ApplicationHandler: IApp
    {
        private readonly IEventLoop eventLoop;

        /// <summary>
        /// Initialize a new instance <see cref="ApplicationHandler"/>
        /// </summary>
        internal ApplicationHandler(IEventLoop eventLoop)
        {
            this.eventLoop = eventLoop ?? throw new ArgumentNullException(nameof(eventLoop));
        }

        /// <inheritdoc />
        public void Dispose() => (Application.ServiceProvider as IDisposable)?.Dispose();

        /// <inheritdoc cref="IApp.Start"/>
        public void Start() => throw new NotImplementedException();
    }
}
