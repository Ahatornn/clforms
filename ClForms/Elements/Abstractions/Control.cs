using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Core;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Themes;
using Microsoft.Extensions.DependencyInjection;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// This is the base class for kernel level implementations that are based on
    /// the CommandLine Forms elements and basic presentation characteristics.
    /// </summary>
    public abstract class Control: IElementStyle<Control>, IDisposable
    {
        private Thickness margin;
        private Thickness padding;
        private Color background;
        private Color foreground;
        private Rect bounds;
        private int? width;
        private int? height;
        private bool autoSize;
        private object tag;
        private ContentControl parent;

        /// <summary>
        /// Initialize a new instance <see cref="Control"/>
        /// </summary>
        protected Control()
        {
            Id = Guid.NewGuid();
            DrawingContext = DefaultDrawingContext.Empty;
            bounds = Rect.Empty;
            background = foreground = Color.NotSet;
            padding = Thickness.Empty;
            margin = Thickness.Empty;
            IsMeasureValid = false;
            IsVisualValid = false;
            autoSize = true;
        }

        #region Properties

        /// <summary>
        /// Gets a value of the control's identifier
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets a value indicating whether component sizing was performed
        /// </summary>
        public bool IsMeasureValid { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the component is being re-rendered
        /// </summary>
        public bool IsVisualValid { get; private set; }

        /// <summary>
        /// Gets the size that this element computed during the measure pass of the
        /// layout process
        /// </summary>
        public Size DesiredSize { get; private set; }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the control relative to
        /// the upper-left corner of its container
        /// </summary>
        public Point Location => bounds.Location;

        /// <summary>
        /// Gets the size and location of the control including its nonclient elements,
        /// in points, relative to the parent control
        /// </summary>
        public Rect Bounds => bounds;

        /// <summary>
        /// Gets a value of the drawing context
        /// </summary>
        public IDrawingContext DrawingContext { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Background"/> has <see cref="Color.NotSet"/>
        /// </summary>
        public bool BackgroundIsTransparent => background == Color.NotSet;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Foreground"/> has <see cref="Color.NotSet"/>
        /// </summary>
        public bool ForegroundIsTransparent => foreground == Color.NotSet;

        #region Parent

        /// <summary>
        /// Gets or sets the parent container of the control
        /// </summary>
        public ContentControl Parent
        {
            get => parent;
            set
            {
                if (parent != value)
                {
                    OnParentChanged?.Invoke(this, new PropertyChangedEventArgs<ContentControl>(parent, value));
                    if (value != null)
                    {
                        value.AddContent(this);
                        parent = value;
                        //TODO необходимо добавить возможность читать стили компонента от родителя
                    }
                    else
                    {
                        var oldParent = parent;
                        parent = null;
                        oldParent.RemoveContent(this);
                    }
                }
            }
        }

        #endregion
        #region Padding

        /// <summary>
        /// Gets or sets a <see cref="Thickness"/> value that describes the amount of space between a control and its child element
        /// </summary>
        public Thickness Padding
        {
            get => padding;
            set
            {
                if (padding != value)
                {
                    OnPaddingChanged?.Invoke(this, new PropertyChangedEventArgs<Thickness>(padding, value));
                    padding = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region Margin

        /// <summary>
        /// Gets or sets the outer margin of an element
        /// </summary>
        public Thickness Margin
        {
            get => margin;
            set
            {
                if (margin != value)
                {
                    OnMarginChanged?.Invoke(this, new PropertyChangedEventArgs<Thickness>(margin, value));
                    margin = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region Background

        /// <summary>
        /// Gets or sets a brush that describes the background of a control
        /// </summary>
        public Color Background
        {
            get
            {
                if (background != Color.NotSet)
                {
                    return background;
                }

                return Parent?.Background ?? Color.Black;
            }
            set
            {
                if (background != value)
                {
                    OnBackgroundChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(background, value));
                    background = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region Foreground

        /// <summary>
        /// Gets or sets a brush that describes the text of a control
        /// </summary>
        public Color Foreground
        {
            get
            {
                if (foreground != Color.NotSet)
                {
                    return foreground;
                }

                return Parent?.Foreground ?? Color.White;
            }
            set
            {
                if (foreground != value)
                {
                    OnForegroundChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(foreground, value));
                    foreground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region Width

        /// <summary>
        /// Gets or sets the width of the control
        /// </summary>
        public int? Width
        {
            get => width;
            set
            {
                if (width != value)
                {
                    OnWidthChanged?.Invoke(this, new PropertyChangedEventArgs<int?>(width, value));
                    width = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region Height

        /// <summary>
        /// Gets or sets the height of the control.
        /// </summary>
        public int? Height
        {
            get => height;
            set
            {
                if (height != value)
                {
                    OnHeightChanged?.Invoke(this, new PropertyChangedEventArgs<int?>(height, value));
                    height = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region AutoSize

        /// <summary>
        /// Gets or sets a value indicating whether the control is resized in accordance with its contents
        /// </summary>
        public bool AutoSize
        {
            get => autoSize;
            set
            {
                if (autoSize != value)
                {
                    OnAutoSizeChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(autoSize, value));
                    autoSize = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region Tag

        /// <summary>
        /// Gets or sets the object that contains data about the control
        /// </summary>
        public object Tag
        {
            get => tag;
            set
            {
                if (tag != value)
                {
                    OnTagChanged?.Invoke(this, new PropertyChangedEventArgs<object>(tag, value));
                    tag = value;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Invalidates the measurement state (layout) for the element
        /// </summary>
        public void InvalidateMeasure()
        {
            IsMeasureValid = false;
            InvalidateVisual();
        }

        /// <summary>
        /// Invalidates the rendering of the element, and forces a complete new layout pass.
        /// OnRender(IDrawingContext) is called after the layout cycle is completed
        /// </summary>
        public void InvalidateVisual() => IsVisualValid = false;

        /// <summary>
        /// Invalidates the measurement state (layout) for the element if <see cref="AutoSize"/>
        /// property is <see langword="true"/> otherwise invalidates the rendering of the element
        /// </summary>
        public void InvalidateMeasureIfAutoSize()
        {
            if (AutoSize)
            {
                InvalidateMeasure();
            }
            else
            {
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Updates the <see cref="DesiredSize" /> of a <see cref="Control" />. Parent elements call
        /// this method from their own Measure(Size) implementations to form a recursive layout update
        /// </summary>
        /// <param name="availableSize">The available space that a parent element can allocate a child element</param>
        public virtual void Measure(Size availableSize)
        {
            DesiredSize = availableSize;
            IsMeasureValid = true;
            IsVisualValid = false;
        }

        /// <summary>
        /// Positions child elements and determines a size for a <see cref="Control" />. Parent elements call
        /// this method from their Arrange(Rect) implementation to form a recursive layout update
        /// </summary>
        /// <param name="finalRect">The final size that the parent computes for the child element,
        /// provided as a <see cref="Rect"/> instance</param>
        /// <param name="reduceMargin">Indicates if should reduce margin from final rect value</param>
        public virtual void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            bounds = reduceMargin
                ? finalRect.Reduce(margin)
                : finalRect;
            IsVisualValid = false;
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Control> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose() => DisposeManagedResources();

        /// <summary>
        /// Gets the form the control is in
        /// </summary>
        public Window ParentWindow() => (Window) FindParentInternal(x => x is Window);

        /// <summary>
        /// Dispose managed resources
        /// </summary>
        protected virtual void DisposeManagedResources() { }

        internal void BeforeRender()
        {
            DrawingContext = new DefaultDrawingContext(bounds, Id, Parent?.DrawingContext);
            OnRender(DrawingContext);
            IsVisualValid = true;
        }

        /// <summary>
        /// Filling a pseudographics drawing context
        /// </summary>
        protected virtual void OnRender(IDrawingContext context) => context.Release(Background, Foreground, '\0');

        protected Control FindParentInternal(Func<Control, bool> findExpression)
        {
            var control = this;
            while (control != null && !findExpression.Invoke(control))
            {
                control = control.Parent;
            }

            return control;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Padding" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Thickness>> OnPaddingChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Margin" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Thickness>> OnMarginChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Background" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnBackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Foreground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Width" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int?>> OnWidthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Height" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int?>> OnHeightChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AutoSize" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnAutoSizeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Tag" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<object>> OnTagChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Parent" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<ContentControl>> OnParentChanged;

        #endregion
    }
}
