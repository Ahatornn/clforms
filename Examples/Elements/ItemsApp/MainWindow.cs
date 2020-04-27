using System;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ItemsApp.Forms;

namespace ItemsApp
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
                    _ = new WelcomeForm { panel1 = { Parent = panel1 } };
                    break;

                case ScreenType.ListBox:
                    _ = new ListBoxForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    listMenuItem.Checked = true;
                    break;

                case ScreenType.TwoListBoxes:
                    _ = new TwoListBoxesForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    twoListMenuItem.Checked = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(targetScreenType), targetScreenType, null);
            }
        }

        private void ControlMenuItemClick(object sender, System.EventArgs e)
        {
            var screenType = (ScreenType) ((sender as MenuItem)?.Tag ?? ScreenType.Welcome);
            statusBarLabel.Text = screenType.ToString();
            GotoScreen(screenType);
        }

        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            base.InputActionInternal(keyInfo);
            if (keyInfo.Key == ConsoleKey.Spacebar && currentScreenType == ScreenType.Welcome)
            {
                MainMenu = mainMenu1;
                GotoScreen((ScreenType) ((int) currentScreenType + 1));
                statusBarLabel.Text = currentScreenType.ToString();
            }
        }

        private void ReleaseCheckedMenu()
        {
            listMenuItem.Checked = false;
            twoListMenuItem.Checked = false;
        }
    }
}
