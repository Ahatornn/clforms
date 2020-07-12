using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace GroupsApp.Forms
{
    public partial class RadioGroupForm
    {
        /// <summary>
        /// Initialize components of Window as buttons, panels, etc.
        /// </summary>
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
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 4));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 55));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 45));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "RadioGroup is a control to group of related but mutually exclusive radio buttons, requiring the user to select one of a set of alternatives. As one button becomes selected, the remaining buttons in the group become automatically deselected.",
            };
            grid.AddContent(lb1, 0, 0, 2);
            targetPanel = new RadioGroup()
            {
                Text = "RadioGroup panel",
                TextAlignment = TextAlignment.Left,
                AutoSize = false,
            };
            grid.AddContent(targetPanel, 0, 1);

            descriptionLabel = new Label("Let's check how to work Columns, Autosize and other properties. If press [Tab] you can focus item inside the control. [Space] - check focusabled item")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
        private RadioGroup targetPanel;
        private Label descriptionLabel;
    }
}
