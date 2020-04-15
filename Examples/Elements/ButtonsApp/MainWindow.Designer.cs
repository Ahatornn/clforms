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
            var controlsMenuItem = new MenuItem("Controls");

            buttonMenuItem = new MenuItem("Button");
            buttonMenuItem.OnClick += ControlMenuItemClick;
            buttonMenuItem.Tag = ScreenType.Button;
            controlsMenuItem.Items.Add(buttonMenuItem);

            checkBoxMenuItem = new MenuItem("CheckBox");
            checkBoxMenuItem.OnClick += ControlMenuItemClick;
            checkBoxMenuItem.Tag = ScreenType.CheckBox;
            controlsMenuItem.Items.Add(checkBoxMenuItem);

            radioMenuItem = new MenuItem("RadioButton");
            radioMenuItem.OnClick += ControlMenuItemClick;
            radioMenuItem.Tag = ScreenType.RadioButton;
            controlsMenuItem.Items.Add(radioMenuItem);

            messageBoxMenuItem = new MenuItem("MessageBox");
            messageBoxMenuItem.OnClick += ControlMenuItemClick;
            messageBoxMenuItem.Tag = ScreenType.MessageBox;
            controlsMenuItem.Items.Add(messageBoxMenuItem);

            mainMenu1.Items.Add(controlsMenuItem);
            MainMenu = mainMenu1;
        }

        private Panel panel1;
        private StatusBar statusBar1;
        private MainMenu mainMenu1;
        private MenuItem buttonMenuItem;
        private MenuItem checkBoxMenuItem;
        private MenuItem radioMenuItem;
        private MenuItem messageBoxMenuItem;
        private Label statusBarLabel;
    }
}
