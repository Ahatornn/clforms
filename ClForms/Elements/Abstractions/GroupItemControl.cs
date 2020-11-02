using System;
using System.Linq;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Themes;

namespace ClForms.Elements.Abstractions
{
    /// <summary>
    /// Base class for group controls
    /// </summary>
    public abstract class GroupItemControl: BaseFocusableControl
    {
        private readonly GroupBase groupBase;
        protected int focusedIndex = -1;
        private int columns;
        protected readonly Chunks<GroupItemElement> chunks;
        protected Size contentSize;

        /// <summary>
        /// Initialize a new instance <see cref="GroupItemControl"/>
        /// </summary>
        protected GroupItemControl()
        {
            groupBase = new GroupBase(this);
            Items = new GroupItemCollection(this);
            columns = 1;
            chunks = new Chunks<GroupItemElement>();
            contentSize = Size.Empty;
        }

        #region Properties

        #region Text

        /// <summary>
        /// Gets or sets the text associated with this control
        /// </summary>
        public string Text
        {
            get => groupBase.text;
            set
            {
                if (groupBase.text != value)
                {
                    OnTextChanged?.Invoke(this, new PropertyChangedEventArgs<string>(groupBase.text, value));
                    groupBase.text = value;
                    groupBase.RecalculateTextPosition();
                    InvalidateMeasureIfAutoSize();
                }
            }
        }

        #endregion
        #region TextAlignment

