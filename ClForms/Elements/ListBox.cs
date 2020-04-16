using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows control to display a list of items
    /// </summary>
    public class ListBox<T>: ListBoxBase<T>, IElementStyle<ListBox<T>>
    {
        /// <inheritdoc />
        protected override void OnRenderItemInternal(IDrawingContext context, T item, int itemAreaWidth)
        {
            var itemText = GetItemText(item);
            var drawingText = itemText.Substring(0, Math.Min(itemAreaWidth, itemText.Length));

            var backColor = GetRenderBackColor();
            var foreColor = GetRenderForeColor();
            if (SelectedItems.Contains(item))
            {
                backColor = IsDisabled
                    ? DisabledForeground
                    : SelectedBackground;
                foreColor = IsDisabled
                    ? DisabledBackground
                    : SelectedForeground;
            }

            context.DrawText(TextHelper.GetTextWithAlignment(drawingText, itemAreaWidth, TextAlignment),
                backColor,
                foreColor);
        }

        /// <inheritdoc />
        protected override bool CanSelectItem(T item) => true;

        /// <inheritdoc />
        protected override int GetItemTextLength(T item) => GetItemText(item).Length;

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<ListBox<T>> styleAction) => styleAction?.Invoke(this);

        private string GetItemText(T item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(FormatString))
            {
                return string.Format(FormatString, item);
            }

            return item.ToString();
        }
    }

    /// <summary>
    /// Represents a Windows control to display a list of items
    /// </summary>
    public class ListBox: ListBox<object>, IElementStyle<ListBox>
    {
        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<ListBox> styleAction) => styleAction?.Invoke(this);
    }
}
