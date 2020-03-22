using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows control to display a list of items
    /// </summary>
    public class ListBox<T>: ListBoxBase<T>, IElementStyle<ListBox<T>>
    {
        protected override void OnRenderItemInternal(IDrawingContext context, T item, int itemAreaWidth) => throw new NotImplementedException();

        protected override bool CanSelectItem(T item) => throw new NotImplementedException();

        protected override int GetItemTextLength(T item) => throw new NotImplementedException();

        public void SetStyle(Action<ListBox<T>> styleAction) => throw new NotImplementedException();
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
