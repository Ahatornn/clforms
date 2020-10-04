using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace ListViewApp.Forms
{
    public partial class BaseListView
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
                Width = 76,
            };
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));

            targetListView = new ListView()
            {
                ShowGridLine = true,
                ShowSummary = true,
            };
            
            grid.AddContent(targetListView, 0, 0, 1, 2);

            descriptionLabel = new Label("Let's check how to work Items, ShowGridLine and ShowSummary properties")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 0);

            codeLabel = new Label("targetListView = new ListView();")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
                Foreground = Color.Yellow,
            };
            grid.AddContent(codeLabel, 1, 1);
        }

        public Panel panel1;
        private ListView targetListView;
        private Label descriptionLabel;
        private Label codeLabel;
    }
}
