using System;
using ClForms.Elements;
using ListViewApp.Forms;

namespace ListViewApp
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
            switch (targetScreenType)
            {
                case ScreenType.Welcome:
                    _ = new WelcomeForm { panel1 = { Parent = panel1 } };
                    break;
                case ScreenType.Base:
                    _ = new BaseListView(propMenuItem) { panel1 = { Parent = panel1 } };
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
    }
}
