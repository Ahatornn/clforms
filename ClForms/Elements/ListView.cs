using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
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
        private int columns;
        private Color headerForeground;

        /// <summary>
        /// Initialize a new instance <see cref="ListView"/>
        /// </summary>
        public ListView()
        {
            Background = Color.NotSet;
            Foreground = Color.NotSet;
            headerForeground = Color.Yellow;
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
                    InvalidateMeasure();
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

        #endregion

        /// <inheritdoc cref="Control.Measure"/>
        public override void Measure(Size availableSize)
            => base.Measure(groupBase.Measure(availableSize, (contentSize) => availableSize, true));

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true) =>
            base.Arrange(groupBase.Arrange(finalRect, null, false));

        #region Methods

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension())
            {
                return;
            }

            var drawingWidth = context.ContextBounds.Width - (showGridLine ? 2 : 0);
            var segmentResidue = drawingWidth % columns;
            var segmentHeight = context.ContextBounds.Height - (!string.IsNullOrWhiteSpace(SummaryText) ? 3 : 1);

            if (showGridLine)
            {
                groupBase.OnRender(context);
                var segmentWidth = drawingWidth / columns;
                if (segmentWidth < 1)
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(SummaryText))
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
                    var headersRow = new StringBuilder(actualSegmentWidth  - (isLastBorderVisible ? 0 : headerInfos[^1]));

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
                                    : string.IsNullOrWhiteSpace(summaryText)
                                        ? gridBorderChars.BottomInner
                                        : gridBorderChars.CrossSpanBottomInner, BorderColor);
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
                                context.DrawText(string.IsNullOrWhiteSpace(summaryText)
                                    ? gridBorderChars.BottomInner
                                    : gridBorderChars.CrossSpanBottomInner, BorderColor);
                                headerIndent += headerInfos[headerIndex];
                            }
                        }
                    }

                    segmentIndent += actualSegmentWidth;
                }
            }

            if (!string.IsNullOrWhiteSpace(SummaryText))
            {
                context.SetCursorPos(1, context.ContextBounds.Height - 2);
                context.DrawText(SummaryText.Substring(0, Math.Min(SummaryText.Length, context.ContextBounds.Width - 2)), Background, Foreground);
            }

            /*
            if (ColumnHeaders.Count == 0)
            {
                DrawItemsWithoutHeaders(context, drawingWidth, segmentWidth, segmentHeight);
            }
            else
            {
                //DrawItemsWithHeaders(context, drawingWidth, segmentWidth, segmentHeight);
            }*/
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

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<ListView<T>> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="BaseFocusableControl.InputActionInternal"/>
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {

        }

        internal void InvalidateVisualIfItemVisible(int index) => InvalidateVisual();

        private void DrawItemsWithHeaders(IDrawingContext context, int drawingWidth, int segmentWidth, int segmentHeight)
        {
            var actualSegmentWidth = segmentWidth;
            var segmentIndent = 0;
            for (var colIndex = 0; colIndex < Columns; colIndex++)
            {
                if (colIndex == Columns - 1)
                {
                    actualSegmentWidth = drawingWidth - (columns - 1) * segmentWidth;
                }

                var drawItems = Items.Skip(colIndex * (segmentHeight - 2)).Take(segmentHeight - 2);
                var topIndent = 2;
                foreach (var item in drawItems)
                {
                    context.SetCursorPos(1, topIndent);
                    var defaultHeaderWidth = (actualSegmentWidth - ColumnHeaders.Where(x => x.Width.HasValue)
                                                 .Sum(x => x.Width.Value)) /
                                             ColumnHeaders.Where(x => !x.Width.HasValue)
                                                 .DefaultIfEmpty(new ColumnHeader<T> { Width = 1 }).Count();
                    var allHeaderWidth = 0;
                    for (var headerIndex = 0; headerIndex < ColumnHeaders.Count; headerIndex++)
                    {
                        var headerWidth = ColumnHeaders[headerIndex].Width ?? defaultHeaderWidth;
                        if (headerIndex == ColumnHeaders.Count - 1)
                        {
                            headerWidth = actualSegmentWidth - allHeaderWidth;
                        }

                        var drawingText = ColumnHeaders[headerIndex].DisplayMember != null
                            ? ColumnHeaders[headerIndex].DisplayMember(item)
                            : item.ToString() ?? string.Empty;
                        var cropDrawingText = drawingText.Substring(0, Math.Min(drawingText.Length, headerWidth - 1));

                        context.SetCursorPos(segmentIndent + allHeaderWidth + 1, topIndent);
                        if (drawingText.Length > cropDrawingText.Length)
                        {
                            context.DrawText(cropDrawingText.Substring(0, cropDrawingText.Length - 1) + "…");
                        }
                        else
                        {
                            context.DrawText(cropDrawingText);
                        }

                        allHeaderWidth += headerWidth;
                    }
                    topIndent++;
                }

                segmentIndent += actualSegmentWidth;
            }
        }

        private void DrawItemsWithoutHeaders(IDrawingContext context, int drawingWidth, int segmentWidth, int segmentHeight)
        {
            var actualSegmentWidth = segmentWidth;
            var colIndent = 0;
            for (var colIndex = 0; colIndex < Columns; colIndex ++)
            {
                if (colIndex == Columns - 1)
                {
                    actualSegmentWidth = (drawingWidth - (columns - 1) * segmentWidth) - 2;
                }

                var drawItems = Items.Skip(colIndex * (segmentHeight - 1)).Take(segmentHeight - 1);
                var topIndent = 1;
                foreach (var item in drawItems)
                {
                    context.SetCursorPos(colIndent + 1, topIndent);
                    var drawingText = item?.ToString() ?? string.Empty;
                    var cropDrawingText = drawingText.Substring(0, Math.Min(drawingText.Length, actualSegmentWidth - 1));
                    if (drawingText.Length > cropDrawingText.Length)
                    {
                        context.DrawText(cropDrawingText.Substring(0, cropDrawingText.Length - 1) + "…");
                    }
                    else
                    {
                        context.DrawText(cropDrawingText);
                    }
                    topIndent++;
                }

                colIndent += actualSegmentWidth;
            }
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
        /// Occurs when the value of the <see cref="SummaryText" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<string>> OnSummaryTextChanged;

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

        #endregion
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
