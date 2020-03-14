using ClForms.Elements;
using System;

namespace ClForms.Abstractions.Core
{
    /// <summary>
    /// Represents a configured application
    /// </summary>
    public interface IApp: IDisposable
    {
        /// <summary>
        /// Starts listening on the configured application
        /// </summary>
        void Start(Window mainWindow);

        /// <summary>
        /// Processes all Windows messages currently in the message queue
        /// </summary>
        void DoEvents();
    }
}
