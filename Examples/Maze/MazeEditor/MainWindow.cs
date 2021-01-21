using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;
using MazeCommon;
using MazeEditor.Forms;
using MazeEditor.Models;

namespace MazeEditor
{
    public partial class MainWindow: Window
    {
        private bool hasChanged;
        private string fileName;
        private int colCount;
        private int rowCount;
        private bool statusBarWasPrepared = false;
        private Point? startPoint;
        private Point? endPoint;
        private MapViewerWindow currentMapWnd;
        private readonly string targetMapFolder;

        public MainWindow()
        {
            InitializeComponent();
            Title = "Maze map Editor 1.0";
            hasChanged = false;
            colCount = 0;
            rowCount = 0;
            targetMapFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Maps");
            if (!Directory.Exists(targetMapFolder))
            {
                Directory.CreateDirectory(targetMapFolder);
            }
        }

        private void MazeItemClick(object sender, EventArgs e)
        {
            SkipCheckedMazeItem();
            var item = (MenuItem) sender;
            item.Checked = true;
            if (currentMapWnd != null)
            {
                currentMapWnd.CurrentMapItem = (MapItem) item.Tag;
                statusMazeItemLabel.Text = MapItemCharConverter.ConvertToChar(currentMapWnd.CurrentMapItem).ToString();
            }
        }

        private void ExitMenuItemClick(object sender, EventArgs e) => Close();

        private void SaveAsMenuItemClick(object sender, System.EventArgs e) => SaveAsInternal();

        private void SaveMenuItemClick(object sender, System.EventArgs e) => SaveInternal();

        private void OpenMenuItemClick(object sender, System.EventArgs e)
        {
            if (!SaveIfHasChanged())
            {
                return;
            }

            var dialogWnd = new OpenFileWindow { TargetFolder = targetMapFolder };
            if (dialogWnd.ShowDialog() == DialogResult.OK)
            {
                string[] mazeString;
                try
                {
                    mazeString = File.ReadAllLines(dialogWnd.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.ShowError(exception.Message);
                    return;
                }

                rowCount = mazeString.Length;
                colCount = mazeString[0].Length;

                var mazeCells = new int[rowCount, colCount];
                for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < colCount; colIndex++)
                    {
                        var mazeCellValue = Convert.ToInt32(mazeString[rowIndex][colIndex].ToString());
                        mazeCells[rowIndex, colIndex] = mazeCellValue;
                        if (mazeCellValue == (int) MapItem.StartPoint)
                        {
                            startPoint = new Point(colIndex, rowIndex);
                        }

                        if (mazeCellValue == (int) MapItem.EndPoint)
                        {
                            endPoint = new Point(colIndex, rowIndex);
                        }
                    }
                }

                var mapWnd = new MapViewerWindow(mazeCells) { panel1 = { Parent = panel1 } };
                PrepareStatusBar(new Size(colCount, rowCount));
                MapViewerWindowPrepare(mapWnd);
                fileName = Path.GetFileNameWithoutExtension(dialogWnd.FileName);
                SetWindowTitle();
            }
        }

        private void NewMenuItemClick(object sender, System.EventArgs e)
        {
            if (!SaveIfHasChanged())
            {
                return;
            }

            var dialogWnd = new MazeDimensionWindow();
            if (dialogWnd.ShowDialog() == DialogResult.OK)
            {
                startPoint = Point.Empty;
                endPoint = null;
                var mapWnd = new MapViewerWindow(CreateEmptyMaze(dialogWnd.SelectedDimension));
                mapWnd.panel1.Parent = this.panel1;
                PrepareStatusBar(dialogWnd.SelectedDimension);
                MapViewerWindowPrepare(mapWnd);
            }
        }

