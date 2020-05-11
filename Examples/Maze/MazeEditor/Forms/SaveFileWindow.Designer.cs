using ClForms.Common;
using ClForms.Elements;
using ClForms.Themes;
using MazeCommon;

namespace MazeEditor.Forms
{
    public partial class SaveFileWindow
    {
        private void InitializeComponent()
        {
            Title = Consts.SavingHeader;
            WindowState = ControlState.Normal;
            Width = 40;
            OnActivated += SaveFileWindowActivated;

            stackPanel1 = new StackPanel(Orientation.Vertical)
            {
                Margin = new Thickness(1, 1, 1, 0),
            };
            label1 = new Label("Previously saved files");
            stackPanel1.AddContent(label1);

            listBox1 = new ListBox<string>() {Height = 6,};
            listBox1.OnSelectedItemClick += ListBoxSelectedItemClick;
            stackPanel1.AddContent(listBox1);

            label2 = new Label("Target file name")
            {
                Margin = new Thickness(0, 1, 0, 0)
            };
            stackPanel1.AddContent(label2);

            textBox1 = new TextBox();
            textBox1.OnTextChanged += TextBox1Changed;
            stackPanel1.AddContent(textBox1);

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

            button1 = new Button("Save")
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
        private Label label2;
        private TextBox textBox1;
        private TilePanel tilePanel1;
        private DockPanel dockPanel1;
        private Button button1;
        private Button button2;
    }
}
