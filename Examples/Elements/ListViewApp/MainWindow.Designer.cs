using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace ListViewApp
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
            Title = "ListView demo app";

            panel1 = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            mainMenu1 = new MainMenu();
            controlsMenuItem = new MenuItem("Steps");

            var baseMenuItem = new MenuItem("Base properties", null, @checked: true);
            baseMenuItem.Tag = ScreenType.Base;
            baseMenuItem.OnClick += StepsMenuClick;
            controlsMenuItem.Items.Add(baseMenuItem);

            var headerMenuItem = new MenuItem("ColumnHeaders");
            headerMenuItem.Tag = ScreenType.Headers;
            headerMenuItem.OnClick += StepsMenuClick;
            controlsMenuItem.Items.Add(headerMenuItem);

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
        private MainMenu mainMenu1;
        private MenuItem propMenuItem;
        private StatusBar statusBar1;
        private Label statusBarLabel;
        private MenuItem controlsMenuItem;
    }
}
