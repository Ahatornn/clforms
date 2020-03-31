using ClForms.Common;
using ClForms.Common.Grid;
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
            var paramItem = new MenuItem("Window parameters");
            var maxParamItem = new MenuItem("Maximized");
            maxParamItem.Tag = ControlState.Maximized;
            maxParamItem.OnClick += WindowStateChangedClick;
            paramItem.Items.Add(maxParamItem);

            var mormParamItem = new MenuItem("Mormal");
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

            var grid = new Grid
            {
                Parent = panel1,
                Width = 70,
                Margin = new Thickness(0, 2),
            };
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 4));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "Now look at an example how to use MainMenu in your application. Look at the top of the window. There are panel with items. Press [Alt+F1] for go to the first main menu item.",
                Margin = new Thickness(0, 0, 0, 1),
            };

            grid.AddContent(lb1, 0, 0, 2);

            codeStackPanel = new StackPanel(Orientation.Vertical);
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

            AddContent(panel1);
        }

        public Panel panel1;
        private StackPanel codeStackPanel;
        private Label descriptionCodeLabel;
        private MainMenu mainMenu1;
        private MenuItem hideMenuItem;
    }
}
