using System;
using System.Text;
using ClForms.Abstractions;
using ClForms.Abstractions.Core;
using ClForms.Abstractions.Engine;
using ClForms.Common;
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
        private const int DoEventsAdditionalSteps = 4;
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
            this.eventLoop.OnLoopEmpty += OnLoopEmptyHandler;
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

        /// <inheritdoc cref="IApp.DoEvents"/>
        public void DoEvents()
        {
            var maxStepCount = eventLoop.Length + DoEventsAdditionalSteps;
            eventLoop.OnLoopEmpty -= OnLoopEmptyHandler;
            eventLoop.Enqueue(() => OnLoopEmptyHandler(eventLoop, EventArgs.Empty));
            var iteration = 0;
            while (eventLoop.Length > 0 && iteration < maxStepCount)
            {
                eventLoop.ProcessIteration();
                iteration++;
            }
            eventLoop.OnLoopEmpty += OnLoopEmptyHandler;
        }

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

        private void OnLoopEmptyHandler(object sender, EventArgs e)
        {
            if (CloseWindow())
            {
                return;
            }
        }

        internal bool CloseWindow()
        {
            var wasClosed = false;
            while (CloseWindowInternal())
            {
                wasClosed = true;
            }

            if (wasClosed)
            {
                if (currentWindowParams?.Context != null)
                {
                    InvalidateScreen(currentWindowParams.Context);
                    CheckMeasureOrVisualInvalidate(currentWindowParams);
                }
                return true;
            }

            return false;
        }

        private bool CloseWindowInternal()
        {
            if (currentWindowParams?.Window != null && currentWindowParams.Window.WasClosed)
            {
                Windows.TryPop(out _);
                if (!Windows.TryPeek(out currentWindowParams))
                {
                    eventLoop.Stop();
                }

                return true;
            }

            return false;
        }

        private void ClearScreen()
        {
            if (systemColors.ScreenBackground != Color.NotSet)
            {
                pseudographicsProvider.BackgroundColor = systemColors.ScreenBackground;
            }
            Console.Clear();
        }

        /// <summary>
        /// Redrawing the screen to the same state as another window was displayed on it
        /// </summary>
        private void InvalidateScreen(IDrawingContext ctx)
        {
            var strBuilder = new StringBuilder(ctx.ContextBounds.Width);
            var colorPoint = ctx.GetColorPoint(0, 0);
            SetConsoleColor(colorPoint);
            for (var row = 0; row < ctx.ContextBounds.Height; row++)
            {
                strBuilder.Clear();

                Console.SetCursorPosition(0, row);
                for (var col = 0; col < ctx.ContextBounds.Width; col++)
                {
                    var currentPoint = ctx.GetColorPoint(col, row);
                    if (currentPoint == colorPoint)
                    {
                        strBuilder.Append(ctx.Chars[col, row]);
                    }
                    else
                    {
                        if (strBuilder.Length > 0)
                        {
                            pseudographicsProvider.Write(strBuilder.ToString());
                            strBuilder.Clear();
                        }
                        SetConsoleColor(currentPoint);
                        colorPoint = currentPoint;
                        strBuilder.Append(ctx.Chars[col, row]);
                    }
                }

                if (strBuilder.Length > 0)
                {
                    pseudographicsProvider.Write(strBuilder.ToString());
                }
            }
            pseudographicsProvider.SetCursorPosition(0, 0);
        }

        private void SetConsoleColor(ContextColorPoint colorPoint)
        {
            pseudographicsProvider.BackgroundColor = colorPoint.Background;
            pseudographicsProvider.ForegroundColor = colorPoint.Foreground;
        }

        private void CheckMeasureOrVisualInvalidate(WindowParameters wndParams)
        {

        }
    }
}
