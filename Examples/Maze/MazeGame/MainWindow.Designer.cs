using System;
using ClForms.Common;
using ClForms.Core;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace MazeGame
{
    public partial class MainWindow
    {
        private void InitializeComponent()
        {
            WindowState = ControlState.Maximized;
            Background = Color.Black;

            mainMenu1 = new MainMenu();
            fileMenuItem = new MenuItem("File");

            openMenuItem = new MenuItem("&Open…");
            openMenuItem.OnClick += OpenMenuItemClick;
            fileMenuItem.Items.Add(openMenuItem);

            exitMenuItem = new MenuItem("E&xit");
            exitMenuItem.OnClick += ExitMenuItemClick;
            fileMenuItem.Items.Add(exitMenuItem);

            mainMenu1.Items.Add(fileMenuItem);
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
                "Welcome to the maze game. Use main menu (Alt+F1) for open (Alt-O) exist file of map")
            {
                Foreground = Color.DarkGray,
                Width = Math.Min(60, Application.Environment.WindowWidth - 4),
                WordWrap = true,
                TextAlignment = TextAlignment.Center,
            };
            panel1.AddContent(label1);

            AddContent(panel1);

            statusPositionLabel = new Label();
            statusDimensionLabel = new Label();
            statusLootLabel = new Label();
            statusCoinsLabel = new Label();
        }

        private MainMenu mainMenu1;
        private MenuItem fileMenuItem;
        private MenuItem openMenuItem;
        private MenuItem exitMenuItem;

        private StatusBar statusBar1;
        private Label statusDimensionLabel;
        private Label statusPositionLabel;
        private Label statusLootLabel;
        private Label statusCoinsLabel;
        private Panel panel1;
        private Label label1;
    }
}
