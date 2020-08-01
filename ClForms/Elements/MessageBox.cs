using System;
using System.Collections.Generic;
using System.Linq;
using ClForms.Common;
using ClForms.Core;

namespace ClForms.Elements
{
    /// <summary>
    /// Displays a message window which presents a message to the user. It is a modal window, blocking other actions in the
    /// application until the user closes it. A <see cref="MessageBox"/> can contain text, buttons, and symbols that inform
    /// and instruct the user
    /// </summary>
    public sealed class MessageBox
    {
        /// <summary>
        /// Displays a message box in front of the specified object and with the specified text. It also contains error icon
        /// </summary>
        /// <param name="message">The text to display in the message box</param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult ShowError(string message)
            => Show("Error", message, MessageBoxIcon.Error, MessageBoxButtons.OK);

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified caption and text. It also contains error icon
        /// </summary>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="message">The text to display in the message box</param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult ShowError(string caption, string message)
            => Show(caption, message, MessageBoxIcon.Error, MessageBoxButtons.OK);

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified caption and exception
        /// </summary>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="exception"><see cref="Exception"/></param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult ShowError(string caption, Exception exception)
            => Show(caption, exception.Message, MessageBoxIcon.Error, MessageBoxButtons.OK);

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified text
        /// </summary>
        /// <param name="message">The text to display in the message box</param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult Show(string message)
            => Show(string.Empty, message, MessageBoxIcon.None, MessageBoxButtons.OK);

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified caption and text
        /// </summary>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="message">The text to display in the message box</param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult Show(string caption, string message)
            => Show(caption, message, MessageBoxIcon.None, MessageBoxButtons.OK);

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified caption, text and buttons
        /// </summary>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="message">The text to display in the message box</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that specifies which buttons to display in the message box</param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult Show(string caption, string message, MessageBoxButtons buttons)
            => Show(caption, message, MessageBoxIcon.None, buttons);

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified text, icon and buttons
        /// </summary>
        /// <param name="message">The text to display in the message box</param>
        /// <param name="messageIcon">One of the <see cref="MessageBoxIcon"/> values that specifies which icon to display in the message box</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that specifies which buttons to display in the message box</param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult Show(string message, MessageBoxIcon messageIcon, MessageBoxButtons buttons)
            => Show(string.Empty, message, messageIcon, buttons);

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified caption, text, icon and buttons
        /// </summary>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="message">The text to display in the message box</param>
        /// <param name="messageIcon">One of the <see cref="MessageBoxIcon"/> values that specifies which icon to display in the message box</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that specifies which buttons to display in the message box</param>
        /// <returns>One of the <see cref="DialogResult"/> values</returns>
        public static DialogResult Show(string caption,
            string message,
            MessageBoxIcon messageIcon,
            MessageBoxButtons buttons)
        {
            var dialogForm = new Window
            {
                Title = GetCaption(caption, messageIcon),
                Padding = new Thickness(2, 0),
                WindowState = ControlState.Normal,
                AutoSize = true,
                Background = Application.SystemColors.DialogWindowBackground,
                Foreground = Application.SystemColors.DialogWindowForeground,
            };
            var mainPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                AutoSize = true,
            };

            var topPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                AutoSize = true,
            };

            if (messageIcon != MessageBoxIcon.None)
            {
                var iconPanel = new GroupBox
                {
                    Margin = new Thickness(0, 0, 2, 0),
                    Height = 3,
                    BorderChars = new BorderChars('╭', '─', '╮', '│', '│', '╰', '─', '╯'),
                };
                var iconLabel = new Label
                {
                    Margin = new Thickness(1, 0),
                };
                switch (messageIcon)
                {
                    case MessageBoxIcon.Error:
                        iconLabel.Text = "╳";
                        break;
                    case MessageBoxIcon.Question:
                        iconLabel.Text = "?";
                        break;
                    case MessageBoxIcon.Warning:
                        iconLabel.Text = "‼";
                        break;
                    case MessageBoxIcon.Information:
                        iconLabel.Text = "i";
                        break;
                }
                iconPanel.AddContent(iconLabel);
                var iconSimplePanel = new Panel { AutoSize = true };
                iconSimplePanel.AddContent(iconPanel);

                topPanel.AddContent(iconSimplePanel);
            }

