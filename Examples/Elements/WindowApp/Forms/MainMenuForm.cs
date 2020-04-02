using System;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace WindowApp.Forms
{
    public partial class MainMenuForm: Window
    {
        private readonly Window targetWindow;
        private bool wasHiddenMenuItem;
        private bool wasControlStatedMenuItem;
        private bool wasBackgroundMenuItem;
        private bool wasForegroundMenuItem;

        public MainMenuForm(Window targetWindow)
        {
            this.targetWindow = targetWindow;
            InitializeComponent();
            targetWindow.MainMenu = mainMenu1;
        }

        private void HideMenuItemClick(object sender, System.EventArgs e)
        {
            targetWindow.HideTitle = !targetWindow.HideTitle;
            hideMenuItem.Checked = targetWindow.HideTitle;
            if (!wasHiddenMenuItem)
            {
                wasHiddenMenuItem = true;
                wasControlStatedMenuItem = false;
                wasBackgroundMenuItem = false;
                wasForegroundMenuItem = false;
                codeStackPanel.RemoveAllContent();
                codeStackPanel.AddContent(new Label
                {
                    Foreground = Color.Blue,
                    Text = "this.HideTitle = false;",
                });
                descriptionCodeLabel.Text = "Gets or sets a value that indicates whether a window should hide title text in the top";
            }
        }

        private void WindowStateChangedClick(object sender, System.EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetWindow.WindowState = (ControlState) targetMenuItem.Tag;
            }

            if (!wasControlStatedMenuItem)
            {
                wasControlStatedMenuItem = true;
                wasHiddenMenuItem = false;
                wasBackgroundMenuItem = false;
                wasForegroundMenuItem = false;
                codeStackPanel.RemoveAllContent();
                codeStackPanel.AddContent(new Label
                {
                    Foreground = Color.Blue,
                    WordWrap = true,
                    Text = "this.WindowState = ControlState.Maximized;",
                });
                descriptionCodeLabel.Text = "The WindowState property with value \"Maximized\" sets window size as big as console window allowed. Its ignore values of width and height properties of the window. It also hides the window title";
            }
        }

        private void ExitClick(object sender, System.EventArgs e) => targetWindow.Close();

        private void ForegroundClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetWindow.Foreground = (Color) targetMenuItem.Tag;
            }

            if (!wasForegroundMenuItem)
            {
                wasHiddenMenuItem = false;
                wasControlStatedMenuItem = false;
                wasBackgroundMenuItem = false;
                wasForegroundMenuItem = true;

                codeStackPanel.RemoveAllContent();
                codeStackPanel.AddContent(new Label
                {
                    Foreground = Color.Blue,
                    WordWrap = true,
                    Text = "this.Foreground = Color.{value};",
                });
                descriptionCodeLabel.Text = "The Foreground property of window sets text color of client area. All controls who set own foreground is [NotSet] will inherit parent text color";
            }
        }

        private void BackgroundItemClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetWindow.Background = (Color) targetMenuItem.Tag;
            }

            if (!wasBackgroundMenuItem)
            {
                wasHiddenMenuItem = false;
                wasControlStatedMenuItem = false;
                wasBackgroundMenuItem = true;
                wasForegroundMenuItem = false;

                codeStackPanel.RemoveAllContent();
                codeStackPanel.AddContent(new Label
                {
                    Foreground = Color.Blue,
                    WordWrap = true,
                    Text = "this.Background = Color.{value};",
                });
                descriptionCodeLabel.Text = "The Background property of window sets background color of client area. All controls who set own background is [NotSet] will inherit parent background";
            }
        }
    }
}