        private void ClearMazeClick(object sender, EventArgs e)
        {
            if (hasChanged && MessageBox.Show("Clear map", "Do you want to clear current map", MessageBoxIcon.Question, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            var dimension = new Size(colCount, rowCount);
            var mapWnd = new MapViewerWindow(CreateEmptyMaze(dimension)) { panel1 = { Parent = this.panel1 } };
            PrepareStatusBar(dimension);
            MapViewerWindowPrepare(mapWnd);
        }

        private void GenerateClick(object sender, EventArgs e)
        {
            if (!SaveIfHasChanged())
            {
                return;
            }
            var dimension = new Size(colCount, rowCount);
            var mapWnd = new MapViewerWindow(CreateMaze(dimension)) { panel1 = { Parent = this.panel1 } };
            PrepareStatusBar(dimension);
            MapViewerWindowPrepare(mapWnd);
        }

        private void CheckPathClick(object sender, EventArgs e)
        {
            if (!startPoint.HasValue)
            {
                MessageBox.ShowError("Check path", "You have not any entry into Maze. Please, select 'Edit → Start point' and specify target point.");
                return;
            }

            if (!endPoint.HasValue)
            {
                MessageBox.ShowError("Check path", "You have not any exit from Maze. Please, select 'Edit → End point' and specify target point.");
                return;
            }

            if (!currentMapWnd.ShowPath(startPoint.Value, endPoint.Value))
            {
                MessageBox.ShowError("Check path", "You have not any path to exit from Maze. Please, delete some walls and try again.");
            }
        }

        private void CurrentMapChanged(object sender, MazeMapItemEventArgs e)
        {
            hasChanged = true;
            statusSaveLabel.Foreground = Color.DarkYellow;
            if (e.Item == MapItem.StartPoint)
            {
                if (startPoint.HasValue)
                {
                    currentMapWnd.UpdateMazeItem(startPoint.Value, MapItem.Empty);
                }
                startPoint = e.Point;
            }

            if (e.Item == MapItem.EndPoint)
            {
                if (endPoint.HasValue)
                {
                    currentMapWnd.UpdateMazeItem(endPoint.Value, MapItem.Empty);
                }
                endPoint = e.Point;
            }
        }

        private bool SaveIfHasChanged()
        {
            if (hasChanged)
            {
                var messageBoxResult = MessageBox.Show(Consts.SavingHeader,
                    "Do you want to save your changes?",
                    MessageBoxIcon.Question,
                    MessageBoxButtons.YesNoCancel);
                if (messageBoxResult == DialogResult.Cancel)
                {
                    return false;
                }

                if (messageBoxResult == DialogResult.Yes)
                {
                    if (!SaveInternal())
                    {
                        return false;
                    }
                }

                hasChanged = false;
            }

            return true;
        }

        private bool SaveInternal()
        {
            currentMapWnd.HidePath();
            if (string.IsNullOrEmpty(fileName))
            {
                return SaveAsInternal();
            }

            return SaveMazeToFile(fileName);
        }

        private bool SaveAsInternal()
        {
            var dialog = new SaveFileWindow();
            dialog.TargetFolder = targetMapFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
                SetWindowTitle();
                return SaveMazeToFile(dialog.FileName);
            }

            return false;
        }

        private bool SaveMazeToFile(string targetFileName)
        {
            var mazeBuilder = new StringBuilder(rowCount);
            for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (var colIndex = 0; colIndex < colCount; colIndex++)
                {
                    mazeBuilder.Append(currentMapWnd.MazeMap[rowIndex, colIndex]);
                }

                if (rowIndex != rowCount - 1)
                {
                    mazeBuilder.Append(Environment.NewLine);
                }
            }

            try
            {
                File.WriteAllText(Path.Combine(targetMapFolder, $"{targetFileName}{Consts.MazeMapFileExtension}"), mazeBuilder.ToString());
                hasChanged = false;
                statusSaveLabel.Foreground = Color.DarkGray;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.ShowError(e.Message);
                return false;
            }
        }

