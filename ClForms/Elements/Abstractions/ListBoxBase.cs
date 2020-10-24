using System;
using System.Linq;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Themes;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Represents a basic control to display a list of items
    /// </summary>
    public abstract class ListBoxBase<T>: BaseFocusableControl
    {
        private Color selectedBackground;
        private Color selectedForeground;
        private short updateCount;
        private SelectionMode selectionMode;
        protected readonly SelectedItemCollection<T> SelectedItems;
        private TextAlignment textAlignment;
        private string formatString;
        private int startRenderedItemIndex = 0;
        private readonly ConsoleKey[] forwardDirection;
        private readonly ConsoleKey[] backwardDirection;

        protected ListBoxBase()
        {
            Background = Application.SystemColors.ControlFace;
            Foreground = Application.SystemColors.ControlText;
            FocusBackground = Background;
            FocusForeground = Foreground;
            selectedBackground = Application.SystemColors.Highlight;
            selectedForeground = Application.SystemColors.HighlightText;
            selectionMode = SelectionMode.One;
            Items = new ItemCollection<T>(this);
            SelectedItems = new SelectedItemCollection<T>(this);
            textAlignment = TextAlignment.Left;
            forwardDirection = new[]
            {
                ConsoleKey.DownArrow,
                ConsoleKey.PageDown,
                ConsoleKey.End,
            };
            backwardDirection = new[]
            {
                ConsoleKey.UpArrow,
                ConsoleKey.PageUp,
                ConsoleKey.Home,
            };
            formatString = " {0} ";
            AutoSize = false;
        }

        #region Properties

        #region TextAlignment

        /// <summary>
        /// Gets or sets the horizontal alignment of the contents of the text
        /// </summary>
        public TextAlignment TextAlignment
        {
            get => textAlignment;
            set
            {
                if (textAlignment != value)
                {
                    OnTextAlignmentChanged?.Invoke(this, new PropertyChangedEventArgs<TextAlignment>(textAlignment, value));
                    textAlignment = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region FormatString

        /// <summary>
        /// Gets or sets the format-specifier characters that indicate how a value is to be displayed
        /// </summary>
        public string FormatString
        {
            get => formatString;
            set
            {
                if (formatString != value)
                {
                    OnFormatStringChanged?.Invoke(this, new PropertyChangedEventArgs<string>(formatString, value));
                    formatString = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region SelectedBackground

        /// <summary>
        /// Gets or sets a value to display of background when the item is selected
        /// </summary>
        public Color SelectedBackground
        {
            get => selectedBackground;
            set
            {
                if (selectedBackground != value)
                {
                    OnSelectedBackgroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(selectedBackground, value));
                    selectedBackground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region SelectedForeground

        /// <summary>
        /// Gets or sets a value to display of foreground when the item is selected
        /// </summary>
        public Color SelectedForeground
        {
            get => selectedForeground;
            set
            {
                if (selectedForeground != value)
                {
                    OnSelectedForegroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(selectedForeground, value));
                    selectedForeground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region SelectionMode

        /// <summary>
        /// Gets or sets the method in which items are selected in the <see cref="ListBox"/>
        /// </summary>
        public SelectionMode SelectionMode
        {
            get => selectionMode;
            set
            {
                if (selectionMode != value)
                {
                    OnSelectionModeChanged?.Invoke(this,
                        new PropertyChangedEventArgs<SelectionMode>(selectionMode, value));
                    selectionMode = value;
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets all elements in <see cref="ListBox" />
        /// </summary>
        public ItemCollection<T> Items { get; }

        /// <summary>
        /// Gets the <see cref="Rect"/> of selected item area
        /// </summary>
        public Rect SelectedItemBounds
        {
            get
            {
                if (SelectedIndex == -1)
                {
                    return Rect.Empty;
                }

                return new Rect(DrawingContext.ContextBounds.Left,
                    DrawingContext.ContextBounds.Top + (SelectedIndex - startRenderedItemIndex),
                    DrawingContext.ContextBounds.Width,
                    1);
            }
        }

        /// <summary>
        /// Indicates whether the component is in collection item update mode
        /// </summary>
        public bool IsInUpdateState => updateCount > 0;

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in a <see cref="ListBox"/>
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                if (SelectionMode == SelectionMode.One &&
                    SelectedItems.Any())
                {
                    return Items.IndexOf(SelectedItems[0]);
                }

                return -1;
            }
            set
            {
                if (value < -1 || value >= Items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(SelectedIndex));
                }

                if (SelectionMode == SelectionMode.None)
                {
                    throw new ArgumentException("ListBox invalid selection mode", nameof(SelectedIndex));
                }

                var oldValue = SelectedIndex;
                if (SelectionMode == SelectionMode.One && value != -1)
                {
                    var selectedIndex = SelectedIndex;
                    if (selectedIndex == value)
                    {
                        return;
                    }

                    if (selectedIndex != -1)
                    {
                        SelectedItems.Remove(Items[selectedIndex]);
                    }

                    SelectedItems.Add(Items[value]);
                }
                else if (value == -1)
                {
                    if (SelectedIndex == -1)
                    {
                        return;
                    }

                    SelectedItems.Clear();
                }

                if(oldValue != SelectedIndex)
                {
                    OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(oldValue, SelectedIndex));
                }
                InvalidateVisual();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Maintains performance while items are added to the <see cref="ListBox"/> one at a
        /// time by preventing the control from drawing until the <see cref="EndUpdate()"/> method is called
        /// </summary>
        public void BeginUpdate()
        {
            ++updateCount;
        }

        /// <summary>
        /// Resumes painting the <see cref="ListBox"/> control after painting is suspended by the <see cref="BeginUpdate()"/> method
        /// </summary>
        public void EndUpdate()
        {
            --updateCount;
            if (updateCount == 0)
            {
                InvalidateMeasure();
            }

            if (updateCount < 0)
            {
                updateCount = 0;
            }
        }

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
        {
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(Margin)
                .Reduce(Padding);
            contentArea.Width = Width ?? contentArea.Width;
            contentArea.Height = Height ?? contentArea.Height;
            if (Items.Any() && AutoSize)
            {
                var itemWidth = Math.Min(Width.HasValue
                        ? Width.Value + Margin.Horizontal
                        : Items.Max(GetItemTextLength) + (Margin + Padding).Horizontal,
                    availableSize.Width);
                var itemHeight = Math.Min(Height.HasValue
                        ? Height.Value + Margin.Vertical
                        : Items.Count + (Margin + Padding).Vertical,
                    availableSize.Height);
                base.Measure(new Size(itemWidth, itemHeight));
            }
            else
            {
                base.Measure(new Size(Math.Min(Width ?? availableSize.Width, availableSize.Width),
                        Math.Min(Height ?? availableSize.Height, availableSize.Height)));
            }
        }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            startRenderedItemIndex = SelectedIndex == -1
                ? 0
                : Math.Max(0, SelectedIndex - finalRect.Reduce(Margin).Reduce(Padding).Height + 1);
            base.Arrange(finalRect);
        }

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);

            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension() || !Items.Any())
            {
                return;
            }
            var rowCount = Math.Min(Items.Count, reducedArea.Height);
            for (var row = 0; row < rowCount; row++)
            {
                context.SetCursorPos(Padding.Left, Padding.Top + row);
                var item = Items[row + startRenderedItemIndex];
                OnRenderItemInternal(context, item, reducedArea.Width);
            }
        }

        protected abstract void OnRenderItemInternal(IDrawingContext context, T item, int itemAreaWidth);

        protected abstract bool CanSelectItem(T item);

        /// <inheritdoc cref="BaseFocusableControl.InputActionInternal"/>
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            if (SelectionMode == SelectionMode.None ||
                !Items.Any())
            {
                return;
            }

            if (forwardDirection.Contains(keyInfo.Key))
            {
                var itemsHeight = DrawingContext.ContextBounds.Reduce(Padding).Height;
                SelectedIndex = MoveForward(SelectedIndex, itemsHeight, keyInfo.Key);

                if (SelectedIndex - startRenderedItemIndex > itemsHeight - 1)
                {
                    startRenderedItemIndex = Math.Max(0, SelectedIndex - itemsHeight + 1);
                }
            }

            if (backwardDirection.Contains(keyInfo.Key))
            {
                var itemsHeight = DrawingContext.ContextBounds.Reduce(Padding).Height;

                SelectedIndex = MoveBackward(SelectedIndex, itemsHeight, keyInfo.Key);

                if (SelectedIndex < startRenderedItemIndex)
                {
                    startRenderedItemIndex = SelectedIndex;
                }
            }

            if ((keyInfo.Key == ConsoleKey.Spacebar || keyInfo.Key == ConsoleKey.Enter) &&
                SelectedIndex > -1)
            {
                OnSelectedItemClick?.Invoke(this, Items[SelectedIndex]);
            }
        }

        /// <summary>
        /// Unselects all items in the <see cref="ListBox"/>
        /// </summary>
        public void ClearSelected()
        {
            if (SelectedItems.Any())
            {
                SelectedItems.Clear();
                OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(-1, -1));
                if (updateCount == 0)
                {
                    InvalidateVisual();
                }
            }
        }

        /// <summary>
        /// Unselects specified item in the <see cref="ListBox"/>
        /// </summary>
        public void ClearSelected(T item)
        {
            if (SelectedItems.Contains(item))
            {
                var oldSelectedValue = SelectedIndex;
                SelectedItems.Remove(item);
                if (oldSelectedValue != SelectedIndex)
                {
                    OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(oldSelectedValue, SelectedIndex));
                }
                if (updateCount == 0)
                {
                    InvalidateVisual();
                }
            }
        }

        /// <summary>
        /// Method for determining the length of a list item
        /// </summary>
        protected abstract int GetItemTextLength(T item);

        private int MoveForward(int initIndex, int itemsHeight, ConsoleKey key)
        {
            var selIndex = initIndex;
            if (selIndex == -1)
            {
                selIndex = 0;
            }
            else
            {
                switch (key)
                {
                    case ConsoleKey.PageDown:
                        selIndex += itemsHeight;
                        break;
                    case ConsoleKey.End:
                        selIndex = Items.Count - 1;
                        break;
                    default:
                        selIndex++;
                        break;
                }
            }

            if (selIndex >= Items.Count)
            {
                selIndex = Items.Count - 1;
            }

            if (!CanSelectItem(Items[selIndex]))
            {
                if (key != ConsoleKey.End)
                {
                    for (var i = selIndex; i < Items.Count; i++)
                    {
                        if (CanSelectItem(Items[i]))
                        {
                            return i;
                        }
                    }
                }
                else
                {
                    for (var i = Items.Count - 2; i > initIndex; i--)
                    {
                        if (CanSelectItem(Items[i]))
                        {
                            return i;
                        }
                    }
                }

                return initIndex;
            }

            return selIndex;
        }

        private int MoveBackward(int initIndex, int itemsHeight, ConsoleKey key)
        {
            var selIndex = initIndex;
            if (selIndex == -1)
            {
                selIndex = 0;
            }
            else
            {
                switch (key)
                {
                    case ConsoleKey.PageUp:
                        selIndex -= itemsHeight;
                        break;
                    case ConsoleKey.Home:
                        selIndex = 0;
                        break;
                    default:
                        selIndex--;
                        break;
                }
            }
            if (selIndex < 0)
            {
                selIndex = 0;
            }

            if (!CanSelectItem(Items[selIndex]))
            {
                if (key == ConsoleKey.Home)
                {
                    for (var i = 1; i < selIndex; i++)
                    {
                        if (CanSelectItem(Items[i]))
                        {
                            return i;
                        }
                    }
                }
                else
                {
                    for (var i = selIndex; i >= 0; i--)
                    {
                        if (CanSelectItem(Items[i]))
                        {
                            return i;
                        }
                    }
                }

                return initIndex;
            }

            return selIndex;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnSelectedIndexChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<TextAlignment>> OnTextAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FormatString" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnFormatStringChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedBackground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnSelectedBackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnSelectedForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<SelectionMode>> OnSelectionModeChanged;

        /// <summary>
        /// Occurs when the selected item is clicked
        /// </summary>
        public event EventHandler<T> OnSelectedItemClick;

        #endregion
    }
}
