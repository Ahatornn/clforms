using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace ButtonsApp.Forms
{
    public partial class MessageBoxForm
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
                Margin = new Thickness(0, 1, 0, 0),
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 20));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 80));

            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 4));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 3));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 4));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "MessageBox displays a message window, also known as a dialog box, which presents a message to the user. It is a modal window, blocking other actions in the application until the user closes it.",
            };
            grid.AddContent(lb1, 0, 0, 2);

            grid.AddContent(new Label("Caption"){Foreground = Color.DarkMagenta}, 0, 1);
            grid.AddContent(new Label("The text to display in the title bar of the message box")
                    {WordWrap = true}, 1, 1);

            grid.AddContent(new Label("Message") { Foreground = Color.DarkMagenta }, 0, 2);
            grid.AddContent(new Label("The text to display in the message box")
                { WordWrap = true }, 1, 2);

            grid.AddContent(new Label("MessageIcon") { Foreground = Color.DarkMagenta }, 0, 3);
            grid.AddContent(new Label("One of the values that specifies which icon to display in the message box [None, Error, Question, Warning, Information]")
                { WordWrap = true }, 1, 3);

            grid.AddContent(new Label("Buttons") { Foreground = Color.DarkMagenta }, 0, 4);
            grid.AddContent(new Label("One of the values that specifies which buttons to display in the message box [OK, OKCancel, AbortRetryIgnore, YesNoCancel, YesNo, RetryCancel]")
                { WordWrap = true }, 1, 4);

            var buttonsPanel = new Panel()
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            var stackPanel = new StackPanel(Orientation.Horizontal);

            var btn1 = new Button("Simple") {Margin = new Thickness(0, 0, 1, 0)};
            btn1.OnClick += SimpleClick;
            stackPanel.AddContent(btn1);

            var btn2 = new Button("With caption") { Margin = new Thickness(0, 0, 1, 0) };
            btn2.OnClick += CaptionClick;
            stackPanel.AddContent(btn2);

            var btn3 = new Button("Caption & icon") { Margin = new Thickness(0, 0, 1, 0) };
            btn3.OnClick += CaptionIconClick;
            stackPanel.AddContent(btn3);

            var btn4 = new Button("Full variant") { Margin = new Thickness(0, 0, 1, 0) };
            btn4.OnClick += FullClick;
            stackPanel.AddContent(btn4);

            buttonsPanel.AddContent(stackPanel);

            grid.AddContent(buttonsPanel, 0, 5, 2);
        }

        public Panel panel1;
    }
}
