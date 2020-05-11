using ClForms.Common;
using ClForms.Elements;

namespace MazeEditor.Forms
{
    public partial class MazeDimensionWindow
    {
        private void InitializeComponent()
        {
            Title = "Select maze dimension";
            Width = 45;
            OnKeyPressed += KeyPressed;

            stackPanel1 = new StackPanel(Orientation.Vertical);
            AddContent(stackPanel1);

            stackPanel2 = new StackPanel(Orientation.Vertical)
            {
                Padding = new Thickness(2, 1),
            };
            stackPanel1.AddContent(stackPanel2);

            label1 = new Label("Select the dimension of the created maze map from the list below")
            {
                Margin = new Thickness(0, 0, 0, 1),
                WordWrap = true,
            };
            stackPanel2.AddContent(label1);

            radioButton1 = new RadioButton("45 x 15")
            {
                Checked = true,
                Margin = new Thickness(5, 0, 0, 0),
            };
            stackPanel2.AddContent(radioButton1);

            radioButton2 = new RadioButton("60 x 21")
            {
                Margin = new Thickness(5, 0, 0, 0),
            };
            stackPanel2.AddContent(radioButton2);

            radioButton3 = new RadioButton("120 x 28")
            {
                Margin = new Thickness(5, 0, 0, 0),
            };
            stackPanel2.AddContent(radioButton3);

            tilePanel1 = new TilePanel
            {
                Tile = '─',
                Background = this.Background,
                Foreground = this.Foreground,
                Height = 1,
            };
            stackPanel1.AddContent(tilePanel1);

            dockPanel1 = new DockPanel()
            {
                Margin = new Thickness(2, 0),
                AutoSize = true,
            };
            stackPanel1.AddContent(dockPanel1);

            button2 = new Button("Cancel")
            {
                Margin = new Thickness(1, 0, 0, 0),
                DialogResult = DialogResult.Cancel,
            };
            dockPanel1.AddContent(button2, Dock.Right);

            button1 = new Button("OK")
            {
                DialogResult = DialogResult.OK,
                Width = 10,
                TabIndex = 1,
            };
            dockPanel1.AddContent(button1, Dock.Right);
        }

        private StackPanel stackPanel1;
        private StackPanel stackPanel2;
        private Label label1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private TilePanel tilePanel1;
        private DockPanel dockPanel1;
        private Button button1;
        private Button button2;
    }
}
