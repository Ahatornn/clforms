
using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace ButtonsApp.Forms
{
    public partial class ProgressBarForm
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
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 3));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "A ProgressBar control visually indicates the progress of a lengthy operation",
            };
            grid.AddContent(lb1, 0, 0, 2);

            targetPanel = new Panel();
            var targetPanelContent = new Grid();

            targetPanelContent.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));
            targetPanelContent.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));
            targetPanelContent.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));
            targetPanelContent.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));

            targetPanelContent.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 25));
            targetPanelContent.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 25));
            targetPanelContent.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 25));
            targetPanelContent.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 25));

            targetPanelContent.AddContent(
                new Label("Press [Tab] to select button below. Change some properties in main menu")
                {
                    WordWrap = true,
                    Margin = new Thickness(0, 0, 0, 1),
                },
                0, 0, 4);

            targetProgressBar = new ProgressBar()
            {
                Minimum = 0,
                Maximum = 200,
                Value = 50,
                SelectedBackground = Color.DarkBlue,
                Background = Color.Gray,
            };

            targetPanelContent.AddContent(targetProgressBar, 0, 2, 4);

            foreach (var item in new []
            {
                ("<<", -200, 0),
                ("<", -10, 1),
                (">", 10, 2),
                (">>", 200, 3),
            })
            {
                var btn1 = new Button(item.Item1)
                {
                    Tag = item.Item2,
                };
                btn1.OnClick += BtnClick;
                targetPanelContent.AddContent(btn1, item.Item3, 1);
                targetPanelContent.SetHorizontalAlignment(btn1, HorizontalAlignment.Center);
            }

            targetPanelContent.AddContent(new Label($"Min: {targetProgressBar.Minimum}"), 0, 3);
            currentValueLabel = new Label($"Val: {targetProgressBar.Value}")
                {
                    TextAlignment = TextAlignment.Center,
                    AutoSize = false,
                };
            targetPanelContent.AddContent(currentValueLabel, 1, 3, 2);
            targetPanelContent.AddContent(new Label($"Max: {targetProgressBar.Maximum}")
            {
                TextAlignment = TextAlignment.Right,
                AutoSize = false,
            }, 3, 3);

            targetPanel.AddContent(targetPanelContent);
            grid.AddContent(targetPanel, 0, 1);

            descriptionLabel = new Label("Let's check how to work Maximum, Minimum, SelectedBackground, SelectedForeground, ShowValue, Value and ValueAsPerCent properties")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
        private Panel targetPanel;
        private ProgressBar targetProgressBar;
        private Label currentValueLabel;
        private Label descriptionLabel;
    }
}
