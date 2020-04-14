using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;

namespace ButtonsApp.Forms
{
    public partial class RadioButtonForm
    {
        private void InitializeComponent()
        {
            panel1 = new Panel
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
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 2));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "RadioButton enables the user to select a single option from a group of choices when paired with other RadioButton controls",
            };
            grid.AddContent(lb1, 0, 0, 2);

            
            var stackPanel = new StackPanel(Orientation.Vertical)
            {
                AutoSize = true,
                Padding = new Thickness(1),
            };
            var grBox1 = new GroupBox()
            {
                Text = "Group1",
                Margin = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(2, 0),
            };
            var grStackPanel1 = new StackPanel(Orientation.Vertical);
            for (int i = 1; i <= 3; i++)
            {
                var rButton = new RadioButton($"RadioButton{i}");
                rButton.OnCheckedChanged += CheckedChanged;
                grStackPanel1.AddContent(rButton);
            }
            grBox1.AddContent(grStackPanel1);
            stackPanel.AddContent(grBox1);

            var grBox2 = new GroupBox()
            {
                Text = "Group2",
                Padding = new Thickness(2, 0),
            };
            var grStackPanel2 = new StackPanel(Orientation.Vertical);
            for (int i = 1; i <= 3; i++)
            {
                var rButton = new RadioButton($"RadioButton{i}");
                rButton.OnCheckedChanged += CheckedChanged;
                grStackPanel2.AddContent(rButton);
            }
            grBox2.AddContent(grStackPanel2);
            stackPanel.AddContent(grBox2);

            grid.AddContent(stackPanel, 0, 1);

            var propertyPanel = new StackPanel(Orientation.Vertical);
            propertyPanel.AddContent(
                new Label(
                    "Use [Tab]\\[Shift+Tab] for change focused radiobutton. [Space] or [Enter] to clik. When the user selects one option button (also known as a radio button) within a group, the others clear automatically. All RadioButton controls in a given container, such as a Window, constitute a group")
                {
                    Margin = new Thickness(1, 1, 0, 0),
                    WordWrap = true,
                });
            propertyChangedLabel = new Label("Click on any radiobutton…")
            {
                Margin = new Thickness(1, 1, 0, 0),
                WordWrap = true,
            };
            propertyPanel.AddContent(propertyChangedLabel);
            grid.AddContent(propertyPanel, 1, 1);
        }

        public Panel panel1;
        private Label propertyChangedLabel;
    }
}
