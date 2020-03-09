using System;
using System.ComponentModel;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Elements.Menu;
using ClForms.Helpers;

namespace ClForms.Elements
{
    public class Window: WindowContentControl, IElementStyle<Window>
    {
        protected Thickness BorderThickness = new Thickness(2, 1);
        private string title;
        private bool hideTitle;
        private ControlState windowState;
        private DialogResult dialogResult;

        public Window()
        {
            windowState = ControlState.Normal;
            hideTitle = false;
            title = "Window";
            Background = Application.SystemColors.WindowBackground;
            Foreground = Application.SystemColors.WindowForeground;
            WasClosed = false;
            Showing = false;
            dialogResult = DialogResult.None;
        }

        #region Properties

        #region Title

        /// <summary>
        /// Gets or sets a window's title
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    OnTitleChanged?.Invoke(this, new PropertyChangedEventArgs<string>(title, value));
                    title = value;
                    if (windowState == ControlState.Normal && !hideTitle)
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion
        #region HideTitle

        /// <summary>
        /// Gets or sets a window's title 
        /// </summary>
        public bool HideTitle
        {
            get => hideTitle;
            set
            {
                if (hideTitle != value)
                {
                    OnHideTitleChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(hideTitle, value));
                    hideTitle = value;
                    if (windowState == ControlState.Normal)
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion
        #region WindowState

        /// <summary>
        /// Gets or sets a value that indicates whether a window is minimized or maximized
        /// </summary>
        public ControlState WindowState
        {
            get => windowState;
            set
            {
                if (windowState != value)
                {
                    OnWindowStateChanged?.Invoke(this, new PropertyChangedEventArgs<ControlState>(windowState, value));
                    windowState = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region DialogResult

        /// <summary>
        /// Gets or sets the dialog result value, which is the value that is returned from the ShowDialog() method
        /// </summary>
        public DialogResult DialogResult
        {
            get => dialogResult;
            set
            {
                if (dialogResult != value)
                {
                    OnDialogResultChanged?.Invoke(this,
                        new PropertyChangedEventArgs<DialogResult>(dialogResult, value));
                    dialogResult = value;
                }
            }
        }

        #endregion
        #region MainMenu

        /// <summary>
        /// Gets or sets the main menu of window
        /// </summary>
        public MainMenu MainMenu
        {
            get => mainMenu;
            set
            {
                if (mainMenu != value)
                {
                    var shouldMeasure = mainMenu == null;
                    /*if (mainMenu?.IsOpened == true)
                    {
                        mainMenu.Close();
                    }
                    */
                    if (mainMenu?.Parent == this)
                    {
                        mainMenu.Parent = null;
                    }
                    OnMainMenuChanged?.Invoke(this, new PropertyChangedEventArgs<MainMenu>(mainMenu, value));
                    mainMenu = value;
                    mainMenu.Parent = this;
                    if (shouldMeasure)
                    {
                        InvalidateMeasure();
                    }
                    else
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion
        #region StatusBar

        /// <summary>
        /// Gets or sets the status bar of window
        /// </summary>
        public StatusBar StatusBar
        {
            get => statusBar;
            set
            {
                if (statusBar != value)
                {
                    var shouldMeasure = statusBar == null;
                    if (statusBar?.Parent == this)
                    {
                        statusBar.Parent = null;
                    }
                    OnStatusBarChanged?.Invoke(this, new PropertyChangedEventArgs<StatusBar>(statusBar, value));
                    statusBar = value;
                    statusBar.Parent = this;
                    if (shouldMeasure)
                    {
                        InvalidateMeasure();
                    }
                    else
                    {
                        InvalidateVisual();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Indicates that the Close() method has been called
        /// </summary>
        public bool WasClosed { get; private set; }

        /// <summary>
        /// Indicates that the window was showing on a screen
        /// </summary>
        public bool Showing { get; private set; }

        /// <summary>
        /// Gets focused control
        /// </summary>
        public IFocusableControl FocusableControl { get; internal set; } = null;

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            Size desiredContentSize;
            var heightAmendment = 0;
            if (WindowState == ControlState.Maximized)
            {
                desiredContentSize = new Size(availableSize.Width - Padding.Horizontal,
                    availableSize.Height - Padding.Vertical);
            }
            else
            {
                var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                    .Reduce(Margin)
                    .Reduce(BorderThickness);
                contentArea.Width = Math.Max(0, (Width ?? contentArea.Width) - Padding.Horizontal);
                contentArea.Height = Math.Max(0, (Height ?? contentArea.Height) - Padding.Vertical);
                desiredContentSize = contentArea.Size;
            }

            if (mainMenu != null)
            {
                mainMenu.Measure(desiredContentSize);
                heightAmendment += mainMenu.DesiredSize.Height;
            }

            if (statusBar != null)
            {
                statusBar.Measure(desiredContentSize);
                heightAmendment += statusBar.DesiredSize.Height;
            }

            Content?.Measure(new Size(desiredContentSize.Width,
                desiredContentSize.Height - heightAmendment));

            if (WindowState == ControlState.Maximized)
            {
                base.Measure(availableSize);
            }
            else
            {
                var size = new Size((Content?.DesiredSize ?? Size.Empty).Width,
                    ((Content?.DesiredSize ?? Size.Empty) +
                     (mainMenu?.DesiredSize ?? Size.Empty) +
                     (statusBar?.DesiredSize ?? Size.Empty))
                    .Height);

                size.Width = Math.Min(Width.HasValue
                        ? Width.Value + (Margin + BorderThickness).Horizontal
                        : size.Width + (Margin + Padding + BorderThickness).Horizontal,
                    availableSize.Width);
                size.Height = Math.Min(Height.HasValue
                        ? Height.Value + (Margin + BorderThickness).Vertical
                        : size.Height + (Margin + Padding + BorderThickness).Vertical,
                    availableSize.Height);

                base.Measure(size);
            }
        }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            var contentRect = new Rect(0, 0, finalRect.Width, finalRect.Height)
                .Reduce(Margin)
                .Reduce(Padding);
            if (WindowState != ControlState.Maximized)
            {
                contentRect = contentRect.Reduce(BorderThickness);
                mainMenu?.Arrange(new Rect(BorderThickness.Left, BorderThickness.Top, finalRect.Width - BorderThickness.Horizontal, mainMenu.DesiredSize.Height));
                statusBar?.Arrange(new Rect(BorderThickness.Left, finalRect.Height - BorderThickness.Vertical - statusBar.DesiredSize.Height + 1, finalRect.Width - BorderThickness.Horizontal, statusBar.DesiredSize.Height));
            }
            else
            {
                mainMenu?.Arrange(new Rect(0, 0, finalRect.Width, mainMenu.DesiredSize.Height));
                statusBar?.Arrange(new Rect(0, finalRect.Height - statusBar.DesiredSize.Height, finalRect.Width, statusBar.DesiredSize.Height));
            }

            Content?.Arrange(new Rect(contentRect.Left,
                contentRect.Top + (mainMenu?.DesiredSize.Height ?? 0),
                Math.Min(contentRect.Width, Content.DesiredSize.Width),
                Math.Min(contentRect.Height - (mainMenu?.DesiredSize.Height ?? 0) - (statusBar?.DesiredSize.Height ?? 0),
                    Content.DesiredSize.Height)));

            base.Arrange(finalRect, WindowState != ControlState.Maximized);
        }

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            var borderColor = Application.SystemColors.BorderColor;
            if (WindowState != ControlState.Maximized && !context.ContextBounds.HasEmptyDimension())
            {
                if (BorderThickness.Top > 0)
                {
                    var topString = $"{Application.Environment.BorderChars.TopLeft}{Application.Environment.BorderChars.TopMiddle}" +
                                       (HideTitle || context.ContextBounds.Height - BorderThickness.Vertical <= 0
                                           ? string.Empty
                                           : Title);
                    if (context.ContextBounds.Width - BorderThickness.Horizontal <= 0)
                    {
                        topString = $"{Application.Environment.BorderChars.TopLeft}{Application.Environment.BorderChars.TopRight}";
                    }
                    else
                    {
                        topString = topString.Length > context.ContextBounds.Width - BorderThickness.Horizontal - 2
                            ? topString.Substring(0, context.ContextBounds.Width - BorderThickness.Horizontal - 1) + '…'
                            : topString += new string(Application.Environment.BorderChars.TopMiddle,
                                context.ContextBounds.Width - BorderThickness.Horizontal - topString.Length);
                        topString = $"{topString}{Application.Environment.BorderChars.TopMiddle}{Application.Environment.BorderChars.TopRight}";
                    }
                    context.SetCursorPos(BorderThickness.Left - 1, BorderThickness.Top - 1);
                    context.DrawText(topString);
                }

                for (var row = BorderThickness.Top; row < context.ContextBounds.Height - BorderThickness.Vertical + 2; row++)
                {
                    if (BorderThickness.Left > 0)
                    {
                        context.SetCursorPos(BorderThickness.Left - 1, row);
                        context.DrawText(Application.Environment.BorderChars.MiddleLeft);
                    }

                    if (BorderThickness.Right > 0)
                    {
                        context.SetCursorPos(context.ContextBounds.Width - BorderThickness.Right, row);
                        context.DrawText(Application.Environment.BorderChars.MiddleRight);
                    }
                }

                if (BorderThickness.Bottom > 0)
                {
                    var bottomLine = context.ContextBounds.Width - BorderThickness.Horizontal - 2 > 0
                        ? new string(Application.Environment.BorderChars.BottomMiddle,
                            context.ContextBounds.Width - BorderThickness.Horizontal)
                        : string.Empty;
                    var topString =
                        $"{Application.Environment.BorderChars.BottomLeft}{bottomLine}{Application.Environment.BorderChars.BottomRight}";
                    context.SetCursorPos(BorderThickness.Left - 1, context.ContextBounds.Height - BorderThickness.Vertical + 1);
                    context.DrawText(topString);
                }
            }
        }

        /// <summary>
        /// Opens a window and returns without waiting for the newly opened window to close
        /// </summary>
        public void Show()
        {
            if (Showing)
            {
                throw new InvalidOperationException($"The window '{Title}' is already showing");
            }
            WasClosed = false;
            //Application.ShowWindow(this);
            Showing = true;
            //SetFocusableControl(Direction.Forward);
            OnActivated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed
        /// </summary>
        public DialogResult ShowDialog()
        {
            Show();
            using (_ = new WindowDialogWrapper(this))
            {
                while (DialogResult == DialogResult.None)
                {
                    Application.DoEvents();
                }
            }
            Close();
            return DialogResult;
        }

        /// <summary>
        /// Manually closes a <see cref="Window"/>
        /// </summary>
        public virtual void Close()
        {
            var args = new CancelEventArgs();
            OnClosing?.Invoke(this, args);
            if (args.Cancel)
            {
                return;
            }
            WasClosed = true;
            Showing = false;
            //Application.CloseWindow();
            OnClosed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Manually handles keystrokes
        /// </summary>
        public void InputAction(ConsoleKeyInfo keyInfo) => InputActionInternal(keyInfo);

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Window> styleAction) => styleAction?.Invoke(this);

        /// <summary>
        /// Trying to shift input focus to component
        /// </summary>
        internal bool TrySetFocus(IFocusableControl control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (FocusableControl?.Id == control.Id || !control.CanFocus())
            {
                return false;
            }

            if (FocusableControl != null)
            {
                FocusableControl.IsFocus = false;
            }
            FocusableControl = control;
            control.IsFocus = true;
            return true;
        }

        protected virtual void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.F4 && (keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
            {
                Close();
            }
            /*
            if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0 &&
                keyInfo.Key == ConsoleKey.F1 &&
                mainMenu != null &&
                !mainMenu.IsOpened)
            {
                mainMenu.Open();
            }

            if (mainMenu?.IsOpened == true)
            {
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    mainMenu.Close();
                }
                else
                {
                    mainMenu.InputAction(keyInfo);
                }

                return;
            }

            if (mainMenu?.FindAndClick(keyInfo) == true)
            {
                return;
            }

            if (statusBar?.FindAndClick(keyInfo) == true)
            {
                return;
            }*/

            if (keyInfo.Key == ConsoleKey.Tab)
            {
                FocusableHelper.SetFocusableControl(this, (keyInfo.Modifiers & ConsoleModifiers.Shift) != 0
                    ? Direction.Backward
                    : Direction.Forward);
            }
            
            FocusableControl?.InputAction(keyInfo);
            OnKeyPressed?.Invoke(this, new KeyPressedEventArgs(keyInfo));
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Title" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnTitleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HideTitle" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnHideTitleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="WindowState" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<ControlState>> OnWindowStateChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="DialogResult" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<DialogResult>> OnDialogResultChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MainMenu" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<MainMenu>> OnMainMenuChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="StatusBar" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<StatusBar>> OnStatusBarChanged;

        /// <summary>
        /// Occurs when the window try closing
        /// </summary>
        public event EventHandler<CancelEventArgs> OnClosing;

        /// <summary>
        /// Occurs when the window was close
        /// </summary>
        public event EventHandler OnClosed;

        /// <summary>
        /// Occurs when the window showing on screen or after setting active window
        /// </summary>
        public event EventHandler OnActivated;

        /// <summary>
        /// Occurs when the key was pressed
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> OnKeyPressed;

        #endregion
    }
}