        /// <summary>
        /// Gets or sets the horizontal alignment of the text associated with this control
        /// </summary>
        public TextAlignment TextAlignment
        {
            get => groupBase.textAlignment;
            set
            {
                if (groupBase.textAlignment != value)
                {
                    OnTextAlignmentChanged?.Invoke(this,
                        new PropertyChangedEventArgs<TextAlignment>(groupBase.textAlignment, value));
                    groupBase.textAlignment = value;
                    groupBase.RecalculateTextPosition();
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region BorderThickness

        /// <summary>
        /// Gets or sets the relative frame <see cref="Thickness"/> of a <see cref="GroupBox"/>
        /// </summary>
        public Thickness BorderThickness
        {
            get => groupBase.borderThickness;
            set
            {
                if (groupBase.borderThickness != value)
                {
                    OnBorderThicknessChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Thickness>(groupBase.borderThickness, value));
                    groupBase.borderThickness = value;
                    InvalidateMeasure();
                }
            }
        }

        #endregion
        #region BorderColor

        /// <summary>
        /// Gets or sets the <see cref="Color"/> that draws the border color
        /// </summary>
        public Color BorderColor
        {
            get => groupBase.borderColor;
            set
            {
                if (groupBase.borderColor != value)
                {
                    OnBorderColorChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(groupBase.borderColor, value));
                    groupBase.borderColor = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region BorderChars

        /// <summary>
        /// Gets or sets the <see cref="BorderChars"/> that draws the border characters
        /// </summary>
        public BorderChars BorderChars
        {
            get => groupBase.borderChars;
            set
            {
                if (groupBase.borderChars != value)
                {
                    OnBorderCharsChanged?.Invoke(this, new PropertyChangedEventArgs<BorderChars>(groupBase.borderChars, value));
                    groupBase.borderChars = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region Columns

        /// <summary>
        /// Gets or sets the number of columns in the switch group
        /// </summary>
        public int Columns
        {
            get => columns;
            set
            {
                if (columns != value)
                {
                    if (value < 1)
                    {
                        throw new InvalidOperationException($"{nameof(Columns)}'s value must be greater than one or equal");
                    }
                    OnColumnsChanged?.Invoke(this, new PropertyChangedEventArgs<int>(columns, value));
                    columns = value;
                    InvalidateMeasure();
                }
            }

        }

        #endregion

        /// <summary>
        /// Gets all elements in <see cref="GroupItemControl" />
        /// </summary>
        public GroupItemCollection Items { get; }

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
            => base.Measure(groupBase.Measure(availableSize,
                contentSize => ContentMeasureDelegate(contentSize),
                ApplyMeasureDelegate));

        protected virtual bool ApplyMeasureDelegate { get; } = true;

        protected virtual Func<Size, Size> ContentMeasureDelegate => contentSize =>
        {
            if (Items.Count == 0 || !AutoSize)
            {
                return AutoSize
                    ? Size.Empty
                    : contentSize;
            }

            var maxColumns = Math.Min(Columns, Items.Count);
            var resultSize = Size.Empty;
            resultSize.Height = (int) Math.Ceiling((decimal) Items.Count / maxColumns);

            var skip = 0;
            for (var i = 0; i < maxColumns; i++)
            {
                var stackHeight = Math.Min((Items.Count - skip) - (maxColumns - (i + 1)), resultSize.Height);

                var items = Items.Skip(skip).Take(stackHeight);
                resultSize.Width += items.Max(x => x.Text.Length + ContentMeasureItemIndent);
                skip += stackHeight;
            }

            resultSize.Width += (maxColumns - 1) * 2;

            return new Size(Math.Min(contentSize.Width, resultSize.Width),
                Math.Min(contentSize.Height, resultSize.Height));
        };

        protected abstract int ContentMeasureItemIndent { get; }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true) =>
            base.Arrange(groupBase.Arrange(finalRect,
                clientRect => ContentArrangeDelegate(clientRect),
                ApplyArrangeDelegate), reduceMargin);

        protected virtual bool ApplyArrangeDelegate { get; } = true;

        protected virtual Action<Rect> ContentArrangeDelegate => finalSize =>
        {
            chunks.Clear();
            contentSize = Size.Empty;
            var maxColumns = Math.Min(Columns, Items.Count);
            contentSize.Height = (int) Math.Ceiling((decimal) Items.Count / maxColumns);
            var skip = 0;
            for (var i = 0; i < maxColumns && contentSize.Width < finalSize.Width; i++)
            {
                var stackHeight = Math.Min((Items.Count - skip) - (maxColumns - (i + 1)), contentSize.Height);

                var items = Items.Skip(skip).Take(stackHeight);
                contentSize.Width += items.Max(x => x.Text.Length + ContentMeasureItemIndent);
                chunks.Add(items.Take(Math.Min(finalSize.Height, stackHeight)));
                skip += stackHeight;
            }
            contentSize.Width = Math.Min(finalSize.Width, contentSize.Width += (maxColumns - 1) * 2);
            contentSize.Height = Math.Min(finalSize.Height, contentSize.Height);
        };

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            groupBase.OnRender(context);
            if (Items.Count > 0)
            {
                OnContentRender(context);
            }
        }

        protected abstract void OnContentRender(IDrawingContext context);

        /// <inheritdoc />
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            GroupItemElement item = null;
            if (keyInfo.Key == ConsoleKey.Home)
            {
                item = chunks.AllList.FirstOrDefault(x => !x.IsDisabled);
            }

            if (keyInfo.Key == ConsoleKey.End)
            {
                item = chunks.AllList.LastOrDefault(x => !x.IsDisabled);
            }

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                var visibleItems = chunks.AllList;
                item = visibleItems
                           .Take(focusedIndex)
                           .LastOrDefault(x => !x.IsDisabled)
                       ?? visibleItems.LastOrDefault(x => !x.IsDisabled);
            }

            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                var visibleItems = chunks.AllList;
                item = visibleItems
                           .Skip(focusedIndex + 1)
                           .FirstOrDefault(x => !x.IsDisabled)
                       ?? visibleItems.FirstOrDefault(x => !x.IsDisabled);
            }

            if (keyInfo.Key == ConsoleKey.RightArrow && chunks.Count > 1)
            {
                var currentValue = GetFocusedItemIndexAndColumn();
                if (currentValue.Index != -1)
                {
                    var targetColumnItems = currentValue.Column < chunks.Count - 1
                        ? chunks[currentValue.Column + 1]
                        : chunks[0];
                    if (targetColumnItems.Any(x => !x.IsDisabled))
                    {
                        if (targetColumnItems.Count >= currentValue.Index)
                        {
                            item = targetColumnItems.Skip(currentValue.Index).FirstOrDefault(x => !x.IsDisabled);
                        }

                        item ??= targetColumnItems.LastOrDefault(x => !x.IsDisabled);
                    }

                    if (item == null)
                    {
                        var visibleItems = chunks.AllList;
                        item = visibleItems
                                   .Skip(focusedIndex + 1)
                                   .FirstOrDefault(x => !x.IsDisabled)
                               ?? visibleItems.FirstOrDefault(x => !x.IsDisabled);
                    }
                }
            }

            if (keyInfo.Key == ConsoleKey.LeftArrow && chunks.Count > 1)
            {
                var currentValue = GetFocusedItemIndexAndColumn();
                if (currentValue.Index != -1)
                {
                    var targetColumnItems = currentValue.Column > 0
                        ? chunks[currentValue.Column - 1]
                        : chunks[^1];
                    if (targetColumnItems.Any(x => !x.IsDisabled))
                    {
                        if (targetColumnItems.Count >= currentValue.Index)
                        {
                            item = targetColumnItems.Take(currentValue.Index + 1).LastOrDefault(x => !x.IsDisabled);
                        }

                        item ??= targetColumnItems.FirstOrDefault(x => !x.IsDisabled);
                    }

                    if (item == null)
                    {
                        var visibleItems = chunks.AllList;
                        item = visibleItems
                                   .Take(focusedIndex)
                                   .LastOrDefault(x => !x.IsDisabled)
                               ?? visibleItems.LastOrDefault(x => !x.IsDisabled);
                    }
                }
            }

            if (item != null)
            {
                var newIndex = chunks.AllList.IndexOf(item);
                if (newIndex != focusedIndex)
                {
                    focusedIndex = newIndex;
                    InvalidateVisual();
                }
            }
        }

        internal virtual void ItemsClearInterceptor() { }

        internal virtual void ItemsRemoveInterceptor(GroupItemElement item) { }

        private (int Index, int Column) GetFocusedItemIndexAndColumn()
        {
            var focusedItem = Items[focusedIndex];
            if (focusedItem != null && Items.Any(x => !x.IsDisabled))
            {
                for (var i = 0; i < chunks.Count; i++)
                {
                    if (chunks[i].Any(x => x == focusedItem))
                    {
                        return (chunks[i].IndexOf(focusedItem), i);
                    }
                }
            }

            return (-1, -1);
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
        /// Occurs when the value of the <see cref="Text" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnTextChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<TextAlignment>> OnTextAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderThickness" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Thickness>> OnBorderThicknessChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderColor" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnBorderColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderChars" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<BorderChars>> OnBorderCharsChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Columns" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnColumnsChanged;

        #endregion
    }
}
