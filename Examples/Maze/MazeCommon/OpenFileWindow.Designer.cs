using ClForms.Common;
using ClForms.Elements;

namespace MazeCommon
{
    public partial class OpenFileWindow
    {
        private void InitializeComponent()
        {
            Title = "Open a file";
            WindowState = ControlState.Normal;
            Width = 40;
            OnActivated += OpenFileWindowActivated;

            stackPanel1 = new StackPanel(Orientation.Vertical)
            {
                Margin = new Thickness(1, 1, 1, 0),
            };
            label1 = new Label("Select a file for open");
            stackPanel1.AddContent(label1);
            listBox1 = new ListBox<string>();
            listBox1.OnSelectedIndexChanged += ListBoxSelectedIndexChanged;
            stackPanel1.AddContent(listBox1);

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
                AutoSize = false,
            };
            stackPanel1.AddContent(dockPanel1);

            button2 = new Button("Cancel")
            {
                Margin = new Thickness(1, 0, 0, 0),
                DialogResult = DialogResult.Cancel,
            };
            dockPanel1.AddContent(button2, Dock.Right);

            button1 = new Button("Open")
            {
                DialogResult = DialogResult.OK,
                Width = 10,
                TabIndex = 1,
                IsDisabled = true,
            };
            dockPanel1.AddContent(button1, Dock.Right);

            AddContent(stackPanel1);
        }

        private StackPanel stackPanel1;
        private Label label1;
        private ListBox<string> listBox1;
        private TilePanel tilePanel1;
        private DockPanel dockPanel1;
        private Button button1;
        private Button button2;
    }
}
