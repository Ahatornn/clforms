using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// Control for drawing some pseudographics chars
    /// </summary>
    public class Canvas: Control, IElementStyle<Canvas>
    {
        /// <summary>
        /// Initialize a new instance <see cref="Canvas"/>
        /// </summary>
        public Canvas()
        {
            Background = Application.SystemColors.WindowBackground;
            Foreground = Application.SystemColors.WindowForeground;
            AutoSize = true;
        }

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;
            if (AutoSize)
            {
                var value = new Size(Math.Min(contentArea.Width + (Margin + Padding).Horizontal, availableSize.Width),
                    Math.Min(contentArea.Height + (Margin + Padding).Vertical, availableSize.Height));
                base.Measure(value);
            }
            else
            {
                base.Measure(new Size(Math.Min(Width ?? availableSize.Width, availableSize.Width),
                    Math.Min(Height ?? availableSize.Height, availableSize.Height)));
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Canvas> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            InheritRender(context);
            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension())
            {
                return;
            }
            OnPaint?.Invoke(this, new PaintEventArgs(reducedArea, context));
        }

        protected virtual void InheritRender(IDrawingContext context) => base.OnRender(context);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the <see cref="Canvas"/> control is rendering
        /// </summary>
        public event EventHandler<PaintEventArgs> OnPaint;

        #endregion
    }
}
