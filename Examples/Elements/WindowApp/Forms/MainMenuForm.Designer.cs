using System;
using System.Linq;
using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Core;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace WindowApp.Forms
{
    public partial class MainMenuForm
    {
        private void InitializeComponent()
        {
            panel1 = new Panel
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            mainMenu1 = new MainMenu();
            var paramItem = new MenuItem("Window");
            var maxParamItem = new MenuItem("Maximized");
            maxParamItem.Tag = ControlState.Maximized;
            maxParamItem.OnClick += WindowStateChangedClick;
            paramItem.Items.Add(maxParamItem);

            var mormParamItem = new MenuItem("Normal");
            mormParamItem.Tag = ControlState.Normal;
            mormParamItem.OnClick += WindowStateChangedClick;
            paramItem.Items.Add(mormParamItem);

            paramItem.Items.Add(new SeparatorMenuItem());
            hideMenuItem = new MenuItem("Hide title");
            hideMenuItem.OnClick += HideMenuItemClick;
            paramItem.Items.Add(hideMenuItem);
            paramItem.Items.Add(new SeparatorMenuItem());
            var exitItem = new MenuItem("Exit");
            exitItem.OnClick += ExitClick;
            paramItem.Items.Add(exitItem);
            mainMenu1.Items.Add(paramItem);

            var commonParam = new MenuItem("Common");
            var bgItem = new MenuItem("Background");
            var fgItem = new MenuItem("Foreground");
            foreach (var value in Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .Where(x => x != Color.NotSet)
                .OrderBy(x => x.ToString()))
            {
                if (value.ToString().StartsWith("Dark"))
                {
                    var bgClItem = new MenuItem(value.ToString());
                    bgClItem.Tag = value;
                    bgClItem.OnClick += BackgroundItemClick;
                    bgItem.Items.Add(bgClItem);
                }
                else
                {
                    var fgClItem = new MenuItem(value.ToString());
                    fgClItem.Tag = value;
                    fgClItem.OnClick += ForegroundClick;
                    fgItem.Items.Add(fgClItem);
                }
            }
            commonParam.Items.Add(bgItem);
            commonParam.Items.Add(fgItem);
            mainMenu1.Items.Add(commonParam);

            var grid = new Grid
            {
                Parent = panel1,
                Width = 70,
                Margin = new Thickness(0, 1),
            };
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 4));
            grid.RowDefinitions.AddRow();
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));

            var lb1 = new Label
            {
                WordWrap = true,
                Foreground = Color.NotSet,
                Background = Color.NotSet,
                Text = "Now look at an example how to use MainMenu in your application. Look at the top of the window. There are panel with items. Press [Alt+F1] for go to the first main menu item.",
            };

            grid.AddContent(lb1, 0, 0, 2);

            codeStackPanel = new StackPanel(Orientation.Vertical)
            {
                Background = Application.SystemColors.WindowBackground,
            };
            codeStackPanel.AddContent(new Label
            {
                Foreground = Color.Blue,
                Text = "var mMenu = new MainMenu();",
            });
            codeStackPanel.AddContent(new Label
            {
                Foreground = Color.DarkBlue,
                Text = "var fItem = new MenuItem(\"File\");",
            });
            codeStackPanel.AddContent(new Label
            {
                Foreground = Color.DarkBlue,
                Text = "var nItem = new MenuItem(\"&New\");",
            });
            codeStackPanel.AddContent(new Label
            {
                Foreground = Color.DarkBlue,
                Text = "nItem.OnClick += itemClick;",
            });
            codeStackPanel.AddContent(new Label
            {
                Foreground = Color.DarkBlue,
                Text = "fItem.Items.Add(nItem);",
            });
            codeStackPanel.AddContent(new Label
            {
                Foreground = Color.Blue,
                Text = "mMenu.Items.Add(fItem);",
            });
            codeStackPanel.AddContent(new Label
            {
                Foreground = Color.Blue,
                Text = "this.MainMenu = mMenu;",
            });

            grid.AddContent(codeStackPanel, 0, 1);

            descriptionCodeLabel = new Label
            {
                WordWrap = true,
                Text = "First you should to create new instance of MainMenu. Then create some menu items for your business logic. Finally set \"MainMenu\" property of main window.",
            };
            grid.AddContent(descriptionCodeLabel, 1, 1);

            grid.AddContent(
                new Label
                {
                    Text = "Press [space bar] button for next controls",
                    Margin = new Thickness(0, 0, 0, 0),
                }, 0, 2, 2);

            AddContent(panel1);
        }

        public Panel panel1;
        private StackPanel codeStackPanel;
        private Label descriptionCodeLabel;
        private MainMenu mainMenu1;
        private MenuItem hideMenuItem;
    }
}
