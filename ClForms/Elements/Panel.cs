using System;
using ClForms.Abstractions;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// Used to group collections of controls
    /// </summary>
    public class Panel: SingleContentControl, IElementStyle<Panel>
    {
        private VerticalAlignment verticalContentAlignment;
        private HorizontalAlignment horizontalContentAlignment;

        /// <summary>
        /// Initialize a new instance <see cref="Panel"/>
        /// </summary>
        public Panel()
        {
            verticalContentAlignment = VerticalAlignment.Top;
            horizontalContentAlignment = HorizontalAlignment.Left;
            AutoSize = false;
        }

        #region Properties

        #region VerticalContentAlignment

        /// <summary>
        /// Gets or sets the vertical alignment of the control's content
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get => verticalContentAlignment;
            set
            {
                if (verticalContentAlignment != value)
                {
                    OnVerticalContentAlignmentChanged?.Invoke(this,
                        new PropertyChangedEventArgs<VerticalAlignment>(verticalContentAlignment, value));
                    verticalContentAlignment = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region HorizontalContentAlignment

        /// <summary>
        /// Gets or sets the horizontal alignment of the control's content
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => horizontalContentAlignment;
            set
            {
                if (horizontalContentAlignment != value)
                {
                    OnHorizontalContentAlignmentChanged?.Invoke(this,
                        new PropertyChangedEventArgs<HorizontalAlignment>(horizontalContentAlignment, value));
                    horizontalContentAlignment = value;
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

            if (Content != null && !desiredContentSize.HasEmptyDimension())
            {
                Content.Measure(desiredContentSize);
                if (AutoSize)
                {
                    var size = Content.DesiredSize;
                    size.Width = Math.Min(Width.HasValue
                            ? Width.Value + Margin.Horizontal
                            : size.Width + (Margin + Padding).Horizontal,
                        availableSize.Width);
                    size.Height = Math.Min(Height.HasValue
                            ? Height.Value + Margin.Vertical
                            : size.Height + (Margin + Padding).Vertical,
                        availableSize.Height);

                    base.Measure(size);
                    return;
                }
            }

            base.Measure(new Size(Math.Min(Width ?? availableSize.Width, availableSize.Width),
                Math.Min(Height ?? availableSize.Height, availableSize.Height)));
        }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            var offset = Margin + Padding;
            var contentRect = new Rect(Padding.Left,
                Padding.Top,
                finalRect.Width - offset.Horizontal,
                finalRect.Height - offset.Vertical);
            var leftIndent = contentRect.Left;
            var topIndent = contentRect.Top;
            switch (horizontalContentAlignment)
            {
                case HorizontalAlignment.Center:
                    leftIndent = contentRect.Left + (contentRect.Width - Content?.DesiredSize.Width ?? 0) / 2;
                    break;
                case HorizontalAlignment.Right:
                    leftIndent = contentRect.Right - Content.DesiredSize.Width;
                    break;
            }
            switch (verticalContentAlignment)
            {
                case VerticalAlignment.Center:
                    topIndent = contentRect.Top + (contentRect.Height - Content?.DesiredSize.Height ?? 0) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    topIndent = contentRect.Bottom - Content?.DesiredSize.Height ?? 0;
                    break;
            }
            Content?.Arrange(new Rect(leftIndent, topIndent, Content.DesiredSize.Width, Content.DesiredSize.Height));
            base.Arrange(finalRect);
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Panel> styleAction) => styleAction?.Invoke(this);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalContentAlignment" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<HorizontalAlignment>> OnHorizontalContentAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalContentAlignment" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<VerticalAlignment>> OnVerticalContentAlignmentChanged;

        #endregion
    }
}