            var label = new Label
            {
                Text = message,
                WordWrap = true,
                Margin = new Thickness(0, 1),
            };

            if (message.Length > Application.Environment.WindowWidth - Application.Environment.WindowWidth / 3)
            {
                label.Width = Application.Environment.WindowWidth - (Application.Environment.WindowWidth / 3);
            }
            topPanel.AddContent(label);
            mainPanel.AddContent(topPanel);

            var bottomPanel = new DockPanel
            {
                AutoSize = true,
            };
            foreach (var button in CreateButtons(buttons).Reverse())
            {
                bottomPanel.AddContent(button, Dock.Right);
            }
            mainPanel.AddContent(bottomPanel);
            dialogForm.AddContent(mainPanel);
            dialogForm.OnKeyPressed += DialogFormKeyPressed;
            dialogForm.Show();

            while (dialogForm.DialogResult == DialogResult.None)
            {
                Application.DoEvents();
            }
            dialogForm.Close();
            dialogForm.OnKeyPressed -= DialogFormKeyPressed;
            Application.CloseWindow();
            return dialogForm.DialogResult;
        }

        private static string GetCaption(string caption, MessageBoxIcon messageIcon)
        {
            if (!string.IsNullOrWhiteSpace(caption))
            {
                return caption;
            }

            if (messageIcon != MessageBoxIcon.None)
            {
                return messageIcon.ToString();
            }

            return string.Empty;
        }

        private static IEnumerable<Button> CreateButtons(MessageBoxButtons buttonsType)
        {
            switch (buttonsType)
            {
                case MessageBoxButtons.OK:
                    yield return CreateDialogButton(DialogResult.OK);
                    break;
                case MessageBoxButtons.OKCancel:
                    yield return CreateDialogButton(DialogResult.OK, 1);
                    yield return CreateDialogButton(DialogResult.Cancel);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    yield return CreateDialogButton(DialogResult.Abort, 2);
                    yield return CreateDialogButton(DialogResult.Retry, 1);
                    yield return CreateDialogButton(DialogResult.Ignore);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    yield return CreateDialogButton(DialogResult.Yes, 2);
                    yield return CreateDialogButton(DialogResult.No, 1);
                    yield return CreateDialogButton(DialogResult.Cancel);
                    break;
                case MessageBoxButtons.YesNo:
                    yield return CreateDialogButton(DialogResult.Yes, 1);
                    yield return CreateDialogButton(DialogResult.No);
                    break;
                case MessageBoxButtons.RetryCancel:
                    yield return CreateDialogButton(DialogResult.Retry, 1);
                    yield return CreateDialogButton(DialogResult.Cancel);
                    break;
            }
        }

        private static Button CreateDialogButton(DialogResult buttonType, int tabIndex = 0)
            => new Button
            {
                DialogResult = buttonType,
                Text = buttonType.ToString(),
                Width = 10,
                Margin = new Thickness(1, 0, 0, 0),
                TabIndex = tabIndex,
            };

        private static void DialogFormKeyPressed(object sender, Common.EventArgs.KeyPressedEventArgs e)
        {
            if (e.KeyInfo.Key == ConsoleKey.LeftArrow)
            {
                var wnd = sender as Window;
                wnd?.InputAction(new ConsoleKeyInfo(default, ConsoleKey.Tab, true, false, false));
            }

            if (e.KeyInfo.Key == ConsoleKey.RightArrow)
            {
                var wnd = sender as Window;
                wnd?.InputAction(new ConsoleKeyInfo(default, ConsoleKey.Tab, false, false, false));
            }
        }
    }
}
