using System;

namespace ClForms.Common.Grid
{
    internal class GridInfo
    {
        private const string GreaterOrEqualZero = "{0}'s value must be greater than zero or equal";
        private const string GreaterOrEqualOne = "{0}'s value must be greater than one or equal";

        private int column;
        private int row;
        private int columnSpan;
        private int rowSpan;

        public long Id { get; set; }

        #region Column

        public int Column
        {
            get => column;
            set
            {
                if (column != value)
                {
                    if (value < 0)
                    {
                        throw new InvalidOperationException(string.Format(GreaterOrEqualZero, nameof(Column)));
                    }

                    column = value;
                }
            }
        }

        #endregion
        #region Row

        public int Row
        {
            get => row;
            set
            {
                if (row != value)
                {
                    if (value < 0)
                    {
                        throw new InvalidOperationException(string.Format(GreaterOrEqualZero, nameof(Row)));
                    }

                    row = value;
                }
            }
        }

        #endregion
        #region ColumnSpan

        public int ColumnSpan
        {
            get => columnSpan;
            set
            {
                if (columnSpan != value)
                {
                    if (value < 1)
                    {
                        throw new InvalidOperationException(string.Format(GreaterOrEqualOne, nameof(ColumnSpan)));
                    }

                    columnSpan = value;
                }
            }
        }

        #endregion
        #region RowSpan

        public int RowSpan
        {
            get => rowSpan;
            set
            {
                if (rowSpan != value)
                {
                    if (value < 1)
                    {
                        throw new InvalidOperationException(string.Format(GreaterOrEqualOne, nameof(RowSpan)));
                    }

                    rowSpan = value;
                }
            }
        }

        #endregion

        public HorizontalAlignment HorizontalAlignment { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }

        public GridInfo(long id)
        {
            Id = id;
            column = 0;
            row = 0;
            columnSpan = 1;
            rowSpan = 1;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
