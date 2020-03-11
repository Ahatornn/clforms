using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Core;
using ClForms.Core.Models;
using ClForms.Elements;
using ClForms.Themes;

namespace ClForms.Core
{
    /// <summary>
    /// Represents a configured Command Line Forms
    /// </summary>
    internal class ApplicationHandler: IApp
    {
        private readonly IEventLoop eventLoop;
        private readonly IPseudographicsProvider pseudographicsProvider;
        private readonly ISystemColors systemColors;
        private WindowParameters currentWindowParams;
        private readonly WindowCollection Windows;

        /// <summary>
        /// Initialize a new instance <see cref="ApplicationHandler"/>
        /// </summary>
        internal ApplicationHandler(IEventLoop eventLoop,
            IPseudographicsProvider pseudographicsProvider,
            ISystemColors systemColors)
        {
            this.eventLoop = eventLoop ?? throw new ArgumentNullException(nameof(eventLoop));
            this.pseudographicsProvider = pseudographicsProvider
                ?? throw new ArgumentNullException(nameof(pseudographicsProvider));
            this.systemColors = systemColors ?? throw new ArgumentNullException();

            Windows = new WindowCollection();
        }

        /// <inheritdoc />
        public void Dispose() => (Application.ServiceProvider as IDisposable)?.Dispose();

        /// <inheritdoc cref="IApp.Start"/>
        public void Start(Window mainWindow)
        {
            if (eventLoop.Running)
            {
                throw new InvalidOperationException("The main message loop is already running in this thread");
            }

            if (string.IsNullOrWhiteSpace(applicationName))
            {
                ApplicationName = mainWindow.Title;
            }
            mainWindow.Show();
            PrepareInvalidateScreen(mainWindow);
            eventLoop.Start(InputAction);
        }

        #region ApplicationName

        private string applicationName;
        public string ApplicationName
        {
            get => applicationName;
            set
            {
                if (applicationName != value)
                {
                    pseudographicsProvider.Title = value;
                    applicationName = value;
                }
            }
        }

        #endregion

        private void InputAction(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.R &&
                (keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                (keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
            {
                currentWindowParams?.Window.Close();
            }
            currentWindowParams?.Window.InputAction(keyInfo);
        }

        private void PrepareInvalidateScreen(Window wnd)
        {
            ClearScreen();
            currentWindowParams.ControlContextHash.Clear();
            var wndParams = Windows.GetWindowParameters(wnd);
            wndParams.Context.Release(systemColors.ScreenBackground, systemColors.ScreenForeground);
            Console.CursorVisible = false;
        }

        private void ClearScreen()
        {
            if (systemColors.ScreenBackground != Color.NotSet)
            {
                pseudographicsProvider.BackgroundColor = systemColors.ScreenBackground;
            }
            Console.Clear();
        }
    }
}
