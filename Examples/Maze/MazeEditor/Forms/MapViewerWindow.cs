using System;
using System.Collections.Generic;
using System.Linq;
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
        private bool pathShowed;

        public MapViewerWindow(int[,] source)
        {
            pathShowed = false;
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
            HidePath();
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
            HidePath();
            if (MazeMap[point.Y, point.X] != (int) value)
            {
                MazeMap[point.Y, point.X] = (int) value;
                InvalidateVisual();
            }
        }

        public bool ShowPath(Point startPoint, Point finishPoint)
        {
            pathShowed = true;
            var currentCell = new Point(startPoint.X, startPoint.Y);
            var cloneMap = (int[,])MazeMap.Clone();
            PrepareMap(mapSize, cloneMap);
            var allCellCount = GetAllAvailableCells(mapSize, cloneMap);
            var visitedCellCount = 1;
            cloneMap[currentCell.Y, currentCell.X] = (int) MapItem.Visited;
            var path = new Stack<Point>();

            while (visitedCellCount < allCellCount)
            {
                var emptyCells = GetNeighbourAvailableCells(mapSize, cloneMap, currentCell).ToArray();
                if (emptyCells.Any())
                {
                    path.Push(currentCell);
                    if (emptyCells.Contains(finishPoint))
                    {
                        break;
                    }
                    var nextCell = emptyCells[0];
                    currentCell = new Point(nextCell.X, nextCell.Y);
                    cloneMap[currentCell.Y, currentCell.X] = (int) MapItem.Visited;
                    visitedCellCount++;
                }
                else if (path.Any())
                {
                    currentCell = path.Pop();
                }
                else
                {
                    return false;
                }
            }

            var prevPoint = new Point(startPoint.X, startPoint.Y);
            foreach (var point in path.Reverse())
            {
                MazeMap[point.Y, point.X] = (int) MapItem.PathShowedDown;
                MazeMap[prevPoint.Y, prevPoint.X] = (int) GetItemDirection(prevPoint, point);
                
                prevPoint = point;
            }
            MazeMap[startPoint.Y, startPoint.X] = (int) MapItem.StartPoint;
            MazeMap[prevPoint.Y, prevPoint.X] = (int) GetItemDirection(prevPoint, finishPoint);
            canvas1.InvalidateVisual();
            return true;
        }

        public void HidePath()
        {
            if (pathShowed)
            {
                pathShowed = false;
                for (var rowIndex = 0; rowIndex < mapSize.Height; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < mapSize.Width; colIndex++)
                    {
                        if (MazeMap[rowIndex, colIndex] < 0)
                        {
                            MazeMap[rowIndex, colIndex] = (int) MapItem.Empty;
                        }
                    }
                }
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

        private static int GetAllAvailableCells(Size mapSize, int[,] mazeMap)
        {
            var result = 0;
            for (var rowIndex = 0; rowIndex < mapSize.Height; rowIndex++)
            {
                for (var colIndex = 0; colIndex < mapSize.Width; colIndex++)
                {
                    if (mazeMap[rowIndex, colIndex] != (int) MapItem.Wall)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private static void PrepareMap(Size mapSize, int[,] cloneMap)
        {
            for (var rowIndex = 0; rowIndex < mapSize.Height; rowIndex++)
            {
                for (var colIndex = 0; colIndex < mapSize.Width; colIndex++)
                {
                    if (cloneMap[rowIndex, colIndex] != (int) MapItem.Wall)
                    {
                        cloneMap[rowIndex, colIndex] = (int) MapItem.Empty;
                    }
                }
            }
        }

        private static IEnumerable<Point> GetNeighbourAvailableCells(Size mapSize, int[,] mazeMap, Point currentCell)
        {
            if (currentCell.Y - 1 > 0 &&
                mazeMap[currentCell.Y - 1, currentCell.X] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X, currentCell.Y - 1);
            }

            if (currentCell.X + 1 < mapSize.Width &&
                mazeMap[currentCell.Y, currentCell.X + 1] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X + 1, currentCell.Y);
            }

            if (currentCell.Y + 1 < mapSize.Height &&
                mazeMap[currentCell.Y + 1, currentCell.X] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X, currentCell.Y + 1);
            }

            if (currentCell.X - 1 > 0 &&
                mazeMap[currentCell.Y, currentCell.X - 1] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X - 1, currentCell.Y);
            }
        }

        private static MapItem GetItemDirection(Point previous, Point next)
        {
            if (previous.X != next.X)
            {
                if (next.X > previous.X)
                {
                    return MapItem.PathShowedRight;
                }

                return MapItem.PathShowedLeft;
            }

            if (next.Y > previous.Y)
            {
                return MapItem.PathShowedDown;
            }

            return MapItem.PathShowedTop;
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
