using ClForms.Abstractions;
using ClForms.Abstractions.Core;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core.Contexts;
using ClForms.Core.Models;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Helpers;
using ClForms.Themes;
using System;
using System.Linq;
using System.Text;

namespace ClForms.Core
{
    /// <summary>
    /// Represents a configured Command Line Forms
    /// </summary>
    internal partial class ApplicationHandler: IApp
    {
        private const int DoEventsAdditionalSteps = 4;
        private readonly IEventLoop eventLoop;
        private readonly IPseudographicsProvider pseudographicsProvider;
        private readonly ISystemColors systemColors;
        private readonly WindowCollection windows;
        private readonly IEnvironment environment;
        private WindowParameters currentWindowParams;
        private Rect screenRect;
        private ScreenDrawingContext screenContext;

        /// <summary>
        /// Initialize a new instance <see cref="ApplicationHandler"/>
        /// </summary>
        public ApplicationHandler(IEventLoop eventLoop,
            IPseudographicsProvider pseudographicsProvider,
            ISystemColors systemColors,
            IEnvironment environment)
        {
            this.eventLoop = eventLoop ?? throw new ArgumentNullException(nameof(eventLoop));
            this.pseudographicsProvider = pseudographicsProvider
                ?? throw new ArgumentNullException(nameof(pseudographicsProvider));
            this.systemColors = systemColors ?? throw new ArgumentNullException(nameof(systemColors));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.eventLoop.OnLoopEmpty += OnLoopEmptyHandler;
            windows = new WindowCollection();
            screenRect = new Rect(0, 0, this.environment.WindowWidth, this.environment.WindowHeight);
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
            Application.MainWindow = mainWindow;
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

        /// <inheritdoc cref="IApp.Terminate"/>
        public void Terminate() => eventLoop.Stop();

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
            var wndParams = windows.GetWindowParameters(wnd);
            wndParams.Context.Release(systemColors.ScreenBackground, systemColors.ScreenForeground);
            pseudographicsProvider.CursorVisible = false;
        }

        private void OnLoopEmptyHandler(object sender, EventArgs e)
        {
            if (CloseWindow())
            {
                return;
            }

            if (screenRect.Width != environment.WindowWidth ||
                screenRect.Height != environment.WindowHeight)
            {
                screenRect = new Rect(0, 0, environment.WindowWidth, environment.WindowHeight);
                ClearScreen();
                IDrawingContext preWndContext = null;
                foreach (var wndPr in windows.Reverse())
                {
                    var wndContext = preWndContext != null
                        ? preWndContext.Clone(wndPr.Window.Id, Guid.Empty)
                        : new DefaultDrawingContext(screenRect, wndPr.Window.Id, GetHashCodeHelper.CalculateHashCode(wndPr.Window), Guid.Empty, null);
                    preWndContext = wndContext;
                    wndPr.SetContext(wndContext);
                    wndPr.ControlContextHash.Clear();
                    PrepareWindow(wndPr.Window);

                    var bufferForRender = new ScreenDrawingContext(screenRect);
                    bufferForRender.Release(Color.NotSet, Color.NotSet);

                    ReleaseDrawingContext(wndPr, bufferForRender);

                    TransferToScreen(bufferForRender);
                }
            }
            CheckMeasureOrVisualInvalidate(currentWindowParams);
        }

        /// <inheritdoc cref="IApp.ShowWindow"/>
        public void ShowWindow(Window wnd)
        {
            if (!(wnd.WasClosed && wnd.Showing))
            {
                var wndParams = currentWindowParams != null
                    ? new WindowParameters(wnd, currentWindowParams.Context.Clone(wnd.Id, Guid.Empty))
                    : new WindowParameters(wnd, new DefaultDrawingContext(screenRect, wnd.Id, GetHashCodeHelper.CalculateHashCode(wnd), Guid.Empty, null));
                windows.Push(wndParams);
                currentWindowParams = wndParams;
                PrepareWindow(currentWindowParams.Window);
            }
        }

        /// <inheritdoc cref="IApp.CloseWindow"/>
        public bool CloseWindow()
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
                windows.TryPop(out _);
                if (!windows.TryPeek(out currentWindowParams))
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
            pseudographicsProvider.Clear();
            screenContext = new ScreenDrawingContext(screenRect);
            screenContext.Release(systemColors.ScreenBackground, systemColors.ScreenForeground);
        }

        /// <summary>
        /// Redrawing the screen to the same state as another window was displayed on it
        /// </summary>
        private void InvalidateScreen(IDrawingContext ctx)
        {
            /*var strBuilder = new StringBuilder(ctx.ContextBounds.Width);
            var colorPoint = ctx.GetColorPoint(0, 0);
            SetConsoleColor(colorPoint);
            for (var row = 0; row < ctx.ContextBounds.Height; row++)
            {
                strBuilder.Clear();

                pseudographicsProvider.SetCursorPosition(0, row);
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
            pseudographicsProvider.SetCursorPosition(0, 0);*/
        }

        /// <summary>
        /// Preparing the window for display on the screen
        /// </summary>
        private void PrepareWindow(Window wnd)
        {
            wnd.Measure(new Size(environment.WindowWidth, environment.WindowHeight));
            if (wnd.WindowState == ControlState.Maximized)
            {
                wnd.Arrange(screenRect);
            }
            else
            {
                var location = new Point((environment.WindowWidth - wnd.DesiredSize.Width) / 2,
                    (environment.WindowHeight - wnd.DesiredSize.Height) / 2);

                if (wnd is PopupMenuWindow popWnd)
                {
                    location = popWnd.PreferredLocation;
                }

                wnd.Arrange(new Rect(location.X, location.Y, wnd.DesiredSize.Width, wnd.DesiredSize.Height));
            }
        }

        private Point GetFocusableCursorPosition(long controlId, Point cursorPosition)
        {
            currentWindowParams.ControlContextHash.TryGetValue(controlId, out var control);
            return control == null
                ? cursorPosition
                : control.Location + cursorPosition;
        }
    }
}
