using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Core.Contexts;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows list view control, which displays a collection of items that can be displayed like a table
    /// </summary>
    /// <typeparam name="T">Type of collection items</typeparam>
    public class ListView<T>: BaseFocusableControl, IElementStyle<ListView<T>>
    {
        private readonly ListViewGroupBase<T> groupBase;
        private bool showGridLine;
        private GridSpanBorderChars gridBorderChars;
        private string summaryText;
        private bool showSummary;
        private int columns;
        private Color headerForeground;
        private ScreenDrawingContext bufferedLinesContext;
        private ScreenDrawingContext bufferedContentContext;
        private int selectedIndex = -1;
        private SelectedRectDescription selectedRectDescription = new SelectedRectDescription();
        private int[] drawingColumnWidths;
        private int segmentHeight = 0;
        private bool wasConstructed = false;
        private Rect groupBaseArrangeFinalRect = Rect.Empty;

        /// <summary>
        /// Initialize a new instance <see cref="ListView"/>
        /// </summary>
        public ListView()
        {
            Background = Color.NotSet;
            Foreground = Color.NotSet;
            headerForeground = Color.Yellow;
            summaryText = string.Empty;
            showSummary = false;
            AutoSize = false;
            showGridLine = true;
            columns = 1;
            FocusForeground = Color.Black;
            FocusBackground = Color.Cyan;
            ColumnHeaders = new ColumnHeaderCollection<T>(this);
            Items = new ListViewItemCollection<T>(this);
            gridBorderChars = new GridSpanBorderChars('╔', '═', '╗', '║', '║',
                '╚', '═', '╝', '╤', '╢', '╧', '╟',
                '│', '─', '┼',
                '┬', '┤', '┴', '├',
                '┌', '┐', '┘', '└');
            groupBase = new ListViewGroupBase<T>(this)
            {
                borderColor = Application.SystemColors.BorderColor,
                borderChars = gridBorderChars,
            };
            OnFocusChanged += InternalFocusChanged;
        }

        #region Properties

        /// <summary>
        /// Gets the collection of all column headers that appear in the control
        /// </summary>
        public ColumnHeaderCollection<T> ColumnHeaders { get; }

        /// <summary>
        /// Gets a collection containing all items in the control
        /// </summary>
        public ListViewItemCollection<T> Items { get; }

        /// <summary>
        /// Gets the index of first visibled item 
        /// </summary>
        public int FirstVisibleItemIndex { get; private set; } = 0;

        /// <summary>
        /// Gets the all visible items
        /// </summary>
        public IEnumerable<T> VisibleItems
        {
            get
            {
                var itemsCount = (segmentHeight - (ColumnHeaders.Any() ? 2 : 1) - 1) * Columns;
                return Items.Skip(FirstVisibleItemIndex).Take(itemsCount);
            }
        }

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
                    if (!groupBaseArrangeFinalRect.HasEmptyDimension())
                    {
                        groupBase.Arrange(groupBaseArrangeFinalRect, null, false);
                    }
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
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region SummaryText

        /// <summary>
        /// Gets or sets the summary text in the bottom of control
        /// </summary>
        public string SummaryText
        {
            get => summaryText;
            set
            {
                if (summaryText != value)
                {
                    OnSummaryTextChanged?.Invoke(this, new PropertyChangedEventArgs<string>(summaryText, value));
                    summaryText = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region ShowSummary

        /// <summary>
        /// Gets or sets a value indicating whether show summary area for <see cref="SummaryText"/>
        /// </summary>
        public bool ShowSummary
        {
            get => showSummary;
            set
            {
                if(showSummary != value)
                {
                    OnShowSummaryChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(showSummary, value));
                    showSummary = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region ShowGridLine

        /// <summary>
        /// Gets or sets a value indicating whether grid lines appear between the columns containing the items in the control
        /// </summary>
        public bool ShowGridLine
        {
            get => showGridLine;
            set
            {
                if (value != showGridLine)
                {
                    OnShowGridLineChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(showGridLine, value));
                    showGridLine = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region BorderChars

        /// <summary>
        /// Gets or sets a value of border chars
        /// </summary>
        public GridSpanBorderChars BorderChars
        {
            get => gridBorderChars;
            set
            {
                if (gridBorderChars != value)
                {
                    OnBorderCharsChanged?.Invoke(this,
                        new PropertyChangedEventArgs<GridSpanBorderChars>(gridBorderChars, value));
                    gridBorderChars = value;
                    groupBase.borderChars = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region BorderColor

        /// <summary>
        /// Gets or sets a value of the grid border
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
                    InvalidateVisual();
                }
            }

        }

        #endregion
        #region HeaderForeground

        /// <summary>
        /// Gets or sets a brush that describes the foreground of a control headers
        /// </summary>
        public Color HeaderForeground
        {
            get => headerForeground;
            set
            {
                if (headerForeground != value)
                {
                    OnHeaderForegroundChanged?.Invoke(this,
                        new PropertyChangedEventArgs<Color>(headerForeground, value));
                    headerForeground = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion
        #region SelectedIndex

        /// <summary>
        /// Gets or sets the index of the selected item in a <see cref="ListView"/> control
        /// </summary>
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if(selectedIndex != value)
                {
                    if(value < -1 || value > Items.Count - 1)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(selectedIndex, value));
                    selectedIndex = value;
                    
                    if (wasConstructed)
                    {
                        ErasePreviousSelectedBackground();
                        var segmentItems = segmentHeight - (ColumnHeaders.Any() ? 2 : 1);
                        if (!(value > FirstVisibleItemIndex && value < FirstVisibleItemIndex + segmentItems))
                        {
                            var fittedColumnItems = value < FirstVisibleItemIndex
                                ? segmentHeight
                                : segmentHeight * Columns;
                            var firstIndex = 0;
                            while (!(value >= firstIndex && value <= firstIndex + fittedColumnItems))
                            {
                                firstIndex += fittedColumnItems;
                            }
                            FirstVisibleItemIndex = firstIndex;
                            InvalidateBufferedContentContext(bufferedContentContext.ContextBounds);
                        }
                        SetSelectedBackground();
                        InvalidateVisual();
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
            => base.Measure(groupBase.Measure(availableSize, (contentSize) => availableSize, true));

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            groupBaseArrangeFinalRect = finalRect;
            var calculatedRect = groupBase.Arrange(groupBaseArrangeFinalRect, null, false);
            bufferedLinesContext = new ScreenDrawingContext(calculatedRect);
            bufferedLinesContext.Release(Color.NotSet, Color.NotSet);
            if (showGridLine)
            {
                CreateGrids(bufferedLinesContext);
            }

            InvalidateBufferedContentContext(calculatedRect);
            if(selectedIndex > -1 && IsFocus)
            {
                SetSelectedBackground();
            }
            base.Arrange(calculatedRect, reduceMargin);
            wasConstructed = true;
        }

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension())
            {
                return;
            }

            if (showGridLine)
            {
                groupBase.OnRender(context);
                ((IDrawingContextDescriptor)context).MergeWith(Point.Empty, bufferedLinesContext, true);
            }

            if (showSummary)
            {
                context.SetCursorPos(1, context.ContextBounds.Height - 2);
                context.DrawText(SummaryText.Substring(0, Math.Min(SummaryText.Length, context.ContextBounds.Width - 2)), Background, Foreground);
            }

            ((IDrawingContextDescriptor)context).MergeWith(Point.Empty, bufferedContentContext, true);
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<ListView<T>> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="BaseFocusableControl.InputActionInternal"/>
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            if (Items.Count == 0)
            {
                return;
            }
            var previousSelectedIndex = selectedIndex;
            var nextSelectedIndex = selectedIndex;
            var previousFirstVisibleItemIndex = FirstVisibleItemIndex;
            var nextFirstVisibleItemIndex = FirstVisibleItemIndex;

            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                if (Items.Count - 1 > selectedIndex)
                {
                    var itemsCount = (segmentHeight - (ColumnHeaders.Any() ? 2 : 1) - 1) * Columns;
                    if ((selectedIndex + 1) - FirstVisibleItemIndex > itemsCount)
                    {
                        nextFirstVisibleItemIndex = FirstVisibleItemIndex + 1;
                    }

                    nextSelectedIndex = selectedIndex + 1;
                }
            }

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (selectedIndex > 0)
                {
                    if (selectedIndex - 1 < FirstVisibleItemIndex)
                    {
                        nextFirstVisibleItemIndex = FirstVisibleItemIndex - 1;
                    }
                    nextSelectedIndex = selectedIndex - 1;
                }
                else
                {
                    nextSelectedIndex = FirstVisibleItemIndex;
                }
            }

            if (keyInfo.Key == ConsoleKey.RightArrow && Columns > 1)
            {
                var segmentItems = segmentHeight - (ColumnHeaders.Any() ? 2 : 1);
                if(selectedIndex == -1)
                {
                    nextSelectedIndex = FirstVisibleItemIndex;
                }
                else if(FirstVisibleItemIndex + (segmentItems * Columns) < Items.Count - 1)
                {
                    nextSelectedIndex = Math.Min(Items.Count - 1, selectedIndex + (segmentHeight - (ColumnHeaders.Any() ? 2 : 1)));
                    if(nextSelectedIndex - FirstVisibleItemIndex >= (segmentItems * Columns))
                    {
                        nextFirstVisibleItemIndex = FirstVisibleItemIndex + segmentItems;
                    }
                }
            }

            if (keyInfo.Key == ConsoleKey.LeftArrow && Columns > 1)
            {
                var segmentItems = segmentHeight - (ColumnHeaders.Any() ? 2 : 1);
                if(selectedIndex == -1)
                {
                    nextSelectedIndex = FirstVisibleItemIndex;
                }
                else if(selectedIndex - segmentItems >= 0)
                {
                    nextSelectedIndex = selectedIndex - segmentItems;
                    if(nextSelectedIndex < FirstVisibleItemIndex)
                    {
                        nextFirstVisibleItemIndex = Math.Max(0, FirstVisibleItemIndex - segmentItems);
                    }
                }
            }

            if (keyInfo.Key == ConsoleKey.Home)
            {
                nextFirstVisibleItemIndex = 0;
                nextSelectedIndex = 0;
            }

            if (keyInfo.Key == ConsoleKey.End)
            {
                var itemsCount = (segmentHeight - (ColumnHeaders.Any() ? 2 : 1)) * Columns;
                nextFirstVisibleItemIndex = Math.Max(0, Items.Count - itemsCount);
                nextSelectedIndex = Items.Count - 1;
            }

            if (keyInfo.Key == ConsoleKey.PageDown)
            {
                var itemsCount = (segmentHeight - (ColumnHeaders.Any() ? 2 : 1)) * Columns;
                if (selectedIndex == -1)
                {
                    nextSelectedIndex = FirstVisibleItemIndex;
                }
                else if (selectedIndex + itemsCount < Items.Count)
                {
                    var currentDiff = selectedIndex - FirstVisibleItemIndex;
                    nextSelectedIndex = selectedIndex + itemsCount;
                    nextFirstVisibleItemIndex = Math.Min(nextSelectedIndex - currentDiff, Items.Count - itemsCount);
                }
            }

            if (keyInfo.Key == ConsoleKey.PageUp)
            {
                var itemsCount = (segmentHeight - (ColumnHeaders.Any() ? 2 : 1)) * Columns;
                if(selectedIndex == -1)
                {
                    nextSelectedIndex = FirstVisibleItemIndex;
                }
                else if (selectedIndex - itemsCount >= 0)
                {
                    var currentDiff = selectedIndex - FirstVisibleItemIndex;
                    nextSelectedIndex = selectedIndex - itemsCount;
                    nextFirstVisibleItemIndex = Math.Max(0, nextSelectedIndex - currentDiff);
                }
            }

            if(previousSelectedIndex != nextSelectedIndex)
            {
                ErasePreviousSelectedBackground();
                if (previousFirstVisibleItemIndex != nextFirstVisibleItemIndex)
                {
                    FirstVisibleItemIndex = nextFirstVisibleItemIndex;
                    InvalidateBufferedContentContext(bufferedContentContext.ContextBounds);
                }
                selectedIndex = nextSelectedIndex;
                OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(previousSelectedIndex, nextSelectedIndex));
                SetSelectedBackground();
                InvalidateVisual();
            }

            if((keyInfo.Key == ConsoleKey.Spacebar || keyInfo.Key == ConsoleKey.Enter) && selectedIndex > -1)
            {
                OnItemClick?.Invoke(this, Items[selectedIndex]);
            }
        }

        internal void InvalidateVisualIfItemVisible(int index, int count, ListViewItemCollectionAction action)
        {
            if (!wasConstructed)
            {
                return;
            }

            var itemsCount = (segmentHeight - (ColumnHeaders.Any() ? 2 : 1)) * Columns;
            if (index > FirstVisibleItemIndex + itemsCount)
            {
                return;
            }
            var oldSelectedIndex = selectedIndex;
            var newSelectedIndex = selectedIndex;

            if (index < FirstVisibleItemIndex)
            {
                FirstVisibleItemIndex += count * (action == ListViewItemCollectionAction.Add ? 1 : -1);
                if(selectedIndex > -1)
                {
                    newSelectedIndex += count * (action == ListViewItemCollectionAction.Add ? 1 : -1);
                }
            }
            else
            {
                if (selectedIndex > index)
                {
                    ErasePreviousSelectedBackground();
                    if(action == ListViewItemCollectionAction.Remove && selectedIndex <= index + count)
                    {
                        newSelectedIndex = -1;
                    }
                    else
                    {
                        newSelectedIndex += count * (action == ListViewItemCollectionAction.Add ? 1 : -1);
                    }
                }
                if(newSelectedIndex > -1 && newSelectedIndex - itemsCount > FirstVisibleItemIndex)
                {
                    FirstVisibleItemIndex = newSelectedIndex - itemsCount;
                }
                InvalidateBufferedContentContext(bufferedContentContext.ContextBounds);
            }

            if (oldSelectedIndex != newSelectedIndex)
            {
                selectedIndex = newSelectedIndex;
                OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(oldSelectedIndex, newSelectedIndex));
            }
            SetSelectedBackground();
            InvalidateVisual();
        }

        private void InternalFocusChanged(object sender, PropertyChangedEventArgs<bool> e)
        {
            if (e.NewValue)
            {
                SetSelectedBackground();
            }
            else
            {
                ErasePreviousSelectedBackground();
            }
        }

        private void SetSelectedBackground()
        {
            if (selectedIndex == -1)
            {
                return;
            }

            var segmentItems = segmentHeight - (ColumnHeaders.Any() ? 2 : 1);
            var columnIndex = (selectedIndex - FirstVisibleItemIndex) / segmentItems;
            var leftIndent = drawingColumnWidths.Take(columnIndex).Sum() + (columnIndex == 0 ? 1 : 2);

            selectedRectDescription.SelectedRect = new Rect(leftIndent, 
                (ColumnHeaders.Any() ? 2 : 1) + (selectedIndex - FirstVisibleItemIndex - segmentItems * columnIndex), 
                drawingColumnWidths[columnIndex] - (columnIndex == 0 ? 0 : 1), 
                1);
            selectedRectDescription.Background = bufferedContentContext.Background[selectedRectDescription.SelectedRect.Location];
            selectedRectDescription.Foreground = bufferedContentContext.Foreground[selectedRectDescription.SelectedRect.Location];
            bufferedContentContext.SetBackgroundValues(selectedRectDescription.SelectedRect, FocusBackground);
            bufferedContentContext.SetForegroundValues(selectedRectDescription.SelectedRect, FocusForeground);
        }

        private void ErasePreviousSelectedBackground()
        {
            if (selectedRectDescription.SelectedRect != Rect.Empty)
            {
                bufferedContentContext.SetBackgroundValues(selectedRectDescription.SelectedRect, selectedRectDescription.Background);
                bufferedContentContext.SetForegroundValues(selectedRectDescription.SelectedRect, selectedRectDescription.Foreground);
                selectedRectDescription.SelectedRect = Rect.Empty;
            }
        }

        internal void ClearItemsInternal()
        {
            if (wasConstructed)
            {
                ErasePreviousSelectedBackground();
                FirstVisibleItemIndex = 0;
                InvalidateBufferedContentContext(bufferedContentContext.ContextBounds);
            }

            if (selectedIndex != -1)
            {
                OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(selectedIndex, -1));
            }
            selectedIndex = -1;
            InvalidateVisual();
        }

        #region Drawing

        private void InvalidateBufferedContentContext(Rect areaRect)
        {
            bufferedContentContext = new ScreenDrawingContext(areaRect);
            bufferedContentContext.Release(Color.NotSet, Color.NotSet);

            if (ColumnHeaders.Count == 0)
            {
                DrawItemsWithoutHeaders(bufferedContentContext);
            }
            else
            {
                DrawItemsWithHeaders(bufferedContentContext);
            }
        }

        private IEnumerable<int> GetHeaderInfo(int actualSegmentWidth)
        {
            var allHeadersWidth = 0;
            if (ColumnHeaders.All(x => x.Width.HasValue))
            {
                foreach (var header in ColumnHeaders)
                {
                    var width = Math.Max((header.Width ?? 0) + (header != ColumnHeaders[^1] ? 1 : 0), 0);
                    if (actualSegmentWidth - allHeadersWidth < width)
                    {
                        width = Math.Max(0, actualSegmentWidth - allHeadersWidth);
                    }
                    yield return width;
                    allHeadersWidth += width;
                    if (allHeadersWidth >= actualSegmentWidth)
                    {
                        yield break;
                    }
                }
            }
            else
            {
                var freeSpace = actualSegmentWidth - ColumnHeaders.Sum(x => (x.Width ?? -1) + 1);
                var dynamicHeaderCount = ColumnHeaders.Count(x => !x.Width.HasValue);
                var freeForOne = freeSpace / dynamicHeaderCount;
                var freeResidue = freeSpace % dynamicHeaderCount;
                var dynamicHeaderIndex = 0;
                foreach (var header in ColumnHeaders)
                {
                    int width;
                    if (header.Width.HasValue)
                    {
                        width = header.Width.Value + 1;
                    }
                    else
                    {
                        width = freeForOne;
                        if (freeResidue != 0 && dynamicHeaderIndex >= dynamicHeaderCount - freeResidue)
                        {
                            width += 1;
                        }
                        width = Math.Max(width, 0);
                        dynamicHeaderIndex++;
                    }

                    if (actualSegmentWidth - allHeadersWidth < width)
                    {
                        width = Math.Max(0, actualSegmentWidth - allHeadersWidth);
                    }
                    allHeadersWidth += width;
                    yield return width;
                    if (allHeadersWidth >= actualSegmentWidth)
                    {
                        yield break;
                    }
                }
            }
        }

        private void CreateGrids(ScreenDrawingContext context)
        {
            var drawingWidth = context.ContextBounds.Width - (showGridLine ? 2 : 0);
            var segmentResidue = drawingWidth % columns;
            segmentHeight = context.ContextBounds.Height - (showSummary ? 3 : 1);

            var segmentWidth = drawingWidth / columns;
            if (segmentWidth < 1)
            {
                return;
            }

            if (showSummary)
            {
                context.SetCursorPos(0, context.ContextBounds.Height - 3);
                var bottomStr = new StringBuilder(gridBorderChars.LeftInner +
                                                  new string(gridBorderChars.HorizontalInner, drawingWidth) +
                                                  gridBorderChars.RightInner);
                context.DrawText(bottomStr.ToString(), BorderColor);
            }

            var segmentIndent = 1;
            for (var column = 0; column < Columns; column++)
            {
                var actualSegmentWidth = drawingWidth / columns;
                if (segmentResidue != 0 && column >= Columns - segmentResidue)
                {
                    actualSegmentWidth++;
                }

                var headerInfos = GetHeaderInfo(actualSegmentWidth).ToList();
                var isLastBorderVisible = headerInfos.Sum() < actualSegmentWidth;
                var headersRow = new StringBuilder(actualSegmentWidth - (isLastBorderVisible ? 0 : headerInfos[^1]));

                for (var headerIndex = 0; headerIndex < headerInfos.Count - 1; headerIndex++)
                {
                    if (headerInfos[headerIndex] > 1)
                    {
                        var formattedString = "{0, " + headerInfos[headerIndex] + "}";
                        headersRow.Append(string.Format(formattedString, gridBorderChars.VerticalInner));
                    }
                }

                if (isLastBorderVisible && headerInfos.Any())
                {
                    var formattedString = "{0, " + headerInfos[^1] + "}";
                    headersRow.Append(string.Format(formattedString, gridBorderChars.VerticalInner));
                }

                for (var rowIndex = 0; rowIndex <= segmentHeight; rowIndex++)
                {
                    if (rowIndex == 0 && groupBase.TextArea.Contains(segmentIndent + actualSegmentWidth, rowIndex))
                    {
                        continue;
                    }

                    if (rowIndex > 0 && rowIndex < segmentHeight)
                    {
                        context.SetCursorPos(segmentIndent + 1, rowIndex);
                        context.DrawText(headersRow.ToString(), BorderColor);
                    }

                    if (rowIndex == 1)
                    {
                        var headerIndent = 0;
                        for (var headerIndex = 0; headerIndex < headerInfos.Count; headerIndex++)
                        {
                            if (headerInfos[headerIndex] > 0)
                            {
                                var headerTextIndent = Math.Max((headerInfos[headerIndex] - ColumnHeaders[headerIndex].Text.Length) / 2, 0);
                                context.SetCursorPos(segmentIndent + headerIndent + headerTextIndent + 1, rowIndex);
                                context.DrawText(ColumnHeaders[headerIndex].Text.Substring(0,
                                    Math.Min(ColumnHeaders[headerIndex].Text.Length, headerInfos[headerIndex] - 1)), headerForeground);
                                headerIndent += headerInfos[headerIndex];
                            }
                        }
                    }

                    if (column < Columns - 1)
                    {
                        context.SetCursorPos(segmentIndent + actualSegmentWidth, rowIndex);
                        context.DrawText(rowIndex == 0
                            ? gridBorderChars.TopInner
                            : rowIndex != segmentHeight
                                ? gridBorderChars.VerticalInner
                                : showSummary
                                    ? gridBorderChars.CrossSpanBottomInner
                                    : gridBorderChars.BottomInner, 
                            BorderColor);
                    }

                    if (rowIndex == segmentHeight)
                    {
                        var headerIndent = 0;
                        for (var headerIndex = 0; headerIndex < headerInfos.Count; headerIndex++)
                        {
                            if (headerInfos[headerIndex] == 0 || headerIndex == headerInfos.Count - 1 && !isLastBorderVisible)
                            {
                                continue;
                            }
                            context.SetCursorPos(segmentIndent + headerInfos[headerIndex] + headerIndent, rowIndex);
                            context.DrawText(showSummary
                                ? gridBorderChars.CrossSpanBottomInner
                                : gridBorderChars.BottomInner,
                                BorderColor);
                            headerIndent += headerInfos[headerIndex];
                        }
                    }
                }

                segmentIndent += actualSegmentWidth;
            }
        }

        private void DrawItemsWithHeaders(ScreenDrawingContext context)
        {
            drawingColumnWidths = new int[Columns];
            var drawingWidth = context.ContextBounds.Width - (showGridLine ? 2 : 0);
            var segmentResidue = drawingWidth % columns;
            segmentHeight = context.ContextBounds.Height - (showSummary ? 3 : 1);

            var segmentWidth = drawingWidth / columns;
            if (segmentWidth < 1)
            {
                drawingColumnWidths = Enumerable.Repeat(0, Columns).ToArray();
                return;
            }
            
            var segmentIndent = 1;
            for (var column = 0; column < Columns; column++)
            {
                var actualSegmentWidth = drawingWidth / columns;
                if (segmentResidue != 0 && column >= Columns - segmentResidue)
                {
                    actualSegmentWidth++;
                }

                var headerInfos = GetHeaderInfo(actualSegmentWidth).ToList();
                var drawItems = Items.Skip(column * (segmentHeight - 2) + FirstVisibleItemIndex).Take(segmentHeight - 2);
                var topIndent = 2;

                foreach (var item in drawItems)
                {
                    var allHeaderWidth = 0;
                    var styleEventArgs = new ListBoxItemStyleEventArgs<T>(item, Color.NotSet, Color.NotSet);
                    OnItemDraw?.Invoke(this, styleEventArgs);

                    for (var headerInfoIndex = 0; headerInfoIndex < headerInfos.Count; headerInfoIndex++)
                    {
                        var leftIndent = segmentIndent + allHeaderWidth + (headerInfoIndex == 0 ? 0 : 1);
                        context.SetCursorPos(leftIndent, topIndent);

                        var drawingEventArgs = new ListBoxItemDrawingEventArgs<T>(item, 
                            styleEventArgs.Background, 
                            styleEventArgs.Foreground,
                            new Rect(leftIndent, topIndent, headerInfos[headerInfoIndex] - 1, 1), 
                            ColumnHeaders[headerInfoIndex], 
                            context);
                        OnItemDrawing?.Invoke(this, drawingEventArgs);
                        if (!drawingEventArgs.Handled)
                        {
                            var drawingText = ColumnHeaders[headerInfoIndex].DisplayMember != null
                                ? ColumnHeaders[headerInfoIndex].DisplayMember(item)
                                : item.ToString() ?? string.Empty;
                            var cropDrawingText = drawingText.Substring(0, Math.Min(drawingText.Length, headerInfos[headerInfoIndex] - 1));
                            if (drawingText.Length > cropDrawingText.Length)
                            {
                                context.DrawText(cropDrawingText.Substring(0, cropDrawingText.Length - 1) + "…", styleEventArgs.Background, styleEventArgs.Foreground);
                            }
                            else
                            {
                                context.DrawText(TextHelper.GetTextWithAlignment(cropDrawingText, headerInfos[headerInfoIndex] - 1, ColumnHeaders[headerInfoIndex].Alignment), styleEventArgs.Background, styleEventArgs.Foreground);
                            }
                        }
                        allHeaderWidth += headerInfos[headerInfoIndex];
                    }

                    topIndent++;
                }

                segmentIndent += actualSegmentWidth + (column == 0 ? 1 : 0);
                drawingColumnWidths[column] = actualSegmentWidth;
            }
        }

        private void DrawItemsWithoutHeaders(ScreenDrawingContext context)
        {
            drawingColumnWidths = new int[Columns];
            var drawingWidth = context.ContextBounds.Width - (showGridLine ? 2 : 0);
            var segmentResidue = drawingWidth % columns;
            segmentHeight = context.ContextBounds.Height - (showSummary ? 3 : 1);

            var segmentWidth = drawingWidth / columns;
            if (segmentWidth < 1)
            {
                drawingColumnWidths = Enumerable.Repeat(0, Columns).ToArray();
                return;
            }

            var segmentIndent = 1;
            for (var column = 0; column < Columns; column++)
            {
                var actualSegmentWidth = drawingWidth / columns;
                if (segmentResidue != 0 && column >= Columns - segmentResidue)
                {
                    actualSegmentWidth++;
                }

                var drawItems = Items.Skip(column * (segmentHeight - 1) + FirstVisibleItemIndex).Take(segmentHeight - 1);
                var topIndent = 1;
                foreach (var item in drawItems)
                {
                    context.SetCursorPos(segmentIndent, topIndent);
                    var styleEventArgs = new ListBoxItemStyleEventArgs<T>(item, Color.NotSet, Color.NotSet);
                    OnItemDraw?.Invoke(this, styleEventArgs);

                    var drawingEventArgs = new ListBoxItemDrawingEventArgs<T>(item,
                            styleEventArgs.Background,
                            styleEventArgs.Foreground,
                            new Rect(segmentIndent, topIndent, actualSegmentWidth - (column == 0 ? 0 : 1), 1),
                            null,
                            context);
                    OnItemDrawing?.Invoke(this, drawingEventArgs);
                    if (!drawingEventArgs.Handled)
                    {
                        var drawingText = item?.ToString() ?? string.Empty;
                        var cropDrawingText = drawingText.Substring(0, Math.Min(drawingText.Length, actualSegmentWidth - (column == 0 ? 0 : 1)));
                        if (drawingText.Length > cropDrawingText.Length)
                        {
                            context.DrawText(cropDrawingText.Substring(0, cropDrawingText.Length - 1) + "…", styleEventArgs.Background, styleEventArgs.Foreground);
                        }
                        else
                        {
                            context.DrawText(cropDrawingText, styleEventArgs.Background, styleEventArgs.Foreground);
                        }
                    }
                    
                    topIndent++;
                }

                segmentIndent += actualSegmentWidth + (column == 0 ? 1 : 0);
                drawingColumnWidths[column] = actualSegmentWidth;
            }
        }

        #endregion

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
        /// Occurs when the value of the <see cref="SummaryText" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnSummaryTextChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ShowSummary" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnShowSummaryChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ShowGridLine" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnShowGridLineChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderChars" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<GridSpanBorderChars>> OnBorderCharsChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderColor" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnBorderColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Columns" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnColumnsChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HeaderForeground" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnHeaderForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnSelectedIndexChanged;

        /// <summary>
        /// Occurs when the any items will draw
        /// </summary>
        public event EventHandler<ListBoxItemStyleEventArgs<T>> OnItemDraw;

        /// <summary>
        /// Occurs when the any items drawing
        /// </summary>
        public event EventHandler<ListBoxItemDrawingEventArgs<T>> OnItemDrawing;

        /// <summary>
        /// Occurs when clicked on selected item
        /// </summary>
        public event EventHandler<T> OnItemClick;

        #endregion

        private struct SelectedRectDescription
        {
            public Rect SelectedRect { get; set; }
            public Color Background { get; set; }
            public Color Foreground { get; set; }
        }
    }

    /// <summary>
    /// Represents a Windows list view control, which displays a collection of items that can be displayed like a table
    /// </summary>
    public class ListView: ListView<object>, IElementStyle<ListView>
    {
        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<ListView> styleAction) => styleAction?.Invoke(this);
    }
}
