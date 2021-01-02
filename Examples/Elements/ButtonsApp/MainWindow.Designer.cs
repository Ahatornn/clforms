using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace ButtonsApp
{
    public partial class MainWindow
    {
        private void InitializeComponent()
        {
            Width = 80;
            Height = 20;
            Title = "Main Window";

            panel1 = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            statusBar1 = new StatusBar();
            statusBar1.AddHelpButton();

            statusBarLabel = new Label();
            statusBar1.AddContent(statusBarLabel, Dock.Right);

            StatusBar = statusBar1;
            AddContent(panel1);

            mainMenu1 = new MainMenu();
            controlsMenuItem = new MenuItem("Controls");

            foreach (var item in Screens)
            {
                var menuItem = new MenuItem(item.Value);
                menuItem.OnClick += ControlMenuItemClick;
                menuItem.Tag = item.Key;
                controlsMenuItem.Items.Add(menuItem);
            }

            mainMenu1.Items.Add(controlsMenuItem);

            propMenuItem = new MenuItem("Properties");
            propMenuItem.Visible = false;
            mainMenu1.Items.Add(propMenuItem);

            MainMenu = mainMenu1;
        }

        private Panel panel1;
        private StatusBar statusBar1;
        private MainMenu mainMenu1;
        private MenuItem controlsMenuItem;
        private Label statusBarLabel;
        private MenuItem propMenuItem;
    }
}
