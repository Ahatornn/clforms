using System;
using ClForms.Common;
using ClForms.Core;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;
using MazeCommon;

namespace MazeEditor
{
    public partial class MainWindow
    {
        private void InitializeComponent()
        {
            WindowState = ControlState.Maximized;
            Background = Color.Black;

            mainMenu1 = new MainMenu();
            fileMenuItem = new MenuItem("File");
            newMenuItem = new MenuItem("&New");
            newMenuItem.OnClick += NewMenuItemClick;
            fileMenuItem.Items.Add(newMenuItem);

            openMenuItem = new MenuItem("&Open…");
            openMenuItem.OnClick += OpenMenuItemClick;
            fileMenuItem.Items.Add(openMenuItem);

            fileMenuItem.Items.Add(new SeparatorMenuItem());

            saveMenuItem = new MenuItem("&Save", null, enabled: false);
            saveMenuItem.OnClick += SaveMenuItemClick;
            fileMenuItem.Items.Add(saveMenuItem);

            saveAsMenuItem = new MenuItem("Save &as…", null, enabled: false);
            saveAsMenuItem.OnClick += SaveAsMenuItemClick;
            fileMenuItem.Items.Add(saveAsMenuItem);

            fileMenuItem.Items.Add(new SeparatorMenuItem());

            exitMenuItem = new MenuItem("E&xit");
            exitMenuItem.OnClick += ExitMenuItemClick;
            fileMenuItem.Items.Add(exitMenuItem);

            mainMenu1.Items.Add(fileMenuItem);

            editMenuItem = new MenuItem("Edit");
            wallMenuItem = new MenuItem("Wall", new Shortcut(ConsoleKey.D1), enabled: false);
            wallMenuItem.OnClick += MazeItemClick;
            wallMenuItem.Tag = MapItem.Wall;
            editMenuItem.Items.Add(wallMenuItem);

            startMenuItem = new MenuItem("Start point", new Shortcut(ConsoleKey.D2), enabled: false);
            startMenuItem.OnClick += MazeItemClick;
            startMenuItem.Tag = MapItem.StartPoint;
            editMenuItem.Items.Add(startMenuItem);

            endMenuItem = new MenuItem("End point", new Shortcut(ConsoleKey.D3), enabled: false);
            endMenuItem.OnClick += MazeItemClick;
            endMenuItem.Tag = MapItem.EndPoint;
            editMenuItem.Items.Add(endMenuItem);

            doorMenuItem = new MenuItem("Door", new Shortcut(ConsoleKey.D4), enabled: false);
            doorMenuItem.OnClick += MazeItemClick;
            doorMenuItem.Tag = MapItem.Door;
            editMenuItem.Items.Add(doorMenuItem);

            keyMenuItem = new MenuItem("Key", new Shortcut(ConsoleKey.D5), enabled: false);
            keyMenuItem.OnClick += MazeItemClick;
            keyMenuItem.Tag = MapItem.Key;
            editMenuItem.Items.Add(keyMenuItem);

            tenCoinsMenuItem = new MenuItem("10 coins", new Shortcut(ConsoleKey.D6), enabled: false);
            tenCoinsMenuItem.OnClick += MazeItemClick;
            tenCoinsMenuItem.Tag = MapItem.TenCoins;
            editMenuItem.Items.Add(tenCoinsMenuItem);

            twentyCoinsMenuItem = new MenuItem("20 coins", new Shortcut(ConsoleKey.D7), enabled: false);
            twentyCoinsMenuItem.OnClick += MazeItemClick;
            twentyCoinsMenuItem.Tag = MapItem.TwentyCoins;
            editMenuItem.Items.Add(twentyCoinsMenuItem);

            editMenuItem.Items.Add(new SeparatorMenuItem());
            generateMenuItem = new MenuItem("Generate", new Shortcut(ConsoleKey.G, false, true, false), enabled: false);
            generateMenuItem.OnClick += GenerateClick;
            editMenuItem.Items.Add(generateMenuItem);

            editMenuItem.Items.Add(new SeparatorMenuItem());
            clearMazeMenuItem = new MenuItem("Clear maze", null, enabled: false);
            clearMazeMenuItem.OnClick += ClearMazeClick;
            editMenuItem.Items.Add(clearMazeMenuItem);

            editMenuItem.Items.Add(new SeparatorMenuItem());
            checkPathMenuItem = new MenuItem("Check path", new Shortcut(ConsoleKey.P, false, true, false), enabled: false);
            checkPathMenuItem.OnClick += CheckPathClick;
            editMenuItem.Items.Add(checkPathMenuItem);

            mainMenu1.Items.Add(editMenuItem);
            MainMenu = mainMenu1;

            statusBar1 = new StatusBar();
            statusBar1.AddHelpButton();
            StatusBar = statusBar1;

            panel1 = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            label1 = new Label(
                "Welcome to the maze map editor. Use main menu (Alt+F1) for create new (Alt-N) maze map or open (Alt-O) exist file of map")
            {
                Foreground = Color.DarkGray,
                Width = Math.Min(65, Application.Environment.WindowWidth - 4),
                WordWrap = true,
                TextAlignment = TextAlignment.Center,
            };
            panel1.AddContent(label1);

            statusPositionLabel = new Label();
            statusDimensionLabel = new Label();
            statusMazeItemLabel = new Label();
            statusSaveLabel = new Label("●");
            AddContent(panel1);
        }

        private MainMenu mainMenu1;
        private MenuItem fileMenuItem;
        private MenuItem newMenuItem;
        private MenuItem openMenuItem;
        private MenuItem saveMenuItem;
        private MenuItem saveAsMenuItem;
        private MenuItem exitMenuItem;
        private MenuItem editMenuItem;
        private MenuItem wallMenuItem;
        private MenuItem startMenuItem;
        private MenuItem endMenuItem;
        private MenuItem doorMenuItem;
        private MenuItem keyMenuItem;
        private MenuItem tenCoinsMenuItem;
        private MenuItem twentyCoinsMenuItem;
        private MenuItem generateMenuItem;
        private MenuItem clearMazeMenuItem;
        private MenuItem checkPathMenuItem;

        private StatusBar statusBar1;
        private Panel panel1;
        private Label label1;
        private Label statusDimensionLabel;
        private Label statusPositionLabel;
        private Label statusMazeItemLabel;
        private Label statusSaveLabel;
    }
}
