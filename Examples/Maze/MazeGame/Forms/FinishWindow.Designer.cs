using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace MazeGame.Forms
{
    public partial class FinishWindow
    {
        private void InitializeComponent()
        {
            panel1 = new Panel()
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                AutoSize = false,
            };

            stackPanel1 = new StackPanel(Orientation.Vertical) { Width = 40, };
            groupBox1 = new GroupBox()
            {
                Background = Color.Green,
                BorderColor = Color.DarkGreen,
            };
            stackPanel2 = new StackPanel(Orientation.Vertical)
            {
                Margin = new Thickness(5, 0),
            };

            label1 = new Label("Congratulations!!!")
            {
                Foreground = Color.DarkGreen,
                TextAlignment = TextAlignment.Center,
            };
            stackPanel2.AddContent(label1);
            labelMapName = new Label()
            {
                Foreground = Color.DarkGreen,
                TextAlignment = TextAlignment.Center,
                WordWrap = true,
            };
            stackPanel2.AddContent(labelMapName);
            groupBox1.AddContent(stackPanel2);

            groupBox2 = new GroupBox()
            {
                Background = Color.Gray,
                BorderColor = Color.DarkGray,
                Margin = new Thickness(0, 1, 0, 0),
            };
            stackPanel3 = new StackPanel(Orientation.Vertical);
            stackPanel3.AddContent(new Label("Your achievements:")
            {
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 1),
            });
            grid1 = new Grid();
            grid1.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Absolute, 20));
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            grid1.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));
            grid1.AddContent(new Label("Steps taken"), 0, 0);
            labelSteps = new Label();
            grid1.AddContent(labelSteps, 1, 0);

            grid1.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));
            grid1.AddContent(new Label("Coins collected"), 0, 1);
            labelCoins = new Label();
            grid1.AddContent(labelCoins, 1, 1);

            stackPanel3.AddContent(grid1);

            groupBox2.AddContent(stackPanel3);

            stackPanel1.AddContent(groupBox1);
            stackPanel1.AddContent(groupBox2);
            panel1.AddContent(stackPanel1);
            AddContent(panel1);
        }

        public Panel panel1;
        private StackPanel stackPanel1;

        private GroupBox groupBox1;
        private StackPanel stackPanel2;
        private Label label1;
        private Label labelMapName;

        private GroupBox groupBox2;
        private StackPanel stackPanel3;
        private Grid grid1;
        private Label labelSteps;
        private Label labelCoins;
    }
}
