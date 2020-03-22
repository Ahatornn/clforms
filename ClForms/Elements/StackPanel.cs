using System;
using System.Linq;
using ClForms.Abstractions;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Arranges child elements into a single line that can be oriented horizontally or vertically
    /// </summary>
    public class StackPanel: MultipleContentControl, IElementStyle<StackPanel>
    {
        private Orientation orientation;

        /// <summary>
        /// Initialize a new instance <see cref="StackPanel"/>
        /// </summary>
        public StackPanel()
        {
            orientation = Orientation.Horizontal;
            Background = Color.NotSet;
            Foreground = Color.NotSet;
        }

        /// <summary>
        /// Initialize a new instance <see cref="StackPanel"/> with value of <see cref="Orientation"/>
        /// </summary>
        public StackPanel(Orientation orientation)
            : this()
        {
            this.orientation = orientation;
        }

        #region Properties

        #region Orientation

        /// <summary>
        /// Gets or sets a value that indicates the dimension by which child elements are stacked
        /// </summary>
        public Orientation Orientation
        {
            get => orientation;
            set
            {
                if (orientation != value)
                {
                    OnOrientationChanged?.Invoke(this, new PropertyChangedEventArgs<Orientation>(orientation, value));
                    orientation = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;

            var desiredContentSize = contentArea.Size;

            var internalChildren = this.ToList();
            if (!desiredContentSize.HasEmptyDimension() && internalChildren.Any())
            {
                var size = new Size();
                var isHorizontal = Orientation == Orientation.Horizontal;
                var index = 0;
                for (var count = internalChildren.Count; index < count; ++index)
                {
                    var uiElement = internalChildren[index];
                    if (uiElement != null)
                    {
                        uiElement.Measure(new Size(
                            isHorizontal
                                ? desiredContentSize.Width - size.Width
                                : desiredContentSize.Width,
                            isHorizontal
                                ? desiredContentSize.Height
                                : desiredContentSize.Height - size.Height));
                        var desiredSize = uiElement.DesiredSize;
                        if (isHorizontal)
                        {
                            size.Width += desiredSize.Width;
                            size.Height = Math.Max(size.Height, desiredSize.Height);
                        }
                        else
                        {
                            size.Width = Math.Max(size.Width, desiredSize.Width);
                            size.Height += desiredSize.Height;
                        }
                    }
                }
                size.Width = Math.Min(Width.HasValue
                        ? Width.Value + Margin.Horizontal
                        : size.Width + (Margin + Padding).Horizontal,
                    availableSize.Width);
                size.Height = Math.Min(Height.HasValue
                        ? Height.Value + Margin.Vertical
                        : size.Height + (Margin + Padding).Vertical,
                    availableSize.Height);

                base.Measure(size);
            }
            else
            {
                base.Measure(new Size(Math.Min((Width ?? Padding.Horizontal) + Margin.Horizontal, availableSize.Width),
                    Math.Min((Height ?? Padding.Vertical) + Margin.Vertical, availableSize.Height)));
            }
        }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            var contentRect = new Rect(0, 0, finalRect.Width, finalRect.Height)
                .Reduce(Margin)
                .Reduce(Padding);
            StackChildrenArrange(contentRect);
            base.Arrange(finalRect, reduceMargin: false);
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<StackPanel> styleAction) => styleAction?.Invoke(this);

        internal void StackChildrenArrange(Rect contentRect)
        {
            var internalChildren = this.ToList();
            var isHorizontal = Orientation == Orientation.Horizontal;
            var finalRect = new Rect(contentRect.X - Margin.Left,
                contentRect.Y - Margin.Top,
                contentRect.Width,
                contentRect.Height);
            var num = 0;
            var index = 0;
            for (var count = internalChildren.Count; index < count; ++index)
            {
                var uiElement = internalChildren[index];
                if (uiElement != null)
                {
                    if (isHorizontal)
                    {
                        finalRect.X += num;
                        num = uiElement.DesiredSize.Width;
                        finalRect.Width = num;
                        finalRect.Height = Math.Max(contentRect.Height, uiElement.DesiredSize.Height);
                    }
                    else
                    {
                        finalRect.Y += num;
                        num = uiElement.DesiredSize.Height;
                        finalRect.Height = num;
                        finalRect.Width = Math.Max(contentRect.Width, uiElement.DesiredSize.Width);
                    }
                    uiElement.Arrange(finalRect);
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Orientation>> OnOrientationChanged;

        #endregion
    }
}
