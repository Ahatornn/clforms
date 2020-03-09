using System;
using System.Collections.Generic;
using System.Linq;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements;
using ClForms.Elements.Abstractions;

namespace ClForms.Helpers
{
    /// <summary>
    /// Wrapper for dialog window
    /// </summary>
    internal class WindowDialogWrapper: IDisposable
    {
        private readonly Window window;

        internal WindowDialogWrapper(Window window)
        {
            this.window = window;
            window.OnKeyPressed += DialogKeyPressed;
        }

        private void DialogKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.KeyInfo.Key == ConsoleKey.Escape)
            {
                window.DialogResult = DialogResult.Cancel;
            }

            if (e.KeyInfo.Key == ConsoleKey.Enter)
            {
                var btnDialogResult = FirstPositiveButtonOrDefault();
                if (btnDialogResult.HasValue)
                {
                    window.DialogResult = btnDialogResult.Value;
                }
            }
        }

        private DialogResult? FirstPositiveButtonOrDefault()
        {
            var results = GetAllDialogResults(window).ToList();
            if (!results.Any())
            {
                return null;
            }
            if (results.Any(x => x == DialogResult.Yes))
            {
                return DialogResult.Yes;
            }

            if (results.Any(x => x == DialogResult.OK))
            {
                return DialogResult.OK;
            }

            if (results.Any(x => x == DialogResult.Abort))
            {
                return DialogResult.Abort;
            }

            if (results.Any(x => x == DialogResult.Retry))
            {
                return DialogResult.Retry;
            }

            return null;
        }

        private IEnumerable<DialogResult> GetAllDialogResults(Control element)
        {
            if (element == null)
            {
                yield break;
            }

            if (element is ContentControl contentControl)
            {
                foreach (var control in contentControl)
                {
                    if (control is Button ctrl && !ctrl.IsDisabled)
                    {
                        yield return ctrl.DialogResult;
                    }
                    else
                    {
                        var result = GetAllDialogResults(control);
                        foreach (var internalControl in result)
                        {
                            yield return internalControl;
                        }
                    }
                }
            }
            else if (element is Button fControl && !fControl.IsDisabled)
            {
                yield return fControl.DialogResult;
            }
        }

        /// <inheritdoc />
        public void Dispose() => window.OnKeyPressed -= DialogKeyPressed;
    }
}
