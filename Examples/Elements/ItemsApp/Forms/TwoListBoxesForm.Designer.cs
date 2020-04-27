using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace ItemsApp.Forms
{
    public partial class TwoListBoxesForm
    {
        private void InitializeComponent()
        {
            panel1 = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            var grid = new Grid
            {
                Parent = panel1,
                Width = 70,
                Margin = new Thickness(0, 1),
            };
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 3));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 20));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "Look at the example how to move some items between two listboxes. Manipulation only [Items] property by add and remove methods",
            };
            grid.AddContent(lb1, 0, 0, 3);

            leftListBox = new ListBox<string>();
            leftListBox.OnSelectedIndexChanged += ListBoxSelectedIndexChanged;
            grid.AddContent(leftListBox, 0, 1);

            stackPanel = new StackPanel(Orientation.Vertical)
            {
                Padding = new Thickness(1, 0),
                AutoSize = false,
            };

            leftToRightButton = new Button(">")
            {
                IsDisabled = true,
            };
            leftToRightButton.OnClick += LeftToRightButtonClick;
            stackPanel.AddContent(leftToRightButton);

            leftAllToRightButton = new Button(">>")
            {
                Margin = new Thickness(0, 1),
                IsDisabled = true,
            };
            leftAllToRightButton.OnClick += LeftAllToRightButtonClick;
            stackPanel.AddContent(leftAllToRightButton);

            rightToLeftButton = new Button("<")
            {
                IsDisabled = true,
            };
            rightToLeftButton.OnClick += RightToLeftButtonClick;
            stackPanel.AddContent(rightToLeftButton);

            rightAllToLeftButton = new Button("<<")
            {
                Margin = new Thickness(0, 1, 0, 0),
                IsDisabled = true,
            };
            rightAllToLeftButton.OnClick += RightAllToLeftButtonClick;
            stackPanel.AddContent(rightAllToLeftButton);

            grid.AddContent(stackPanel, 1, 1);

            rightListBox = new ListBox<string>();
            rightListBox.OnSelectedIndexChanged += ListBoxSelectedIndexChanged;
            grid.AddContent(rightListBox, 2, 1);
        }

        public Panel panel1;
        private ListBox<string> leftListBox;
        private ListBox<string> rightListBox;
        private StackPanel stackPanel;
        private Button leftToRightButton;
        private Button leftAllToRightButton;
        private Button rightToLeftButton;
        private Button rightAllToLeftButton;
    }
}
