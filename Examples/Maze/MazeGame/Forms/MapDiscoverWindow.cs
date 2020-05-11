using System;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements;
using ClForms.Helpers;
using MazeCommon;

namespace MazeGame.Forms
{
    public partial class MapDiscoverWindow: Window
    {
        private readonly Size mapSize;
        private Point cursorPosition;
        private int[,] mazeMap;
        private int[,] fogDiscover;
        private int visibilityArea = 2;
        private int keyCount = 0;

        public MapDiscoverWindow(int[,] source, Point startPoint)
        {
            InitializeComponent();
            cursorPosition = startPoint;
            mapSize = new Size(source.GetLength((int) ArrayDimension.Column),
                source.GetLength((int) ArrayDimension.Row));
            mazeMap = new int[mapSize.Height, mapSize.Width];
            fogDiscover = new int[mapSize.Height, mapSize.Width];
            tilePanel.Width = mapSize.Width;
            tilePanel.Height = mapSize.Height;
            SetVisibility();

            for (var row = 0; row < mapSize.Height; row++)
            {
                for (var col = 0; col < mapSize.Width; col++)
                {
                    mazeMap[row, col] = source[row, col];
                }
            }
        }

        private void CanvasPaint(object sender, PaintEventArgs e)
        {
            for (var row = 0; row < mapSize.Height; row++)
            {
                for (var col = 0; col < mapSize.Width; col++)
                {
                    if (fogDiscover[row, col] == 1)
                    {
                        e.Context.SetCursorPos(col, row);
                        var printingChar = MapItemCharConverter.ConvertToChar((MapItem) mazeMap[row, col]);
                        var colorParams = MapItemColorHelper.GetColorPointForMapItem((MapItem) mazeMap[row, col], false);
                        e.Context.DrawText(printingChar, colorParams.Background, colorParams.Foreground);
                    }
                }
            }
        }

        public void KeyPressedHandler(object sender, KeyPressedEventArgs e)
        {
            switch (e.KeyInfo.Key)
            {
                case ConsoleKey.DownArrow:
                    MoveTo(new Point(cursorPosition.X, cursorPosition.Y + 1));
                    break;
                case ConsoleKey.UpArrow:
                    MoveTo(new Point(cursorPosition.X, cursorPosition.Y - 1));
                    break;
                case ConsoleKey.LeftArrow:
                    MoveTo(new Point(cursorPosition.X - 1, cursorPosition.Y));
                    break;
                case ConsoleKey.RightArrow:
                    MoveTo(new Point(cursorPosition.X + 1, cursorPosition.Y));
                    break;
            }
        }

        private void SetVisibility()
        {
            fogDiscover[cursorPosition.Y, cursorPosition.X] = 1;

            for (var column = Math.Max(0, cursorPosition.Y - visibilityArea); column < Math.Min(mapSize.Height, cursorPosition.Y + visibilityArea + 1); column++)
            {
                fogDiscover[column, cursorPosition.X] = 1;
            }

            for (var row = Math.Max(0, cursorPosition.X - visibilityArea); row < Math.Min(mapSize.Width, cursorPosition.X + visibilityArea + 1); row++)
            {
                fogDiscover[cursorPosition.Y, row] = 1;
            }

            fogDiscover[cursorPosition.Y - 1, cursorPosition.X - 1] = 1;
            fogDiscover[cursorPosition.Y - 1, cursorPosition.X + 1] = 1;
            fogDiscover[cursorPosition.Y + 1, cursorPosition.X - 1] = 1;
            fogDiscover[cursorPosition.Y + 1, cursorPosition.X + 1] = 1;
        }

        private void MoveTo(Point point)
        {
            var targetMazeItem = (MapItem) mazeMap[point.Y, point.X];
            switch (targetMazeItem)
            {
                case MapItem.Door:
                    if (keyCount > 0)
                    {
                        keyCount--;
                        StandTo(point);
                        OnMazeItemDiscovered?.Invoke(this, targetMazeItem);
                    }
                    break;

                case MapItem.Key:
                    keyCount++;
                    StandTo(point);
                    OnMazeItemDiscovered?.Invoke(this, targetMazeItem);
                    break;

                case MapItem.EndPoint:
                case MapItem.TenCoins:
                case MapItem.TwentyCoins:
                    StandTo(point);
                    OnMazeItemDiscovered?.Invoke(this, targetMazeItem);
                    break;

                case MapItem.Empty:
                    StandTo(point);
                    break;
            }
        }

        private void StandTo(Point point)
        {
            mazeMap[cursorPosition.Y, cursorPosition.X] = (int) MapItem.Empty;
            mazeMap[point.Y, point.X] = (int) MapItem.StartPoint;
            OnCursorChanged?.Invoke(this, new PropertyChangedEventArgs<Point>(cursorPosition, point));
            cursorPosition = point;
            SetVisibility();
            tilePanel.InvalidateVisual();
        }

        #region Events

        public event EventHandler<PropertyChangedEventArgs<Point>> OnCursorChanged;

        public event EventHandler<MapItem> OnMazeItemDiscovered;

        #endregion
    }
}
