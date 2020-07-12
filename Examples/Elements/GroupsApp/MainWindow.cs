using System;
using ClForms.Elements;
using ClForms.Elements.Menu;
using GroupsApp.Forms;

namespace GroupsApp
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScreenType currentScreenType;

        /// <summary>
        /// Initialize a new instance <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            GotoScreen(ScreenType.Welcome);
        }

        private void GotoScreen(ScreenType targetScreenType)
        {
            currentScreenType = targetScreenType;
            ReleaseCheckedMenu();
            if (targetScreenType != ScreenType.Welcome && !propMenuItem.Enabled)
            {
                propMenuItem.Enabled = true;
            }
            switch (targetScreenType)
            {
                case ScreenType.Welcome:
                    _ = new WelcomeForm { panel1 = { Parent = panel1 } };
                    break;
                case ScreenType.RadioGroup:
                    _ = new RadioGroupForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    break;
                case ScreenType.CheckBoxGroup:
                    _ = new CheckBoxGroupForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetScreenType), targetScreenType, null);
            }
        }

        private void ReleaseCheckedMenu()
        {
            radioGroupMenuItem.Checked = false;
            checkBoxGroupMenuItem.Checked = false;
        }

        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            base.InputActionInternal(keyInfo);
            if (keyInfo.Key == ConsoleKey.Spacebar && currentScreenType == ScreenType.Welcome)
            {
                GotoScreen(ScreenType.RadioGroup);
                radioGroupMenuItem.Checked = true;
                statusBarLabel.Text = currentScreenType.ToString();
            }
        }

        private void ControlMenuItemClick(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                var screenType = (ScreenType) (menuItem.Tag ?? ScreenType.Welcome);
                statusBarLabel.Text = screenType.ToString();
                GotoScreen(screenType);
                menuItem.Checked = true;
            }
        }
    }
}
