using System.Collections.Generic;
using System.Linq;
using ClForms.Abstractions;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Abstractions;

namespace ClForms.Helpers
{
    /// <summary>
    /// Helper for focusable controls
    /// </summary>
    internal static class FocusableHelper
    {
        /// <summary>
        /// Set a next focusable control
        /// </summary>
        internal static void SetFocusableControl(Window window, Direction direction)
        {
            var oldControl = window.FocusableControl;
            var controls = FindAllFocusableControls(window.Content, 0)
                .Where(x => x.control != null)
                .OrderBy(x => x.level)
                .ThenByDescending(x => x.control.TabIndex)
                .Select(x => x.control)
                .ToList();
            if (controls.Any())
            {
                if (window.FocusableControl == null)
                {
                    window.FocusableControl = controls.FirstOrDefault();
                }
                else
                {
                    var targetIndex = controls.FindIndex(x => x.Id == window.FocusableControl.Id) + (int) direction;
                    if (targetIndex < 0)
                    {
                        window.FocusableControl = controls.LastOrDefault();
                    }
                    else if (targetIndex >= controls.Count)
                    {
                        window.FocusableControl = controls.FirstOrDefault();
                    }
                    else
                    {
                        window.FocusableControl = controls[targetIndex];
                    }
                }
            }

            if (oldControl != null)
            {
                oldControl.IsFocus = false;
            }

            if (window.FocusableControl != null)
            {
                window.FocusableControl.IsFocus = true;
            }
        }

        private static IEnumerable<(int level, IFocusableControl control)> FindAllFocusableControls(Control element, int level)
        {
            if (element == null)
            {
                yield break;
            }

            if (element is ContentControl contentControl)
            {
                foreach (var control in contentControl)
                {
                    if (control is IFocusableControl ctrl)
                    {
                        if (ctrl.CanFocus())
                        {
                            yield return (level, ctrl);
                        }
                    }
                    else
                    {
                        var result = FindAllFocusableControls(control, level++);
                        foreach (var internalControl in result)
                        {
                            yield return internalControl;
                        }
                    }
                }
            }
            else if (element is IFocusableControl fControl && fControl.CanFocus())
            {
                yield return (level, fControl);
            }
        }
    }
}
