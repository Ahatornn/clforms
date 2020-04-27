using System;
using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace ItemsApp.Forms
{
    public partial class ListBoxForm
    {
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

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Absolute, 2));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "ListBox represents a Windows control to display a list of items. You can use [ListBox] with object items or [ListBox<T>] with items of specified [T] type",
            };
            grid.AddContent(lb1, 0, 0, 3);

            targetListBox = new ListBox<ItemTest>();
            grid.AddContent(targetListBox, 0, 1);
            targetListBox.OnSelectedIndexChanged += ListBoxSelectedIndexChanged;

            Action<GlyphLabel> glyphStyle = x =>
            {
                x.Foreground = Color.Cyan;
                x.GlyphForeground = Color.Black;
            };

            var stackPanel = new StackPanel(Orientation.Vertical);

            countLabel = new GlyphLabel("Items count: ", "0");
            countLabel.SetStyle(glyphStyle);
            stackPanel.AddContent(countLabel);

            selectedIndexLabel = new GlyphLabel("Selected index: ", "-1");
            selectedIndexLabel.SetStyle(glyphStyle);
            stackPanel.AddContent(selectedIndexLabel);

            formatStringLabel = new GlyphLabel("Format string: ", " {0} ");
            formatStringLabel.SetStyle(glyphStyle);
            stackPanel.AddContent(formatStringLabel);

            addButton = new Button("Add item")
            {
                Margin = new Thickness(0, 1, 0, 0),
            };
            addButton.OnClick += AddButtonClick;
            stackPanel.AddContent(addButton);

            removeButton = new Button("Remove item")
            {
                IsDisabled = true,
                Margin = new Thickness(0, 1, 0, 0),
            };
            removeButton.OnClick += RemoveButtonClick;
            stackPanel.AddContent(removeButton);

            grid.AddContent(stackPanel, 2, 1);
        }

        public Panel panel1;
        private ListBox<ItemTest> targetListBox;
        private GlyphLabel countLabel;
        private GlyphLabel selectedIndexLabel;
        private GlyphLabel formatStringLabel;
        private Button addButton;
        private Button removeButton;
    }
}
