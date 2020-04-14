using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;

namespace ButtonsApp.Forms
{
    public partial class CheckBoxForm
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
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 3));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "CheckBox represents a control to give the user an option, such as true/false or yes/no. CheckBox controls let the user pick a combination of options",
            };
            grid.AddContent(lb1, 0, 0, 2);

            var targetPanel = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            var stackPanel = new StackPanel(Orientation.Vertical)
            {
                AutoSize = true,
                Padding = new Thickness(1),
            };

            var cb1 = new CheckBox("CheckBox1")
            {
                Margin = new Thickness(0, 0, 0, 1),
            };
            cb1.OnCheckedChanged += CheckedChanged;
            stackPanel.AddContent(cb1);

            var cb2 = new CheckBox("CheckBox2")
            {
                Checked = true,
                Margin = new Thickness(0, 0, 0, 1),
            };
            cb2.OnCheckedChanged += CheckedChanged;
            stackPanel.AddContent(cb2);

            var cb3 = new CheckBox("CheckBox3")
            {
                IsDisabled = true,
                Margin = new Thickness(0, 0, 0, 1),
            };
            cb3.OnCheckedChanged += CheckedChanged;
            stackPanel.AddContent(cb3);

            var cb4 = new CheckBox("CheckBox4")
            {
                IsDisabled = true,
                Checked = true,
                Margin = new Thickness(0, 0, 0, 1),
            };
            cb4.OnCheckedChanged += CheckedChanged;
            stackPanel.AddContent(cb4);

            var cb5 = new CheckBox("CheckBox5")
            {
                ThreeState = true,
            };
            cb5.OnCheckedChanged += CheckedChanged;
            stackPanel.AddContent(cb5);

            targetPanel.AddContent(stackPanel);
            grid.AddContent(targetPanel, 0, 1);

            var propertyPanel = new StackPanel(Orientation.Vertical);
            propertyPanel.AddContent(
                new Label(
                    "Use [Tab]\\[Shift+Tab] for change focused checkbox. [Space] or [Enter] to change the state. The ThreeState property determines whether the control supports two or three states")
                {
                    Margin = new Thickness(1, 1, 0, 0),
                    WordWrap = true,
                });
            propertyChangedLabel = new Label("Click on any available checkbox…")
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
