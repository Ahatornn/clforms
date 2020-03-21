using System;
using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;

namespace ClForms.Helpers
{
    internal sealed class HelpWindow
    {
        internal static void ShowHelp()
        {
            var wnd = new Window
            {
                Title = "Help",
                WindowState = ControlState.Normal,
                Width = 58,
            };

            var context = new Grid()
            {
                Padding = new Thickness(1),
            };
            context.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Absolute, 15));
            context.ColumnDefinitions.Add(new ColumnDefinition());

            context.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));
            context.AddContent(new Label("Alt+F1"), 0, 0);
            context.AddContent(new Label("Go to main menu"), 1, 0);

            context.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 2));
            context.AddContent(new Label("Ctrl+F4"), 0, 1);
            context.AddContent(new Label("Close current window. If current window is main - it close all application")
            {
                WordWrap = true,
            }, 1, 1);

            context.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 2));
            context.AddContent(new Label("Tab"), 0, 2);
            context.AddContent(new Label("Change focusable control to next. You can also change selected menu item")
            {
                WordWrap = true,
            }, 1, 2);

            context.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 1));
            context.AddContent(new Label("Shift+Tab"), 0, 3);
            context.AddContent(new Label("Change focusable control to previous"), 1, 3);

            context.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 3));
            context.AddContent(new Label("Space(Enter)"), 0, 4);
            context.AddContent(new Label("Imitation of click on buttons, menu items, checkboxes and other focusable controls")
            {
                WordWrap = true,
            }, 1, 4);

            context.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 2));
            context.AddContent(new Label("Esc"), 0, 5);
            context.AddContent(new Label("Close popup menu items, exit from main menu. Close this help window")
            {
                WordWrap = true,
            }, 1, 5);

            wnd.AddContent(context);
            wnd.OnKeyPressed += WndKeyPressed;
            wnd.Show();
        }

        private static void WndKeyPressed(object sender, Common.EventArgs.KeyPressedEventArgs e)
        {
            if (e.KeyInfo.Key == ConsoleKey.Escape)
            {
                (sender as Window)?.Close();
            }
        }
    }
}
