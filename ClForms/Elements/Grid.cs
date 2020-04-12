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
        private GridBorderChars gridBorderChars;

        /// <summary>
        /// Initialize a new instance <see cref="Grid"/>
        /// </summary>
        public Grid()
        {
            ColumnDefinitions = new GridColumnDefinitionCollection(this);
            RowDefinitions = new GridRowDefinitionCollection(this);
            borderColor = Application.SystemColors.BorderColor;
            gridInfos = new List<GridInfo>();
            gridBorderChars = new GridBorderChars('┌', '┬', '┐',
                '├', '┼', '┤',
                '└', '┴', '┘',
                '│', '─');
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
        public GridBorderChars BorderChars
        {
            get => gridBorderChars;
            set
            {
                if (gridBorderChars != value)
                {
                    OnBorderCharsChanged?.Invoke(this,
                        new PropertyChangedEventArgs<GridBorderChars>(gridBorderChars, value));
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

            base.Measure(new Size(measureColumns.Sum() + amendment + (Margin + Padding).Horizontal,
                measureRows.Sum() + amendment + (Margin + Padding).Vertical));
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
            var printedFillingRowText = new StringBuilder(reducedArea.Width);
            var topIndent = 0;
            for (var rowIndex = 0; rowIndex < measureRows.Length; rowIndex++)
            {
                printedRowText.Clear();
                printedFillingRowText.Clear();
                for (var columnIndex = 0; columnIndex < measureColumns.Length; columnIndex++)
                {
                    if (columnIndex == 0)
                    {
                        printedRowText.Append(rowIndex == 0 ? gridBorderChars.TopLeft : gridBorderChars.MiddleLeft);
                    }
                    else
                    {
                        printedRowText.Append(rowIndex == 0 ? gridBorderChars.TopMiddle : gridBorderChars.MiddleCenter);
                    }
                    printedRowText.Append(new string(gridBorderChars.HorizontalLine, measureColumns[columnIndex] - 1));

                    printedFillingRowText.Append(gridBorderChars.VerticalLine);
                    printedFillingRowText.Append(new string('\0', measureColumns[columnIndex] - 1));

                    if (columnIndex == measureColumns.Length - 1)
                    {
                        if (printedRowText.Length == reducedArea.Width)
                        {
                            printedRowText[printedRowText.Length - 1] = rowIndex == 0 ? gridBorderChars.TopRight : gridBorderChars.MiddleRight;
                            printedFillingRowText[printedRowText.Length - 1] = gridBorderChars.VerticalLine;
                        }
                        else
                        {
                            printedRowText.Append(rowIndex == 0 ? gridBorderChars.TopRight : gridBorderChars.MiddleRight);
                            printedFillingRowText.Append(gridBorderChars.HorizontalLine);
                        }
                    }
                }
                context.SetCursorPos(Padding.Left, Padding.Top + topIndent);
                context.DrawText(printedRowText.ToString(), Background, borderColor);
                for (var edge = 1; edge < measureRows[rowIndex]; edge++)
                {
                    context.SetCursorPos(Padding.Left, Padding.Top + topIndent + edge);
                    context.DrawText(printedFillingRowText.ToString(), Background, borderColor);
                }
                topIndent += measureRows[rowIndex];
            }

            var printedFillingBottomRowText = new StringBuilder(reducedArea.Width);
            for (var columnIndex = 0; columnIndex < measureColumns.Length; columnIndex++)
            {
                printedFillingBottomRowText.Append(columnIndex == 0 ? gridBorderChars.BottomLeft : gridBorderChars.BottomMiddle);
                printedFillingBottomRowText.Append(new string(gridBorderChars.HorizontalLine, measureColumns[columnIndex] - 1));
                if (columnIndex == measureColumns.Length - 1)
                {
                    if (printedFillingBottomRowText.Length == reducedArea.Width)
                    {
                        printedFillingBottomRowText[printedFillingBottomRowText.Length - 1] = gridBorderChars.BottomRight;
                    }
                    else
                    {
                        printedFillingBottomRowText.Append(gridBorderChars.BottomRight);
                    }
                }
            }

            context.SetCursorPos(Padding.Left, Math.Min(Padding.Top + topIndent, reducedArea.Bottom - 1));
            context.DrawText(printedFillingBottomRowText.ToString(), Background, borderColor);
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<Grid> styleAction) => styleAction?.Invoke(this);

        /// <summary>
        /// Добавляет контент в ячейку таблицы
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
        ///  Get the vertical alignment of specified control
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
        public event EventHandler<PropertyChangedEventArgs<GridBorderChars>> OnBorderCharsChanged;

        #endregion
    }
}
