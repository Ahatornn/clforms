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

        /// <summary>
        /// Informs all message pumps that they must terminate, and then closes all application windows
        /// </summary>
        void Terminate();

        /// <summary>
        /// Show specified window on the screen
        /// </summary>
        void ShowWindow(Window wnd);

        /// <summary>
        /// Close current active window
        /// </summary>
        bool CloseWindow();
    }
}
