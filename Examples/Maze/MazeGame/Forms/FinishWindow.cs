using ClForms.Elements;

namespace MazeGame.Forms
{
    public partial class FinishWindow: Window
    {
        public FinishWindow(string mapName, int steps, int coins)
        {
            InitializeComponent();
            labelMapName.Text = $"You have successfully completed '{mapName}'";
            labelSteps.Text = steps.ToString();
            labelCoins.Text = coins.ToString();
        }
    }
}
