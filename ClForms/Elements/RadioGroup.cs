using System;
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
    /// A group of related but mutually exclusive radio buttons, requiring the user to select one of a set of alternatives.
    /// As one button becomes selected, the remaining buttons in the group become automatically deselected
    /// </summary>
    public class RadioGroup: GroupItemControl, IElementStyle<RadioGroup>
    {
        private int focusedIndex = -1;
        private int selectedIndex = -1;

        /// <summary>
        /// Initialize a new instance <see cref="RadioGroup"/>
        /// </summary>
        public RadioGroup()
        {
            Background = Color.NotSet;
            Foreground = Color.NotSet;
            DisabledForeground = Application.SystemColors.ButtonInactiveFace;
            FocusForeground = Application.SystemColors.ButtonFocusedFace;
        }

        #region Properties

        #region SelectedIndex

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in a <see cref="RadioGroup"/>
        /// </summary>
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex != value)
                {
                    if (selectedIndex < -1 && selectedIndex > Items.Count)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(selectedIndex, value));
                    selectedIndex = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        /// <inheritdoc />
        protected override bool ApplyMeasureDelegate => true;

        /// <inheritdoc />
        protected override bool ApplyArrangeDelegate => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnContentRender(IDrawingContext context)
        {
            if (focusedIndex == -1)
            {
                var firstElement = Items.FirstOrDefault(x => !x.IsDisabled);
                if (firstElement != null)
                {
                    focusedIndex = Items.IndexOf(firstElement);
                }
            }
            var leftIndent = BorderThickness.Left + Padding.Left;
            var topIndent = BorderThickness.Top + Padding.Top;

            var maxColumns = Math.Min(Columns, Items.Count);
            var columnHeight = (int) Math.Ceiling((decimal) Items.Count / maxColumns);
            var skip = 0;
            for (var col = 0; col < maxColumns; col++)
            {
                var stackHeight = Math.Min((Items.Count - skip) - (maxColumns - (col + 1)), columnHeight);

                var items = Items.Skip(skip).Take(stackHeight).ToList();
                for (var i = 0; i < items.Count; i++)
                {
                    if (!context.ContextBounds.Contains(leftIndent, topIndent + i))
                    {
                        break;
                    }

                    var foreColor = IsDisabled || items[i].IsDisabled
                        ? DisabledForeground
                        : IsFocus && Items.IndexOf(items[i]) == focusedIndex
                            ? FocusForeground
                            : Foreground;

                    context.SetCursorPos(leftIndent, topIndent + i);
                    context.DrawText($"{GetGlyph(Items.IndexOf(items[i]) == SelectedIndex)}  {items[i].Text}", foreColor);
                }
                leftIndent += items.Max(x => x.Text.Length + 3) + 2;
                skip += stackHeight;
            }
        }

        /// <inheritdoc />
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            GroupItemElement item = null;
            if (keyInfo.Key == ConsoleKey.Home)
            {
                item = Items.FirstOrDefault(x => !x.IsDisabled);
            }

            if (keyInfo.Key == ConsoleKey.End)
            {
                item = Items.LastOrDefault(x => !x.IsDisabled);
            }

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                item = Items
                    .Take(focusedIndex)
                    .LastOrDefault(x => !x.IsDisabled)
                       ?? Items.LastOrDefault(x => !x.IsDisabled);
            }

            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                item = Items
                    .Skip(focusedIndex + 1)
                    .FirstOrDefault(x => !x.IsDisabled)
                       ?? Items.FirstOrDefault(x => !x.IsDisabled);
            }

            var maxColumns = Math.Min(Columns, Items.Count);
            var columnHeight = (int) Math.Ceiling((decimal) Items.Count / maxColumns);

            if (keyInfo.Key == ConsoleKey.RightArrow && maxColumns > 1)
            {
                var currentColumnIndex = GetFocusedItemColumnIndex(columnHeight);
                if (currentColumnIndex != -1)
                {
                    var stack = Items.Skip(currentColumnIndex * columnHeight)
                        .Take(columnHeight)
                        .ToList();
                    var indent = stack.IndexOf(Items[focusedIndex]);
                    var columnInfo = GetIndexItemsOfColumn(currentColumnIndex + 1 >= maxColumns
                            ? 0
                            : currentColumnIndex + 1,
                        maxColumns,
                        columnHeight);

                    item = Items
                               .Skip(columnInfo.StartIndex)
                               .Take(columnInfo.ItemCount)
                               .Skip(Math.Min(indent, columnInfo.ItemCount - 1))
                               .FirstOrDefault(x => !x.IsDisabled)
                           ?? Items
                               .Skip(columnInfo.StartIndex)
                               .FirstOrDefault(x => !x.IsDisabled)
                           ?? Items
                               .FirstOrDefault(x => !x.IsDisabled);
                }
            }

            if (keyInfo.Key == ConsoleKey.LeftArrow && maxColumns > 1)
            {
                var currentColumnIndex = GetFocusedItemColumnIndex(columnHeight);
                if (currentColumnIndex != -1)
                {
                    var stack = Items.Skip(currentColumnIndex * columnHeight)
                        .Take(columnHeight)
                        .ToList();
                    var indent = stack.IndexOf(Items[focusedIndex]);
                    var columnInfo = GetIndexItemsOfColumn(currentColumnIndex - 1 < 0
                            ? maxColumns
                            : currentColumnIndex - 1,
                        maxColumns,
                        columnHeight);

                    item = Items
                               .Skip(columnInfo.StartIndex - (columnInfo.StartIndex == Items.Count ? 1 : 0))
                               .Take(indent + 1)
                               .LastOrDefault(x => !x.IsDisabled)
                           ?? Items
                               .Skip(columnInfo.StartIndex - -(columnInfo.StartIndex == Items.Count ? 1 : 0))
                               .Take(columnInfo.ItemCount)
                               .FirstOrDefault(x => !x.IsDisabled)
                           ?? Items
                               .LastOrDefault(x => !x.IsDisabled);
                }
            }

            if (item != null)
            {
                var newIndex = Items.IndexOf(item);
                if (newIndex != focusedIndex)
                {
                    focusedIndex = newIndex;
                    InvalidateVisual();
                }
            }

            if (keyInfo.Key == ConsoleKey.Spacebar || keyInfo.Key == ConsoleKey.Enter)
            {
                var element = Items[focusedIndex];
                if (!element.IsDisabled)
                {
                    SelectedIndex = Items.IndexOf(element);
                }
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<RadioGroup> styleAction) => styleAction?.Invoke(this);

        private char GetGlyph(bool isChecked) => Application.Environment.GetRadioCheckChar(isChecked);

        /// <inheritdoc />
        protected override Func<Size, Size> ContentMeasureDelegate =>
            contentSize =>
            {
                var maxColumns = Math.Min(Columns, Items.Count);
                var resultSize = Size.Empty;
                resultSize.Height = (int) Math.Ceiling((decimal) Items.Count / maxColumns);

                var skip = 0;
                for (var i = 0; i < maxColumns; i++)
                {
                    var stackHeight = Math.Min((Items.Count - skip) - (maxColumns - (i + 1)), resultSize.Height);

                    var items = Items.Skip(skip).Take(stackHeight);
                    resultSize.Width += items.Max(x => x.Text.Length + 3);
                    skip += stackHeight;
                }

                resultSize.Width += (maxColumns - 1) * 2;

                return new Size(Math.Min(contentSize.Width, resultSize.Width),
                    Math.Min(contentSize.Height, resultSize.Height));
            };

        /// <inheritdoc />
        protected override Action<Rect> ContentArrangeDelegate => null;

        private int GetFocusedItemColumnIndex(int columnHeight)
        {
            var focusedItem = Items[focusedIndex];
            if (focusedItem == null || Items.Count(x => !x.IsDisabled) == 0)
            {
                return -1;
            }

            return Items.IndexOf(focusedItem) / columnHeight;
        }

        private (int StartIndex, int ItemCount) GetIndexItemsOfColumn(int column, int maxColumns, int height)
        {
            var skip = 0;
            for (var col = 0; col <= column; col++)
            {
                var stackHeight = Math.Min((Items.Count - skip) - (maxColumns - (col + 1)), height);
                if (col == column)
                {
                    return (skip, stackHeight);
                }
                skip += stackHeight;
            }

            throw new IndexOutOfRangeException();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnSelectedIndexChanged;

        #endregion
    }
}
