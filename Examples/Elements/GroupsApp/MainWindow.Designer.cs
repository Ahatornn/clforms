using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace GroupsApp
{
    public partial class MainWindow
    {
        /// <summary>
        /// Initialize components of Window as buttons, panels, etc.
        /// </summary>
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

            radioGroupMenuItem = new MenuItem("RadioGroup");
            radioGroupMenuItem.OnClick += ControlMenuItemClick;
            radioGroupMenuItem.Tag = ScreenType.RadioGroup;
            controlsMenuItem.Items.Add(radioGroupMenuItem);

            checkBoxGroupMenuItem = new MenuItem("CheckBoxGroup");
            checkBoxGroupMenuItem.OnClick += ControlMenuItemClick;
            ;
            checkBoxGroupMenuItem.Tag = ScreenType.CheckBoxGroup;
            controlsMenuItem.Items.Add(checkBoxGroupMenuItem);

            mainMenu1.Items.Add(controlsMenuItem);

            propMenuItem = new MenuItem("Properties", null, enabled: false);
            mainMenu1.Items.Add(propMenuItem);
            MainMenu = mainMenu1;
        }

        private Panel panel1;
        private StatusBar statusBar1;
        private MainMenu mainMenu1;
        private MenuItem propMenuItem;
        private MenuItem radioGroupMenuItem;
        private MenuItem checkBoxGroupMenuItem;
        private Label statusBarLabel;
    }
}
