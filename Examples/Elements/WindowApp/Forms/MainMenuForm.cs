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

        public MainMenuForm(Window targetWindow)
        {
            wasHiddenMenuItem = false;
            wasControlStatedMenuItem = false;
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
    }
}
