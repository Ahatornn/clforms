using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;
using ListViewApp.Models;

namespace ListViewApp.Forms
{
    public partial class HeaderForm
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
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Percent, 100));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));

            targetListView = new ListView<CountryInfo>()
            {
                ShowGridLine = true,
                ShowSummary = true,
                BorderColor = Color.Black,
                SummaryText = "Use [Tab], [↓] or [↑]",
                Text = "Counties",
                TextAlignment = TextAlignment.Center,
                AutoSelect = true,
                HeaderForeground = Color.Cyan,
            };
            targetListView.Items.AddRange(MainWindow.CountryInfos);
            targetListView.ColumnHeaders.Add("Country", x => x.Country);
            targetListView.ColumnHeaders.Add("Gdp", x => x.Gdp.ToString(), 6, TextAlignment.Right);
            targetListView.ColumnHeaders.Add("Ppl", x => x.Population.ToString(), 9, TextAlignment.Right);
            targetListView.OnItemDraw += TargetListViewItemDraw;

            grid.AddContent(targetListView, 0, 0, 1, 2);

            var stackPanel = new StackPanel(Orientation.Vertical)
            {
                Margin = new Thickness(1, 0, 0, 0),
            };

            descriptionLabel = new Label("Let's check how to work with ColumnHeaders")
            {
                WordWrap = true,
            };
            stackPanel.AddContent(descriptionLabel);

            codeLabel = new Label(new[]
            {
                "targetListView.HeaderForeground = Color.Cyan;",
                "…",
                "targetListView.ColumnHeaders",
                ".Add(\"Country\",",
                "    x => x.Population,",
                "    9,",
                "    TextAlignment.Right",
                ");",
            })
            {
                WordWrap = true,
                Foreground = Color.Yellow,
            };
            stackPanel.AddContent(codeLabel);

            grid.AddContent(stackPanel, 1, 0);
        }

        public Panel panel1;
        private ListView<CountryInfo> targetListView;
        private Label descriptionLabel;
        private Label codeLabel;
    }
}
