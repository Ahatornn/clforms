using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core.Models;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using System;

namespace ClForms.Core
{
    internal partial class ApplicationHandler
    {
        private void ReleaseDrawingContext(WindowParameters wndParams)
        {
            pseudographicsProvider.CursorVisible = false;
            // Run on all controls and for those who have !IsVisualValid calling BeforeRender
            DetectVisualInvalidate(wndParams.Window, Guid.NewGuid(), wndParams.Window.Location, wndParams);
            pseudographicsProvider.SetCursorPosition(0, 0);
            pseudographicsProvider.BackgroundColor = systemColors.WindowBackground;
            pseudographicsProvider.ForegroundColor = systemColors.WindowForeground;
            // Now we are looking for a control that the cursor will work for and put it there
            if (wndParams.Window?.FocusableControl is ICursorAdmit control)
            {
                var cursorPosition =
                    GetFocusableCursorPosition(wndParams.Window.FocusableControl.Id, control.CursorPosition);
                pseudographicsProvider.SetCursorPosition(cursorPosition.X, cursorPosition.Y);
                pseudographicsProvider.CursorVisible = true;
            }
        }

        /// <summary>
        /// Going through all the elements and looking for those that need to be re-size
        /// and re-render
        /// </summary>
        private void CheckMeasureOrVisualInvalidate(WindowParameters wndParams)
        {
            if (wndParams?.Window == null || wndParams.Window.WasClosed)
            {
                return;
            }
            // 1-Check whether there is anything to re-size or re-render (run to the first 
            // !IsMeasureValid or all trying to find at least one !IsVisualValid)
            var shouldRender = false;
            var shouldMeasure = DetectMeasureInvalidate(wndParams.Window, ref shouldRender);

            // 2- If it needed re-size, call Measure and Arrange
            if (shouldMeasure)
            {
                PrepareWindow(wndParams.Window);
            }

            // 3- if it needed re-render, or re-size was called, call ReleaseDrawingContext
            if (shouldMeasure || shouldRender)
            {
                ReleaseDrawingContext(wndParams);
            }
        }

        /// <summary>
        /// Going through all the elements and looking for those that need to be re-size
        /// </summary>
        private static bool DetectMeasureInvalidate(Control element, ref bool shouldRender)
        {
            if (element == null)
            {
                return false;
            }
            if (!element.IsVisualValid)
            {
                shouldRender = true;
            }
            if (!element.IsMeasureValid)
            {
                return true;
            }

            if (element is ContentControl contentControl)
            {
                foreach (var control in contentControl)
                {
                    var result = DetectMeasureInvalidate(control, ref shouldRender);
                    if (result)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Going through all the elements and looking for those that need to be re-rendered
        /// </summary>
        private void DetectVisualInvalidate(Control element, Guid renderSessionId, Point startPoint, WindowParameters wndParams)
        {
            if (element == null)
            {
                return;
            }

            if (!element.IsVisualValid)
            {
                element.BeforeRender(renderSessionId, GetHashCodeHelper.CalculateHashCode(element));
                InvalidateScreenArea(element.DrawingContext, startPoint, wndParams);
            }

            if (element is ContentControl contentControl)
            {
                foreach (var control in contentControl)
                {
                    var location = control.Location + startPoint;
                    DetectVisualInvalidate(control, renderSessionId, location, wndParams);
                }
            }
        }

        /// <summary>
        /// Redrawing a piece of the screen
        /// </summary>
        private void InvalidateScreenArea(IDrawingContext ctx, Point location, WindowParameters wndParams)
        {
            throw new NotImplementedException();
        }
    }
}
