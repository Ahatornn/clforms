using System;
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

            if (showGridLine)
            {
                var drawingWidth = context.ContextBounds.Width;
                var segmentWidth = drawingWidth / columns;
                var segmentHeight = context.ContextBounds.Height - (!string.IsNullOrWhiteSpace(SummaryText) ? 3 : 0);
                groupBase.OnRender(context);
                if (!string.IsNullOrWhiteSpace(SummaryText))
                {
                    context.SetCursorPos(0, context.ContextBounds.Height - 3);
                    var bottomStr = new StringBuilder(gridBorderChars.LeftInner +
                                                      new string(gridBorderChars.HorizontalInner, drawingWidth - 2) +
                                                      gridBorderChars.RightInner);
                    for (var i = segmentWidth; i < drawingWidth; i += segmentWidth)
                    {
                        if (drawingWidth - i >= segmentWidth)
                        {
                            bottomStr[i] = gridBorderChars.CrossSpanBottomInner;
                        }
                    }
                    context.DrawText(bottomStr.ToString(), BorderColor);
                    context.SetCursorPos(1, context.ContextBounds.Height - 2);
                    context.DrawText(SummaryText.Substring(0, Math.Min(SummaryText.Length, context.ContextBounds.Width - 2)), Background, Foreground);
                }

                for (var rowIndex = 0; rowIndex <= segmentHeight; rowIndex++)
                {
                    var actualSegmentWidth = segmentWidth;
                    for (var colCharIndex = 0; colCharIndex < drawingWidth; colCharIndex += segmentWidth)
                    {
                        if (columns > 1 && colCharIndex + segmentWidth >= drawingWidth)
                        {
                            actualSegmentWidth = (drawingWidth - (columns - 1) * segmentWidth) - 2;
                        }
                        if (drawingWidth - colCharIndex >= segmentWidth)
                        {
                            if (groupBase.TextArea.Contains(colCharIndex, rowIndex))
                            {
                                continue;
                            }

                            if (colCharIndex > 0)
                            {
                                context.SetCursorPos(colCharIndex, rowIndex);
                                context.DrawText(rowIndex == 0
                                    ? gridBorderChars.TopInner
                                    : rowIndex != segmentHeight
                                        ? gridBorderChars.VerticalInner
                                        : gridBorderChars.CrossSpanBottomInner, BorderColor);
                            }

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
                                else
                                {
                                    allHeaderWidth += headerWidth;
                                }

                                if (rowIndex == 1)
                                {
                                    var headerTextIndent =
                                        ((headerWidth - ColumnHeaders[headerIndex].Text.Length) / 2) + 1;
                                    if (headerTextIndent < 0)
                                    {
                                        headerTextIndent = 0;
                                    }

                                    context.SetCursorPos(colCharIndex + (headerIndex * headerWidth) + headerTextIndent,
                                        rowIndex);
                                    context.DrawText(
                                        ColumnHeaders[headerIndex].Text.Substring(0,
                                            Math.Min(ColumnHeaders[headerIndex].Text.Length, headerWidth)),
                                        headerForeground);
                                }

                                if (headerIndex < ColumnHeaders.Count - 1 && rowIndex > 0)
                                {
                                    context.SetCursorPos(colCharIndex + headerWidth, rowIndex);
                                    context.DrawText(rowIndex != segmentHeight
                                        ? gridBorderChars.VerticalInner
                                        : gridBorderChars.CrossSpanBottomInner, BorderColor);
                                }
                            }
                        }
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

        internal void InvalidateVisualIfItemVisible(int index) => throw new NotImplementedException();

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
