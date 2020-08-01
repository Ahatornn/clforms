using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Common.Grid;
using ClForms.Common.Grid.Abstractions;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows
    /// </summary>
    public class Grid: MultipleContentControl, IElementStyle<Grid>
    {
        private bool showGridLine;
        private Color borderColor;
        private int[] measureColumns;
        private int[] measureRows;
        private readonly List<GridInfo> gridInfos;
        private GridSpanBorderChars gridBorderChars;

        /// <summary>
        /// Initialize a new instance <see cref="Grid"/>
        /// </summary>
        public Grid()
        {
            ColumnDefinitions = new GridColumnDefinitionCollection(this);
            RowDefinitions = new GridRowDefinitionCollection(this);
            borderColor = Application.SystemColors.BorderColor;
            gridInfos = new List<GridInfo>();
            gridBorderChars = new GridSpanBorderChars('┌', '─', '┐', '│', '│',
                '└', '─', '┘', '┬', '┤', '┴', '├',
                '│', '─', '┼',
                '┬', '┤', '┴', '├',
                '┌', '┐', '┘', '└');
        }

        #region Properties

        /// <summary>
        /// Gets a <see cref="GridColumnDefinitionCollection"/> defined on this instance of <see cref="Grid"/>
        /// </summary>
        public GridColumnDefinitionCollection ColumnDefinitions { get; }

        /// <summary>
        /// Gets a <see cref="GridRowDefinitionCollection"/> defined on this instance of <see cref="Grid"/>
        /// </summary>
        public GridRowDefinitionCollection RowDefinitions { get; }

        /// <summary>
        /// Get of the row count
        /// </summary>
        public int RowCount => RowDefinitions.Count;

        /// <summary>
        /// Get of the column count
        /// </summary>
        public int ColumnCount => ColumnDefinitions.Count;

        #region ShowGridLine

        /// <summary>
        /// Gets or sets a value that indicates whether grid lines are visible within this <see cref="Grid"/>
        /// </summary>
        public bool ShowGridLine
        {
            get => showGridLine;
            set
            {
                if (showGridLine != value)
                {
                    OnShowGridLineChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(showGridLine, value));
                    showGridLine = value;
                    InvalidateMeasure();
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
            get => borderColor;
            set
            {
                if (borderColor != value)
                {
                    OnBorderColorChanged?.Invoke(this, new PropertyChangedEventArgs<Color>(borderColor, value));
                    borderColor = value;
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
                    InvalidateVisual();
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
            var amendment = showGridLine ? 1 : 0;

            PrepareDimension(desiredContentSize.Width - amendment,
                out measureColumns,
                ColumnDefinitions.ToArray());
            PrepareDimension(desiredContentSize.Height - amendment,
                out measureRows,
                RowDefinitions.ToArray());
            foreach (var control in this)
            {
                var info = gridInfos.First(x => x.Id == control.Id);
                control.Measure(new Size(
                    measureColumns.Skip(info.Column).Take(info.ColumnSpan).Sum() - amendment,
                    measureRows.Skip(info.Row).Take(info.RowSpan).Sum() - amendment));
            }

            var result = new Size(measureColumns.Sum() + amendment + (Margin + Padding).Horizontal,
                measureRows.Sum() + amendment + (Margin + Padding).Vertical);

            base.Measure(result);
        }

        /// <inheritdoc cref="Control.Arrange"/>
        public override void Arrange(Rect finalRect, bool reduceMargin = true)
        {
            var amendment = showGridLine ? 1 : 0;
            var topIndent = 0;
            for (var row = 0; row < RowCount; row++)
            {
                var leftIndent = 0;
                for (var col = 0; col < ColumnCount; col++)
                {
                    var colIndex = col;
                    var rowIndex = row;
                    var arrangeControls = this.Where(x => gridInfos
                        .Where(y => y.Column == colIndex && y.Row == rowIndex)
                        .Select(y => y.Id)
                        .Contains(x.Id));
                    foreach (var control in arrangeControls)
                    {
                        var controlInfo = gridInfos.First(x => x.Id == control.Id);
                        var controlAreaWidth = measureColumns.Skip(col).Take(controlInfo.ColumnSpan).Sum() - amendment;
                        var controlAreaHeight = measureRows.Skip(row).Take(controlInfo.RowSpan).Sum() - amendment;
                        var controlLeftIndent = 0;
                        var controlTopIndent = 0;
                        if (controlInfo.HorizontalAlignment == HorizontalAlignment.Center)
                        {
                            controlLeftIndent = (controlAreaWidth - control.DesiredSize.Width) / 2;
                        }

                        if (controlInfo.HorizontalAlignment == HorizontalAlignment.Right)
                        {
                            controlLeftIndent = controlAreaWidth - control.DesiredSize.Width;
                        }

                        if (controlInfo.VerticalAlignment == VerticalAlignment.Center)
                        {
                            controlTopIndent = (controlAreaHeight - control.DesiredSize.Height) / 2;
                        }

                        if (controlInfo.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            controlTopIndent = controlAreaHeight - control.DesiredSize.Height;
                        }

                        control.Arrange(new Rect(Padding.Left + leftIndent + controlLeftIndent + amendment,
                            Padding.Top + topIndent + controlTopIndent + amendment,
                            Math.Min(controlAreaWidth, control.DesiredSize.Width),
                            Math.Min(controlAreaHeight, control.DesiredSize.Height)));
                    }

                    leftIndent += measureColumns[col];
                }

                topIndent += measureRows[row];
            }
            base.Arrange(finalRect);
        }

        /// <inheritdoc cref="Control.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);
            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (reducedArea.HasEmptyDimension() || !showGridLine)
            {
                return;
            }

            var printedRowText = new StringBuilder(reducedArea.Width);
            var printedFillText = new StringBuilder(reducedArea.Width);
            var topIndent = 0;
            for (var rowIndex = 0; rowIndex <= measureRows.Length; rowIndex++)
            {
                printedRowText.Clear();
                printedFillText.Clear();
                printedFillText.Append(new string(' ', reducedArea.Width));
                for (var columnIndex = 0; columnIndex < measureColumns.Length; columnIndex++)
                {
                    var printedColText = new StringBuilder(measureColumns[columnIndex] + (columnIndex == measureColumns.Length - 1 ? 1 : 0));

                    printedColText.Append(new string(rowIndex == 0
                        ? gridBorderChars.TopMiddle
                        : rowIndex == measureRows.Length
                            ? gridBorderChars.BottomMiddle
                            : GetHorizontalInnerChar(columnIndex, rowIndex),
                        measureColumns[columnIndex] + (columnIndex == measureColumns.Length - 1 ? 1 : 0)));

                    var columnStartIndex = measureColumns.Take(columnIndex).DefaultIfEmpty(0).Sum();
                    if (!gridInfos.Any(x => x.IsHorizontalCross(columnIndex, rowIndex)))
                    {
                        printedFillText[columnStartIndex] = gridBorderChars.VerticalInner;
                    }

                    printedColText[0] = rowIndex == 0
                        ? GetTopInnerChar(columnIndex)
                        : rowIndex == measureRows.Length
                            ? GetBottomInnerChar(columnIndex)
                            : GetCrossInnerChar(columnIndex, rowIndex);

                    if (columnIndex == 0)
                    {
                        printedColText[0] = rowIndex == 0
                            ? gridBorderChars.TopLeft
                            : rowIndex == measureRows.Length
                                ? gridBorderChars.BottomLeft
                                : GetLeftInnerChar(rowIndex);

                        printedFillText[columnStartIndex] = gridBorderChars.MiddleLeft;
                    }
                    else if (columnIndex == measureColumns.Length - 1)
                    {
                        printedColText[^1] = rowIndex == 0
                            ? gridBorderChars.TopRight
                            : rowIndex == measureRows.Length
                                ? gridBorderChars.BottomRight
                                : GetRightInnerChar(rowIndex);
                        printedFillText[^1] = gridBorderChars.MiddleRight;
                    }

                    printedRowText.Append(printedColText.ToString());
                }

                context.SetCursorPos(Padding.Left, Padding.Top + topIndent);
                context.DrawText(printedRowText.ToString(), Background, borderColor);
                if (rowIndex < measureRows.Length)
                {
                    for (var rowFilling = 1; rowFilling < measureRows[rowIndex]; rowFilling++)
                    {
                        context.SetCursorPos(Padding.Left, Padding.Top + topIndent + rowFilling);
                        context.DrawText(printedFillText.ToString(), Background, borderColor);
                    }
                    topIndent += measureRows[rowIndex];
                }
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Grid> styleAction) => styleAction?.Invoke(this);

        /// <summary>
        /// Adds content to a table cell
        /// </summary>
        public virtual void AddContent(Control content, int column, int row, int columnSpan = 1, int rowSpan = 1,
            HorizontalAlignment horizontal = HorizontalAlignment.Left, VerticalAlignment vertical = VerticalAlignment.Top)
        {
            gridInfos.Add(new GridInfo(content.Id)
            {
                Column = column,
                Row = row,
                ColumnSpan = columnSpan,
                RowSpan = rowSpan,
                HorizontalAlignment = horizontal,
                VerticalAlignment = vertical,
            });
            AddContent(content);
        }

        /// <inheritdoc cref="MultipleContentControl.AddContentInterceptor"/>
        protected override void AddContentInterceptor(Control content)
        {
            if (gridInfos.All(x => x.Id != content.Id))
            {
                gridInfos.Add(new GridInfo(content.Id));
            }
        }

        private void PrepareDimension(int size, out int[] target, IList<GridLayoutDefinition> collections)
        {
            target = new int[collections.Count()];
            foreach (var definition in collections.Where(x => x.SizeType == SizeType.Absolute))
            {
                target[collections.IndexOf(definition)] = definition.Size;
            }
            var processedWidth = collections.Where(x => x.SizeType == SizeType.Absolute).Sum(x => x.Size);
            foreach (var definition in collections.Where(x => x.SizeType == SizeType.Percent))
            {
                target[collections.IndexOf(definition)] =
                    (size - processedWidth) * definition.Size / 100;
            }

            processedWidth = target.Sum();
            var otherColumns = collections
                .Where(x => x.SizeType == SizeType.AutoSize)
                .ToList();
            foreach (var definition in otherColumns)
            {
                target[collections.IndexOf(definition)] =
                    (size - processedWidth) / otherColumns.Count();
            }
        }

        #region GetBorderChars

        private char GetTopInnerChar(int columnIndex)
        {
            if (gridInfos.Any(x => x.IsHorizontalCross(columnIndex, 0)))
            {
                return gridBorderChars.TopMiddle;
            }
            return gridBorderChars.TopInner;
        }

        private char GetBottomInnerChar(int columnIndex)
        {
            if (gridInfos.Any(x => x.IsHorizontalCross(columnIndex, ColumnCount - 1)))
            {
                return gridBorderChars.BottomMiddle;
            }
            return gridBorderChars.BottomInner;
        }

        private char GetHorizontalInnerChar(int columnIndex, int rowIndex)
        {
            if (gridInfos.Any(x => x.IsVerticalCross(rowIndex, columnIndex)))
            {
                return '\0';
            }

            return gridBorderChars.HorizontalInner;
        }

        private char GetLeftInnerChar(int rowIndex)
        {
            if (gridInfos.Any(x => x.IsVerticalCross(rowIndex, 0)))
            {
                return gridBorderChars.MiddleLeft;
            }

            return gridBorderChars.LeftInner;
        }

        private char GetRightInnerChar(int rowIndex)
        {
            if (gridInfos.Any(x => x.IsVerticalCross(rowIndex, ColumnCount - 1)))
            {
                return gridBorderChars.MiddleRight;
            }

            return gridBorderChars.RightInner;
        }

        private char GetCrossInnerChar(int columnIndex, int rowIndex)
        {
            var anyPreHValue = gridInfos.Any(x => x.IsHorizontalCross(columnIndex, rowIndex - 1));
            var anyCurHValue = gridInfos.Any(x => x.IsHorizontalCross(columnIndex, rowIndex));
            var anyPreVValue = gridInfos.Any(x => x.IsVerticalCross(rowIndex, columnIndex - 1));
            var anyCurVValue = gridInfos.Any(x => x.IsVerticalCross(rowIndex, columnIndex));

            if (anyPreHValue && anyCurHValue && anyPreVValue && anyCurVValue)
            {
                return '\0';
            }

            if ((anyPreHValue || anyCurHValue) && anyPreVValue && anyCurVValue)
            {
                return gridBorderChars.VerticalInner;
            }

            if (anyPreHValue && anyCurHValue && (anyPreVValue || anyCurVValue))
            {
                return gridBorderChars.HorizontalInner;
            }

            if (anyPreHValue && anyCurVValue)
            {
                return gridBorderChars.CrossSpanCornerRightInner;
            }

            if (anyPreHValue && anyPreVValue)
            {
                return gridBorderChars.CrossSpanCornerTopInner;
            }

            if (anyCurHValue && anyCurVValue)
            {
                return gridBorderChars.CrossSpanCornerBottomInner;
            }

            if (anyCurHValue && anyPreVValue)
            {
                return gridBorderChars.CrossSpanCornerLeftInner;
            }

            if (anyPreHValue && anyCurHValue)
            {
                return gridBorderChars.HorizontalInner;
            }

            if (anyPreHValue && !anyCurHValue)
            {
                return gridBorderChars.CrossSpanTopInner;
            }

            if (!anyPreHValue && anyCurHValue)
            {
                return gridBorderChars.CrossSpanBottomInner;
            }

            if (anyPreVValue && anyCurVValue)
            {
                return gridBorderChars.VerticalInner;
            }

            if (anyPreVValue && !anyCurVValue)
            {
                return gridBorderChars.CrossSpanLeftInner;
            }

            if (!anyPreVValue && anyCurVValue)
            {
                return gridBorderChars.CrossSpanRightInner;
            }

            return gridBorderChars.CrossInner;
        }

        #endregion

        #endregion

        #region Cell placement methods

        /// <summary>
        /// Set column for specified control
        /// </summary>
        public void SetColumn(Control content, int column)
        {
            if (column >= ColumnDefinitions.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            gridInfos.First(x => x.Id == content.Id).Column = column;
            InvalidateMeasure();
        }

        /// <summary>
        /// Get the column index of specified control
        /// </summary>
        public int GetColumn(Control content) => gridInfos.First(x => x.Id == content.Id).Column;

        /// <summary>
        /// Set row for specified control
        /// </summary>
        public void SetRow(Control content, int row)
        {
            if (row >= RowDefinitions.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            gridInfos.First(x => x.Id == content.Id).Row = row;
            InvalidateMeasure();
        }

        /// <summary>
        /// Get the row of specified control
        /// </summary>
        public int GetRow(Control content) => gridInfos.First(x => x.Id == content.Id).Row;

        /// <summary>
        /// Set column span for specified control
        /// </summary>
        public void SetColumnSpan(Control content, int columnSpan)
        {
            gridInfos.First(x => x.Id == content.Id).ColumnSpan = columnSpan;
            InvalidateMeasure();
        }

        /// <summary>
        /// Get the column span of specified control
        /// </summary>
        public int GetColumnSpan(Control content) => gridInfos.First(x => x.Id == content.Id).ColumnSpan;

        /// <summary>
        /// Set row span for specified control
        /// </summary>
        public void SetRowSpan(Control content, int rowSpan)
        {
            gridInfos.First(x => x.Id == content.Id).RowSpan = rowSpan;
            InvalidateMeasure();
        }

        /// <summary>
        /// Get the row span of specified control
        /// </summary>
        public int GetRowSpan(Control content) => gridInfos.First(x => x.Id == content.Id).RowSpan;

        /// <summary>
        /// Set horizontal alignment for specified control
        /// </summary>
        public void SetHorizontalAlignment(Control content, HorizontalAlignment horizontalAlignment)
        {
            gridInfos.First(x => x.Id == content.Id).HorizontalAlignment = horizontalAlignment;
            InvalidateMeasure();
        }

        /// <summary>
        /// Get the horizontal alignment of specified control
        /// </summary>
        public HorizontalAlignment GetHorizontalAlignment(Control content)
            => gridInfos.First(x => x.Id == content.Id).HorizontalAlignment;

        /// <summary>
        /// Set vertical alignment for specified control
        /// </summary>
        public void SetVerticalAlignment(Control content, VerticalAlignment verticalAlignment)
        {
            gridInfos.First(x => x.Id == content.Id).VerticalAlignment = verticalAlignment;
            InvalidateMeasure();
        }

        /// <summary>
        /// Get the vertical alignment of specified control
        /// </summary>
        public VerticalAlignment GetVerticalAlignment(Control content)
            => gridInfos.First(x => x.Id == content.Id).VerticalAlignment;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="ShowGridLine" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<bool>> OnShowGridLineChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderColor" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Color>> OnBorderColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderChars" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<GridSpanBorderChars>> OnBorderCharsChanged;

        #endregion
    }
}