        private void PrepareStatusBar(Size size)
        {
            colCount = size.Width;
            rowCount = size.Height;
            statusDimensionLabel.Text = $"{size.Width}x{size.Height}";
            statusPositionLabel.Text = "X=1, Y=1";
            statusMazeItemLabel.Text = MapItemCharConverter.ConvertToChar(MapItem.Wall).ToString();
            statusSaveLabel.Foreground = Color.DarkGray;
            if (statusBarWasPrepared)
            {
                return;
            }

            statusBarWasPrepared = true;
            statusBar1.AddContent(new GlyphLabel("[↔↕]", "Move") { GlyphForeground = Color.DarkBlue });
            statusBar1.AddContent(new GlyphLabel("[Space]", "Set") { GlyphForeground = Color.DarkBlue });
            statusBar1.AddContent(new GlyphLabel("[Del]", "Remove") { GlyphForeground = Color.DarkBlue });
            statusBar1.AddContent(statusMazeItemLabel, Dock.Right);
            statusBar1.AddContent(statusPositionLabel, Dock.Right);
            statusBar1.AddContent(statusDimensionLabel, Dock.Right);
            statusBar1.AddContent(statusSaveLabel, Dock.Right);

            SkipCheckedMazeItem();
            wallMenuItem.Checked = true;
            foreach (var item in editMenuItem.Items.Where(x => x.Tag is MapItem))
            {
                item.Enabled = true;
            }

            generateMenuItem.Enabled = true;
            clearMazeMenuItem.Enabled = true;
            checkPathMenuItem.Enabled = true;
        }

        private void CurrentMapWndCursorChanged(object sender, PropertyChangedEventArgs<Point> e)
            => statusPositionLabel.Text = $"X={e.NewValue.X + 1}, Y={e.NewValue.Y + 1}";

        private void SkipCheckedMazeItem()
        {
            foreach (var item in editMenuItem.Items.Where(x => x.Tag is MapItem && x is MenuItem).Cast<MenuItem>())
            {
                item.Checked = false;
            }
        }

        private void SetWindowTitle() => Title = $"Maze map Editor 1.0: {fileName}";

        private void SetSaveEnabled()
        {
            saveMenuItem.Enabled = true;
            saveAsMenuItem.Enabled = true;
        }

        private void MapViewerWindowPrepare(MapViewerWindow targetWnd)
        {
            if (currentMapWnd != null)
            {
                OnKeyPressed -= currentMapWnd.KeyPressedHandler;
                currentMapWnd.OnCursorChanged -= CurrentMapWndCursorChanged;
                currentMapWnd.OnMapChanged -= CurrentMapChanged;
            }
            currentMapWnd = targetWnd;
            currentMapWnd.CurrentMapItem = MapItem.Wall;
            OnKeyPressed += currentMapWnd.KeyPressedHandler;
            currentMapWnd.OnCursorChanged += CurrentMapWndCursorChanged;
            currentMapWnd.OnMapChanged += CurrentMapChanged;
            SetSaveEnabled();
        }

        private static int[,] CreateEmptyMaze(Size size)
        {
            var mazeCells = new int[size.Height, size.Width];
            for (var rowIndex = 0; rowIndex < size.Height; rowIndex++)
            {
                for (var colIndex = 0; colIndex < size.Width; colIndex++)
                {
                    mazeCells[rowIndex, colIndex] = (int) MapItem.Empty;
                    if (rowIndex == 0 ||
                        rowIndex == size.Height - 1 ||
                        colIndex == 0 ||
                        colIndex == size.Width - 1)
                    {
                        mazeCells[rowIndex, colIndex] = (int) MapItem.Wall;
                    }
                }
            }

            return mazeCells;
        }

