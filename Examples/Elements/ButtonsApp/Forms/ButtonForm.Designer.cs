using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace ButtonsApp.Forms
{
    public partial class ButtonForm
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
                Text = "Button is a common button control. It inherited from ButtonBase and has Background as Application.SystemColors.ButtonFace and Foreground as Application.SystemColors.ButtonText",
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
            for (int i = 1; i <= 4; i++)
            {
                var btn = new Button($"Button{i}")
                {
                    Margin = new Thickness(0, 0, 0, i < 4 ? 1 : 0),
                    IsDisabled = i == 2,
                };
                btn.OnClick += BtnClick;
                stackPanel.AddContent(btn);
            }
            targetPanel.AddContent(stackPanel);
            grid.AddContent(targetPanel, 0, 1);

            var descriptionLabel = new Label("Use [Tab]\\[Shift+Tab] for change focused button. [Space] or [Enter] to clik. If you want to disable button selection, set the IsDisabled property. DialogResult gets or sets a value that is returned to the parent form when the button is clicked")
            {
                Margin = new Thickness(1, 1, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
    }
}
