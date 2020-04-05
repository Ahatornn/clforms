using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace PanelsApp
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
            var controlsMenuItem = new MenuItem("Controls");

            var panelMenuItem = new MenuItem("Panel");
            panelMenuItem.OnClick += ControlMenuItemClick;
            panelMenuItem.Tag = ScreenType.Panel;
            controlsMenuItem.Items.Add(panelMenuItem);

            var gruopMenuItem = new MenuItem("GroupBox");
            gruopMenuItem.OnClick += ControlMenuItemClick;
            gruopMenuItem.Tag = ScreenType.GroupBox;
            controlsMenuItem.Items.Add(gruopMenuItem);

            var stackMenuItem = new MenuItem("StackPanel");
            stackMenuItem.OnClick += ControlMenuItemClick;
            stackMenuItem.Tag = ScreenType.StackPanel;
            controlsMenuItem.Items.Add(stackMenuItem);

            var dockMenuItem = new MenuItem("DockPanel");
            dockMenuItem.OnClick += ControlMenuItemClick;
            dockMenuItem.Tag = ScreenType.DockPanel;
            controlsMenuItem.Items.Add(dockMenuItem);

            mainMenu1.Items.Add(controlsMenuItem);
            propMenuItem = new MenuItem("Properties");
            mainMenu1.Items.Add(propMenuItem);
        }

        private Panel panel1;
        private StatusBar statusBar1;
        private MainMenu mainMenu1;
        private MenuItem propMenuItem;
        private Label statusBarLabel;
    }
}
