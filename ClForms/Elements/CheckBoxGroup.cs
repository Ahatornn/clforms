using System;
using System.Collections.Generic;
using System.Linq;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// A group of related but mutually exclusive checkbox buttons.
    /// </summary>
    public class CheckBoxGroup: GroupItemControl, IElementStyle<CheckBoxGroup>
    {
        private readonly IList<GroupItemElement> selectedItems;

        /// <summary>
        /// Initialize a new instance <see cref="CheckBoxGroup"/>
        /// </summary>
        public CheckBoxGroup()
        {
            Background = Color.NotSet;
            Foreground = Color.NotSet;
            DisabledForeground = Application.SystemColors.ButtonInactiveFace;
            FocusForeground = Application.SystemColors.ButtonFocusedFace;

            selectedItems = new List<GroupItemElement>();
        }

        #region Properties

        /// <inheritdoc />
        protected override int ContentMeasureItemIndent => 2;

        /// <summary>
        /// Gets or sets the zero-based index of the all selected items in a <see cref="CheckBoxGroup"/>
        /// </summary>
        public IEnumerable<int> SelectedIndexes
        {
            get
            {
                foreach (var item in selectedItems)
                {
                    yield return Items.IndexOf(item);
                }
            }
            set
            {
                selectedItems.Clear();
                foreach (var index in value)
                {
                    var item = Items[index];
                    if (item != null)
                    {
                        selectedItems.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CheckState"/> of specified item by index
        /// </summary>
        public CheckState this[int index]
        {
            get
            {
                var item = Items[index];
                return item != null && selectedItems.Contains(item)
                    ? CheckState.Checked
                    : CheckState.Unchecked;
            }
            set
            {
                var item = Items[index];

                if (value == CheckState.Checked && !selectedItems.Contains(item))
                {
                    selectedItems.Add(item);
                }

                if (value == CheckState.Unchecked && selectedItems.Contains(item))
                {
                    selectedItems.Remove(item);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks all the items in the <see cref="CheckBoxGroup"/>
        /// </summary>
        public void CheckAll(bool excludeDisabled = false)
        {
            foreach (var item in Items)
            {
                if (!(selectedItems.Contains(item) || (excludeDisabled && item.IsDisabled)))
                {
                    selectedItems.Add(item);
                }
            }
        }

        /// <summary>
        /// Uncheck all the items in the <see cref="CheckBoxGroup"/>
        /// </summary>
        public void UncheckAll(bool excludeDisabled = false)
        {
            foreach (var item in Items)
            {
                if (selectedItems.Contains(item) && !(excludeDisabled && item.IsDisabled))
                {
                    selectedItems.Remove(item);
                }
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<CheckBoxGroup> styleAction) => styleAction?.Invoke(this);

        protected override void OnContentRender(IDrawingContext context)
        {
            if (focusedIndex == -1)
            {
                var visibleItems = chunks.AllList;
                var firstElement = visibleItems.FirstOrDefault(x => !x.IsDisabled);
                if (firstElement != null)
                {
                    focusedIndex = visibleItems.IndexOf(firstElement);
                }
            }
            var renderArea = new Rect(context.ContextBounds.Size)
                .Reduce(BorderThickness)
                .Reduce(Padding);
            var leftIndent = renderArea.Left + (renderArea.Width - contentSize.Width) / 2;
            var topIndent = renderArea.Top + (renderArea.Height - contentSize.Height) / 2;

            for (var col = 0; col < chunks.Count; col++)
            {
                var items = chunks[col].ToList();
                for (var i = 0; i < items.Count; i++)
                {
                    if (!renderArea.Contains(leftIndent, topIndent + i))
                    {
                        break;
                    }
                    var foreColor = IsDisabled || items[i].IsDisabled
                        ? DisabledForeground
                        : IsFocus && Items.IndexOf(items[i]) == focusedIndex
                            ? FocusForeground
                            : Foreground;
                    context.SetCursorPos(leftIndent, topIndent + i);
                    context.DrawText($"{GetGlyph(selectedItems.Contains(items[i]))} {items[i].Text}", foreColor);
                }
                leftIndent += items.Max(x => x.Text.Length + 3) + 2;
            }
        }

        /// <inheritdoc />
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            base.InputActionInternal(keyInfo);

            if (keyInfo.Key == ConsoleKey.Spacebar || keyInfo.Key == ConsoleKey.Enter)
            {
                var item = chunks.AllList[focusedIndex];
                if (item != null)
                {
                    if (selectedItems.Contains(item))
                    {
                        selectedItems.Remove(item);
                        OnSelectedCheckedChanged?.Invoke(this, new SelectedItemCheckedEventArgs(focusedIndex, CheckState.Unchecked));
                    }
                    else
                    {
                        selectedItems.Add(item);
                        OnSelectedCheckedChanged?.Invoke(this, new SelectedItemCheckedEventArgs(focusedIndex, CheckState.Checked));
                    }

                    InvalidateVisual();
                }
            }
        }

        internal override void ItemsClearInterceptor()
        {
            selectedItems.Clear();
            focusedIndex = -1;
        }

        private char GetGlyph(bool isChecked)
            => Application.Environment.GetCheckStateChar(isChecked ? CheckState.Checked : CheckState.Unchecked);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the selected item changes state
        /// </summary>
        public event EventHandler<SelectedItemCheckedEventArgs> OnSelectedCheckedChanged;

        #endregion
    }
}