        private int[,] CreateMaze(Size size)
        {
            var mazeCells = new int[size.Height, size.Width];
            for (var rowIndex = 0; rowIndex < size.Height; rowIndex++)
            {
                for (var colIndex = 0; colIndex < size.Width; colIndex++)
                {
                    mazeCells[rowIndex, colIndex] = (int) MapItem.Empty;
                    if (rowIndex == 0 ||
                        rowIndex == size.Height - 1 ||
                        colIndex == 0 ||
                        colIndex == size.Width - 1 ||
                        rowIndex % 2 == 0 ||
                        colIndex % 2 == 0)
                    {
                        mazeCells[rowIndex, colIndex] = (int) MapItem.Wall;
                    }
                }
            }

            var allCellCount = ((size.Width - 1) / 2) * ((size.Height - 1) / 2);
            var visitedCellCount = 1;

            var currentCell = new Point(1, 1);
            startPoint = new Point(currentCell.X, currentCell.Y);
            mazeCells[currentCell.Y, currentCell.X] = (int) MapItem.StartPoint;
            var path = new Stack<Point>();
            var rndGenerator = new Random(DateTime.Now.Millisecond);

            while (visitedCellCount < allCellCount)
            {
                var emptyCells = GetNeighbourEmptyCells(size, mazeCells, currentCell).ToArray();
                if (emptyCells.Length > 0)
                {
                    path.Push(currentCell);
                    var nextCell = emptyCells[0];
                    if (emptyCells.Length > 1)
                    {
                        nextCell = emptyCells[rndGenerator.Next(emptyCells.Length)];
                    }

                    var wallForBreakX = (currentCell.X + nextCell.X) / 2;
                    var wallForBreakY = (currentCell.Y + nextCell.Y) / 2;
                    mazeCells[wallForBreakY, wallForBreakX] = (int) MapItem.Empty;

                    currentCell = new Point(nextCell.X, nextCell.Y);
                    mazeCells[currentCell.Y, currentCell.X] = (int) MapItem.Visited;

                    visitedCellCount++;
                }
                else if(path.Any())
                {
                    currentCell = path.Pop();
                }
                else if(TryGetEmptyCell(size, mazeCells, ref currentCell))
                {
                    mazeCells[currentCell.Y, currentCell.X] = (int) MapItem.Visited;
                }
                else
                {
                    break;
                }
            }

            for (var rowIndex = 0; rowIndex < size.Height; rowIndex++)
            {
                for (var colIndex = 0; colIndex < size.Width; colIndex++)
                {
                    if(mazeCells[rowIndex, colIndex] == (int) MapItem.Visited)
                    {
                        mazeCells[rowIndex, colIndex] = (int) MapItem.Empty;
                    }
                }
            }
            return mazeCells;
        }

        private static bool TryGetEmptyCell(Size size, int[,] mazeCells, ref Point currentCell)
        {
            for (var rowIndex = 0; rowIndex < size.Height; rowIndex++)
            {
                for (var colIndex = 0; colIndex < size.Width; colIndex++)
                {
                    if(mazeCells[rowIndex, colIndex] == (int) MapItem.Empty)
                    {
                        currentCell = new Point(colIndex, rowIndex);
                        return true;
                    }
                }
            }

            return false;
        }

        private static IEnumerable<Point> GetNeighbourEmptyCells(Size dimension, int[,] mazeCells, Point currentCell)
        {
            if (currentCell.Y - 2 > 0 &&
                mazeCells[currentCell.Y - 2, currentCell.X] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X, currentCell.Y - 2);
            }

            if (currentCell.X + 2 < dimension.Width &&
                mazeCells[currentCell.Y, currentCell.X + 2] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X + 2, currentCell.Y);
            }

            if (currentCell.Y + 2 < dimension.Height &&
                mazeCells[currentCell.Y + 2, currentCell.X] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X, currentCell.Y + 2);
            }

            if (currentCell.X - 2 > 0 &&
                mazeCells[currentCell.Y, currentCell.X - 2] == (int) MapItem.Empty)
            {
                yield return new Point(currentCell.X - 2, currentCell.Y);
            }
        }
    }
}
