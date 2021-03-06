using System;
using ClForms.Elements;
using ClForms.Elements.Menu;
using PanelsApp.Forms;

namespace PanelsApp
{
    public partial class MainWindow : Window
    {
        private ScreenType currentScreenType;

        public MainWindow()
        {
            InitializeComponent();
            GotoScreen(ScreenType.Welcome);
        }

        private void GotoScreen(ScreenType targetScreenType)
        {
            currentScreenType = targetScreenType;
            ReleaseCheckedMenu();
            switch (targetScreenType)
            {
                case ScreenType.Welcome:
                    _ = new WelcomeForm {panel1 = {Parent = panel1}};
                    break;
                case ScreenType.Panel:
                    _ = new PanelForm(propMenuItem) {panel1 = {Parent = panel1}};
                    panelMenuItem.Checked = true;
                    break;
                case ScreenType.GroupBox:
                    _ = new GroupBoxForm(propMenuItem) {panel1 = {Parent = panel1}};
                    gruopMenuItem.Checked = true;
                    break;
                case ScreenType.StackPanel:
                    _ = new StackPanelForm(propMenuItem) {panel1 = {Parent = panel1}};
                    stackMenuItem.Checked = true;
                    break;
                case ScreenType.DockPanel:
                    _ = new DockPanelForm(propMenuItem) {panel1 = {Parent = panel1}};
                    dockMenuItem.Checked = true;
                    break;
                case ScreenType.TilePanel:
                    _ = new TilePanelForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    tileMenuItem.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetScreenType), targetScreenType, null);
            }

            if (currentScreenType != ScreenType.Welcome && MainMenu == null)
            {
                MainMenu = mainMenu1;
            }
        }

        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            base.InputActionInternal(keyInfo);
            if (keyInfo.Key == ConsoleKey.Spacebar && currentScreenType == ScreenType.Welcome)
            {
                GotoScreen((ScreenType) ((int) currentScreenType + 1));
                statusBarLabel.Text = currentScreenType.ToString();
            }
        }

        private void ControlMenuItemClick(object sender, System.EventArgs e)
        {
            var screenType = (ScreenType) ((sender as MenuItem)?.Tag ?? ScreenType.Welcome);
            statusBarLabel.Text = screenType.ToString();
            GotoScreen(screenType);
        }

        private void ReleaseCheckedMenu()
        {
            panelMenuItem.Checked = false;
            gruopMenuItem.Checked = false;
            stackMenuItem.Checked = false;
            dockMenuItem.Checked = false;
            tileMenuItem.Checked = false;
        }
    }
}
