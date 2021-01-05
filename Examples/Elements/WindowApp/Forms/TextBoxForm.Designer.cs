using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace WindowApp.Forms
{
    public partial class TextBoxForm
    {
        /// <summary>
        /// Initialize components of Window as buttons, panels, etc.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            var grid = new Grid
            {
                Parent = panel1,
                Width = 70,
                Margin = new Thickness(0, 1),
            };

            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 3));
            grid.RowDefinitions.AddRow();
            grid.RowDefinitions.AddRow();
            grid.RowDefinitions.AddRow();
            grid.RowDefinitions.AddRow();

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));

            var lb1 = new Label
            {
                WordWrap = true,
                Foreground = Color.NotSet,
                Background = Color.NotSet,
                Text = "Now look at an example how to use TextBox in your application. Press [Tab] to change control.",
            };

            grid.AddContent(lb1, 0, 0, 2);

            var tb1 = new TextBox()
            {
                CharacterCasing = CharacterCasing.Lower,
                Text = "Character casing is Lower",
                Tag = "CharacterCasing is modifies the case of characters as they are typed. Lower: converts all characters to lowercase",
            };
            tb1.OnEnter += ShowDescriptionByControlEnter;
            grid.AddContent(tb1, 0, 1);

            var tb2 = new TextBox()
            {
                CharacterCasing = CharacterCasing.Normal,
                Text = "Character casing is Normal",
                Tag = "CharacterCasing is modifies the case of characters as they are typed. Normal: converts all characters is left unchanged",
            };
            tb2.OnEnter += ShowDescriptionByControlEnter;
            grid.AddContent(tb2, 0, 2);

            var tb3 = new TextBox()
            {
                CharacterCasing = CharacterCasing.Upper,
                Text = "Character casing is Upper",
                Tag = "CharacterCasing is modifies the case of characters as they are typed. Upper: converts all characters to uppercase",
            };
            tb3.OnEnter += ShowDescriptionByControlEnter;
            grid.AddContent(tb3, 0, 3);

            var tb4 = new TextBox()
            {
                PasswordChar = '●',
                Text = "Password char was set",
                Tag = "PasswordChar has character used to mask characters of a password in the control. Original text is: 'Password char was set'",
            };
            tb4.OnEnter += ShowDescriptionByControlEnter;
            tb4.OnTextChanged += TextChanged;
            grid.AddContent(tb4, 0, 4);

            descriptionCodeLabel = new Label
            {
                WordWrap = true,
                Text = "Let's check how to work CharacterCasing and PasswordChar properties",
                Margin = new Thickness(1, 0, 0, 0),
            };
            grid.AddContent(descriptionCodeLabel, 1, 1, 1, 4);
        }

        public Panel panel1;
        private Label descriptionCodeLabel;
    }
}
