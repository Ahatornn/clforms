using System;
using System.Collections.Generic;
using System.Linq;
using ButtonsApp.Forms;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace ButtonsApp
{
    public partial class MainWindow : Window
    {
        internal static Dictionary<ScreenType, string> Screens = new Dictionary<ScreenType, string>
        {
            {ScreenType.Button, "Button"},
            {ScreenType.CheckBox, "CheckBox"},
            {ScreenType.RadioButton, "RadioButton"},
            {ScreenType.MessageBox, "MessageBox"},
            {ScreenType.ProgressBar, "ProgressBar"},
        };
        private ScreenType currentScreenType;

        public MainWindow()
        {
            InitializeComponent();
            GotoScreen(ScreenType.Welcome);
        }

        private void GotoScreen(ScreenType targetScreenType)
        {
            propMenuItem.Visible = false;
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
                case ScreenType.MessageBox:
                    _ = new MessageBoxForm {panel1 = {Parent = panel1}};
                    break;
                case ScreenType.ProgressBar:
                    _ = new ProgressBarForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    propMenuItem.Visible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetScreenType), targetScreenType, null);
            }

            var element = controlsMenuItem.Items.Cast<MenuItem>().FirstOrDefault(x => (ScreenType) x.Tag == targetScreenType);
            if (element != null)
            {
                element.Checked = true;
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
            foreach (var item in controlsMenuItem.Items.Cast<MenuItem>())
            {
                item.Checked = false;
            }
        }
    }
}
