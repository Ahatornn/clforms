using System;
using System.Runtime.InteropServices.ComTypes;
using ClForms.Elements;
using WindowApp.Forms;

namespace WindowApp
{
    public partial class MainWindow: Window
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
                    _ = new WelcomeForm {panel1 = {Parent = panel1}};
                    break;
                case ScreenType.StatusBarCommon:
                    _ = new StatusBarCommonForm {panel1 = {Parent = panel1}};
                    StatusBar = statusBar1;
                    break;
                case ScreenType.MainWindowCommon:
                    _ = new MainMenuForm(this) {panel1 = {Parent = panel1}};
                    break;
                case ScreenType.TextBox:
                    MainMenu.Items.Clear();
                    _ = new TextBoxForm {panel1 = {Parent = panel1}};
                    break;
            }
        }

        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            base.InputActionInternal(keyInfo);
            if (keyInfo.Key == ConsoleKey.Spacebar)
            {
                GotoScreen((ScreenType) ((int) currentScreenType + 1));
            }
        }
    }
}
