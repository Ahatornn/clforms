using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements;
using ClForms.Themes;
using MazeCommon;
using MazeGame.Forms;

namespace MazeGame
{
    public partial class MainWindow : Window
    {
        private readonly string targetMapFolder;
        private int colCount;
        private int rowCount;
        private bool statusBarWasPrepared = false;
        private Point startPoint;
        private int coins;
        private int keyCount;
        private string currentFileName;
        private int steps;
        private MapDiscoverWindow currentMapWnd;

        public MainWindow()
        {
            InitializeComponent();
            Title = "Maze Game 1.0";
            targetMapFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Maps");
            if (!Directory.Exists(targetMapFolder))
            {
                Directory.CreateDirectory(targetMapFolder);
            }
        }

        private void OpenMenuItemClick(object sender, EventArgs e)
        {
            var dialogWnd = new OpenFileWindow { TargetFolder = targetMapFolder };
            if (dialogWnd.ShowDialog() == DialogResult.OK)
            {
                coins = 0;
                keyCount = 0;
                steps = 0;
                string[] mazeString;
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(dialogWnd.FileName);
                    if (fileName?.StartsWith('<') == true)
                    {
                        var assembly = Assembly.GetExecutingAssembly();
                        var resourceName = assembly.GetManifestResourceNames()
                            .Single(str => str.EndsWith($"{fileName.Substring(2, fileName.Length - 4)}.mmf"));
                        using var stream = assembly.GetManifestResourceStream(resourceName);
                        using var reader = new StreamReader(stream);
                        var resourceAllText = reader.ReadToEnd();

                        mazeString = Regex.Split(resourceAllText, "\r\n|\r|\n");
                    }
                    else
                    {
                        mazeString = File.ReadAllLines(dialogWnd.FileName);
                    }
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
                    }
                }
                var mapWnd = new MapDiscoverWindow(mazeCells, startPoint) { panel1 = { Parent = panel1 } };
                MapDiscoverWindowPrepare(mapWnd);
                PrepareStatusBar(new Size(colCount, rowCount));
                currentFileName = Path.GetFileNameWithoutExtension(dialogWnd.FileName);
                SetWindowTitle(currentFileName);
            }
        }

        private void MapDiscoverWindowPrepare(MapDiscoverWindow targetWnd)
        {
            ForgetMapDiscoverWindow();

            currentMapWnd = targetWnd;
            OnKeyPressed += currentMapWnd.KeyPressedHandler;
            currentMapWnd.OnCursorChanged += CurrentMapWndCursorChanged;
            currentMapWnd.OnMazeItemDiscovered += CurrentMapWndItemDiscovered;
        }

        private void CurrentMapWndItemDiscovered(object sender, MapItem e)
        {
            switch (e)
            {
                case MapItem.TenCoins:
                    coins += 10;
                    UpdateCoins();
                    break;
                case MapItem.TwentyCoins:
                    coins += 20;
                    UpdateCoins();
                    break;
                case MapItem.Key:
                    keyCount++;
                    UpdateLoot();
                    break;
                case MapItem.Door:
                    keyCount--;
                    UpdateLoot();
                    break;
                case MapItem.EndPoint:
                    Application.DoEvents();
                    ForgetMapDiscoverWindow();
                    _ = new FinishWindow(currentFileName, steps, coins) { panel1 = { Parent = panel1 } };
                    ClearStatusBar();
                    break;
            }
        }

        private void ExitMenuItemClick(object sender, EventArgs e) => Close();

        private void PrepareStatusBar(Size size)
        {
            colCount = size.Width;
            rowCount = size.Height;
            statusDimensionLabel.Text = $"{size.Width}x{size.Height}";
            statusPositionLabel.Text = $"X={startPoint.X + 1}, Y={startPoint.Y + 1}";
            UpdateLoot();
            UpdateCoins();
            if (statusBarWasPrepared)
            {
                return;
            }

            statusBarWasPrepared = true;
            statusBar1.AddContent(new GlyphLabel("[↔↕]", "Move") { GlyphForeground = Color.DarkBlue });
            statusBar1.AddContent(statusCoinsLabel);
            statusBar1.AddContent(statusLootLabel);
            statusBar1.AddContent(statusPositionLabel, Dock.Right);
            statusBar1.AddContent(statusDimensionLabel, Dock.Right);
        }

        private void ClearStatusBar()
        {
            var controls = statusBar1.ToArray();
            foreach (var element in controls.Skip(1))
            {
                statusBar1.RemoveContent(element);
            }

            statusBarWasPrepared = false;
        }

        private void SetWindowTitle(string fileName) => Title = $"Maze Game 1.0: {fileName}";

        private void CurrentMapWndCursorChanged(object sender, PropertyChangedEventArgs<Point> e)
        {
            statusPositionLabel.Text = $"X={e.NewValue.X + 1}, Y={e.NewValue.Y + 1}";
            steps++;
        }

        private void UpdateCoins() => statusCoinsLabel.Text = $"Coins: {coins:0000}";

        private void UpdateLoot()
        {
            if (keyCount == 0)
            {
                statusLootLabel.Clear();
            }

            statusLootLabel.Text = new string(MapItemCharConverter.ConvertToChar(MapItem.Key), keyCount);
        }

        private void ForgetMapDiscoverWindow()
        {
            if (currentMapWnd != null)
            {
                OnKeyPressed -= currentMapWnd.KeyPressedHandler;
                currentMapWnd.OnCursorChanged -= CurrentMapWndCursorChanged;
                currentMapWnd.OnMazeItemDiscovered -= CurrentMapWndItemDiscovered;
                currentMapWnd = null;
            }
        }
    }
}
