using System;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements;
using ClForms.Helpers;
using MazeCommon;
using MazeEditor.Models;

namespace MazeEditor.Forms
{
    public partial class MapViewerWindow: Window
    {
        private readonly Size mapSize;
        private Point cursorPosition;

        public MapViewerWindow(int[,] source)
        {
            InitializeComponent();
            CurrentMapItem = MapItem.Empty;
            cursorPosition = new Point(0, 0);
            mapSize = new Size(source.GetLength((int) ArrayDimension.Column),
                source.GetLength((int) ArrayDimension.Row));
            MazeMap = new int[mapSize.Height, mapSize.Width];
            canvas1.Width = mapSize.Width;
            canvas1.Height = mapSize.Height;

            for (var row = 0; row < mapSize.Height; row++)
            {
                for (var col = 0; col < mapSize.Width; col++)
                {
                    MazeMap[row, col] = source[row, col];
                }
            }
        }

        public MapItem CurrentMapItem { get; set; }

        public int[,] MazeMap { get; }

        public void KeyPressedHandler(object sender, KeyPressedEventArgs e)
        {
            var newPosition = cursorPosition;
            if (e.KeyInfo.Key == ConsoleKey.LeftArrow && cursorPosition.X > 0)
            {
                newPosition.X--;
            }

            if (e.KeyInfo.Key == ConsoleKey.RightArrow && cursorPosition.X < mapSize.Width - 1)
            {
                newPosition.X++;
            }

            if (e.KeyInfo.Key == ConsoleKey.UpArrow && cursorPosition.Y > 0)
            {
                newPosition.Y--;
            }

            if (e.KeyInfo.Key == ConsoleKey.DownArrow && cursorPosition.Y < mapSize.Height - 1)
            {
                newPosition.Y++;
            }

            if (cursorPosition != newPosition)
            {
                OnCursorChanged?.Invoke(this, new PropertyChangedEventArgs<Point>(cursorPosition, newPosition));
                cursorPosition = newPosition;
                canvas1.InvalidateVisual();
            }

            if (e.KeyInfo.Key == ConsoleKey.Spacebar)
            {
                if (MazeMap[cursorPosition.Y, cursorPosition.X] != (int) CurrentMapItem)
                {
                    OnMapChanged?.Invoke(this, new MazeMapItemEventArgs(cursorPosition, CurrentMapItem));
                    MazeMap[cursorPosition.Y, cursorPosition.X] = (int) CurrentMapItem;
                    canvas1.InvalidateVisual();
                }
            }

            if (e.KeyInfo.Key == ConsoleKey.Delete)
            {
                if (MazeMap[cursorPosition.Y, cursorPosition.X] != (int) MapItem.Empty)
                {
                    OnMapChanged?.Invoke(this, new MazeMapItemEventArgs(cursorPosition, CurrentMapItem));
                    MazeMap[cursorPosition.Y, cursorPosition.X] = (int) MapItem.Empty;
                    canvas1.InvalidateVisual();
                }
            }
        }

        public void UpdateMazeItem(Point point, MapItem value)
        {
            if (MazeMap[point.Y, point.X] != (int) value)
            {
                MazeMap[point.Y, point.X] = (int) value;
                InvalidateVisual();
            }
        }

        private void CanvasPaint(object sender, PaintEventArgs e)
        {
            for (var row = 0; row < mapSize.Height; row++)
            {
                for (var col = 0; col < mapSize.Width; col++)
                {
                    e.Context.SetCursorPos(col, row);
                    var printingChar = MapItemCharConverter.ConvertToChar((MapItem) MazeMap[row, col]);
                    var colorParams = MapItemColorHelper.GetColorPointForMapItem((MapItem) MazeMap[row, col],
                        cursorPosition == new Point(col, row));
                    e.Context.DrawText(printingChar, colorParams.Background, colorParams.Foreground);
                }
            }
        }

        #region Events

        /// <summary>
        /// Происходит при изменении значения текущей позиции курсора элемента карты
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<Point>> OnCursorChanged;

        /// <summary>
        /// Происходит при внесении изменений в лабиринт пользователем
        /// </summary>
        public event EventHandler<MazeMapItemEventArgs> OnMapChanged;

        #endregion
    }
}
