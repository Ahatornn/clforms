using System;
using System.Collections.Concurrent;
using ClForms.Abstractions;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// Defines an area where you can arrange child elements either horizontally
    /// or vertically, relative to each other
    /// </summary>
    public class DockPanel: MultipleContentControl, IElementStyle<DockPanel>
    {
        private readonly ConcurrentDictionary<long, Dock> controlDocks;
        private bool lastChildFill;
        private readonly Dock defaultDockBehavior;
        private ControlState state;

        /// <summary>
        /// Initialize a new instance <see cref="DockPanel"/>
        /// </summary>
        public DockPanel()
        {
            controlDocks = new ConcurrentDictionary<long, Dock>();
            State = ControlState.Maximized;
            defaultDockBehavior = Dock.Top;
        }

        #region Properties

        #region LastChildFill

        /// <summary>
        /// Gets or sets a value that indicates whether the last child element within a <see cref="DockPanel"/> stretches
        /// to fill the remaining available space.
        /// </summary>
        public bool LastChildFill
        {
            get => lastChildFill;
            set
            {
                if (lastChildFill != value)
                {
                    OnLastChildFillChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(lastChildFill, value));
                    lastChildFill = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region State

        /// <summary>
        /// Gets or sets a <see cref="ControlState"/> value
        /// </summary>
        public ControlState State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    OnStateChanged?.Invoke(this, new PropertyChangedEventArgs<ControlState>(state, value));
                    state = value;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="MultipleContentControl.AddContent"/>
        public virtual void AddContent(Control content, Dock dock)
        {
            AddContent(content);
            SetDock(content, dock);
        }

        /// <inheritdoc cref="MultipleContentControl.AddContentInterceptor"/>
        protected override void AddContentInterceptor(Control content) => SetDock(content, defaultDockBehavior);

        /// <summary>
        /// Set of the <see cref="Dock"/> value for a specified control
        /// </summary>
        public void SetDock(Control content, Dock dock)
        {
            controlDocks.AddOrUpdate(content.Id, dock, (key, value) => dock);
            InvalidateMeasure();
        }

        /// <summary>
        /// Get of the <see cref="Dock"/> value for a specified control
        /// </summary>
        public Dock GetDock(Control content)
        {
            if (controlDocks.TryGetValue(content.Id, out var value))
            {
                return value;
            }

            throw new InvalidOperationException($"Failed to read Dock property of the {content}");
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<DockPanel> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;

            var desiredContentSize = contentArea.Size;
            var leftRightHeight = 0;
            var leftRightWidth = 0;
            var topBottomHeight = 0;
            var topBottomWidth = 0;
            foreach (var element in this)
            {
                if (element != null)
                {
                    var constraint = new Size(Math.Max(0, desiredContentSize.Width - leftRightWidth), Math.Max(0, desiredContentSize.Height - topBottomHeight));
                    element.Measure(constraint);
                    var desiredSize = element.DesiredSize;
                    switch (GetDock(element))
                    {
                        case Dock.Left:
                        case Dock.Right:
                            leftRightHeight = Math.Max(leftRightHeight, topBottomHeight + desiredSize.Height);
                            leftRightWidth += desiredSize.Width;
                            continue;
                        case Dock.Top:
                        case Dock.Bottom:
                            topBottomWidth = Math.Max(topBottomWidth, leftRightWidth + desiredSize.Width);
                            topBottomHeight += desiredSize.Height;
                            continue;
                        default:
                            continue;
                    }
                }
            }

            if (State == ControlState.Maximized)
            {
                base.Measure(availableSize);
            }
            else
            {
                var contentWidth = (Width ?? Math.Max(topBottomWidth, leftRightWidth)) +
                                   Margin.Horizontal +
                                   (!Width.HasValue
                                       ? Padding.Horizontal
                                       : 0);
                var contentHeight = (Height ?? Math.Max(leftRightHeight, topBottomHeight)) +
                                    Margin.Vertical +
                                    (!Height.HasValue
                                        ? Padding.Vertical
                                        : 0);

                base.Measure(new Size(Math.Min(contentWidth, availableSize.Width),
                    Math.Min(contentHeight, availableSize.Height)));
            }
        }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            var contentRect = new Rect(0, 0, finalRect.Width, finalRect.Height)
                .Reduce(Margin)
                .Reduce(Padding);

            var num1 = ContentCount - (LastChildFill ? 1 : 0);
            var x = 0;
            var y = 0;
            var num2 = 0;
            var num3 = 0;
            var index = 0;
            foreach (var element in this)
            {
                if (element != null)
                {
                    var desiredSize = element.DesiredSize;
                    var arrangeSize = new Rect(x + Padding.Left,
                        y + Padding.Top,
                        Math.Max(0, contentRect.Width - (x + num2)),
                        Math.Max(0, contentRect.Height - (y + num3)));
                    if (index < num1)
                    {
                        switch (GetDock(element))
                        {
                            case Dock.Left:
                                x += desiredSize.Width;
                                arrangeSize.Width = desiredSize.Width;
                                break;
                            case Dock.Top:
                                y += desiredSize.Height;
                                arrangeSize.Height = desiredSize.Height;
                                break;
                            case Dock.Right:
                                num2 += desiredSize.Width;
                                arrangeSize.X = Math.Max(0, contentRect.Width - num2) + Padding.Left;
                                arrangeSize.Width = desiredSize.Width;
                                break;
                            case Dock.Bottom:
                                num3 += desiredSize.Height;
                                arrangeSize.Y = Math.Max(0, contentRect.Height - num3) + Padding.Top;
                                arrangeSize.Height = desiredSize.Height;
                                break;
                        }
                    }
                    element.Arrange(arrangeSize);
                }

                index++;
            }
            base.Arrange(finalRect, false);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="LastChildFill" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnLastChildFillChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="State" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<ControlState>> OnStateChanged;

        #endregion
    }
}
