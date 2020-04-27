using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace ItemsApp
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

            mainMenu1 = new MainMenu();
            var controlsMenuItem = new MenuItem("Controls");

            listMenuItem = new MenuItem("Simple listbox");
            listMenuItem.OnClick += ControlMenuItemClick;
            listMenuItem.Tag = ScreenType.ListBox;
            controlsMenuItem.Items.Add(listMenuItem);

            twoListMenuItem = new MenuItem("Two listboxes");
            twoListMenuItem.OnClick += ControlMenuItemClick;
            twoListMenuItem.Tag = ScreenType.TwoListBoxes;
            controlsMenuItem.Items.Add(twoListMenuItem);

            mainMenu1.Items.Add(controlsMenuItem);

            propMenuItem = new MenuItem("Properties");
            mainMenu1.Items.Add(propMenuItem);

            statusBar1 = new StatusBar();
            statusBar1.AddHelpButton();

            statusBarLabel = new Label();
            statusBar1.AddContent(statusBarLabel, Dock.Right);
            StatusBar = statusBar1;

            AddContent(panel1);
        }

        private Panel panel1;
        private StatusBar statusBar1;
        private Label statusBarLabel;
        private MainMenu mainMenu1;
        private MenuItem listMenuItem;
        private MenuItem twoListMenuItem;
        private MenuItem propMenuItem;
    }
}
