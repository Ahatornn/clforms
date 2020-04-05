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
            switch (targetScreenType)
            {
                case ScreenType.Welcome:
                    _ = new WelcomeForm { panel1 = { Parent = panel1 } };
                    break;
                case ScreenType.Panel:
                    _ = new PanelForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    break;
                case ScreenType.GroupBox:
                    break;
                case ScreenType.StackPanel:
                    break;
                case ScreenType.DockPanel:
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
            }
        }

        private void ControlMenuItemClick(object sender, System.EventArgs e)
        {
            var screenType = (ScreenType) ((sender as MenuItem)?.Tag ?? ScreenType.Welcome);
            statusBarLabel.Text = screenType.ToString();
            GotoScreen(screenType);
        }
    }
}
