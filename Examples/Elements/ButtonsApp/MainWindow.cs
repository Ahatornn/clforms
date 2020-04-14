using System;
using ButtonsApp.Forms;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace ButtonsApp
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
                case ScreenType.Button:
                    _ = new ButtonForm {panel1 = {Parent = panel1}};
                    break;
                case ScreenType.CheckBox:
                    _ = new CheckBoxForm {panel1 = {Parent = panel1}};
                    break;
                case ScreenType.RadioButton:
                    _ = new RadioButtonForm {panel1 = {Parent = panel1}};
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetScreenType), targetScreenType, null);
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
            buttonMenuItem.Checked = false;
            checkBoxMenuItem.Checked = false;
            radioMenuItem.Checked = false;
        }
    }
}
