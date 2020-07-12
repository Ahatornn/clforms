using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;

namespace GroupsApp.Forms
{
    public partial class CheckBoxGroupForm
    {
        /// <summary>
        /// Initialize components of Window as buttons, panels, etc.
        /// </summary>
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

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 55));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 45));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "CheckBoxGroup is a control to group of related but mutually exclusive checkbox buttons, requiring the user to select any of alternatives.",
            };
            grid.AddContent(lb1, 0, 0, 2);
            targetPanel = new CheckBoxGroup()
            {
                Text = "CheckBoxGroup panel",
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
        private CheckBoxGroup targetPanel;
        private Label descriptionLabel;
    }
}
