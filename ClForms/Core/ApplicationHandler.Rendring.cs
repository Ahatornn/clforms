using System.Text;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core.Contexts;
using ClForms.Core.Models;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Core
{
    internal partial class ApplicationHandler
    {
        private void ReleaseDrawingContext(WindowParameters wndParams, ScreenDrawingContext bufferContext)
        {
            pseudographicsProvider.CursorVisible = false;

            // Run on all controls and for those who have !IsVisualValid calling BeforeRender
            DetectVisualInvalidate(wndParams.Window, Point.Empty, bufferContext);

            pseudographicsProvider.SetCursorPosition(0, 0);
            pseudographicsProvider.BackgroundColor = systemColors.WindowBackground;
            pseudographicsProvider.ForegroundColor = systemColors.WindowForeground;

            // Now we are looking for a control that the cursor will work for and put it there
            if (wndParams.Window?.FocusableControl is ICursorAdmit cursorControl &&
                wndParams.Window?.FocusableControl is Control control)
            {
                var cursorPosition = control.Location + cursorControl.CursorPosition;
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
            var previousWndSize = wndParams.Window.Bounds;
            if (shouldMeasure)
            {
                PrepareWindow(wndParams);
            }

            // 3- if it needed re-render, or re-size was called, call ReleaseDrawingContext
            if (shouldMeasure || shouldRender)
            {
                var bufferForRender = new ScreenDrawingContext(wndParams.Window.Bounds);
                bufferForRender.Release(Color.NotSet, Color.NotSet);
                ReleaseDrawingContext(wndParams, bufferForRender);
                TransferToScreen(wndParams, bufferForRender, wndParams.Window.Bounds != previousWndSize);
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
        private void DetectVisualInvalidate(Control element,
            Point startPoint,
            ScreenDrawingContext bufferContext)
        {
            if (element == null)
            {
                return;
            }

            if (!element.IsVisualValid)
            {
                element.BeforeRender();
            }
            InvalidateScreenArea(element.DrawingContext, startPoint, bufferContext);

            if (element is ContentControl contentControl)
            {
                foreach (var control in contentControl)
                {
                    var location = control.Location + startPoint;
                    DetectVisualInvalidate(control, location, bufferContext);
                }
            }
        }

        /// <summary>
        /// Redrawing a piece of the screen
        /// </summary>
        private void InvalidateScreenArea(IDrawingContext ctx,
            Point location,
            ScreenDrawingContext bufferContext)
        {
            if (ctx.ContextBounds.HasEmptyDimension())
            {
                return;
            }

            var strBuilder = new StringBuilder(ctx.ContextBounds.Width);
            var colorPoint = ctx.GetColorPoint(0, 0);

            for (var row = 0; row < ctx.ContextBounds.Height; row++)
            {
                strBuilder.Clear();

                bufferContext.SetCursorPos(new Point(location.X, row + location.Y));

                for (var col = 0; col < ctx.ContextBounds.Width; col++)
                {
                    var currentPoint = ctx.GetColorPoint(col, row);
                    if (currentPoint == colorPoint)
                    {
                        strBuilder.Append(ctx.Chars[col, row]);
                    }
                    else
                    {
                        if (strBuilder.Length > 0)
                        {
                            bufferContext.DrawText(strBuilder.ToString(), colorPoint.Background,
                                colorPoint.Foreground);
                            strBuilder.Clear();
                        }
                        colorPoint = currentPoint;
                        strBuilder.Append(ctx.Chars[col, row]);
                    }
                }

                if (strBuilder.Length > 0)
                {
                    bufferContext.DrawText(strBuilder.ToString(),
                        colorPoint.Background,
                        colorPoint.Foreground);
                }
            }
        }

        private void TransferToScreen(WindowParameters windowParameters, ScreenDrawingContext bufferForRender, bool shouldBackgroundRender)
        {
            var targetBuffer = shouldBackgroundRender
                ? windowParameters.ParentContext.Merge(windowParameters.Window.Location, bufferForRender)
                : bufferForRender;
            var targetLocation = shouldBackgroundRender
                ? Point.Empty
                : windowParameters.Window.Location;

            var strBuilder = new StringBuilder(targetBuffer.ContextBounds.Width);
            var colorPoint = targetBuffer.GetColorPoint(0, 0);
            for (var row = 0; row < targetBuffer.ContextBounds.Height; row++)
            {
                strBuilder.Clear();

                pseudographicsProvider.SetCursorPosition(targetLocation.X, row + targetLocation.Y);
                for (var col = 0; col < targetBuffer.ContextBounds.Width; col++)
                {
                    var currentPoint = targetBuffer.GetColorPoint(col, row);
                    if (currentPoint == colorPoint)
                    {
                        strBuilder.Append(targetBuffer.Chars[col, row]);
                    }
                    else
                    {
                        if (strBuilder.Length > 0)
                        {
                            if(colorPoint.Background != Color.NotSet && colorPoint.Foreground != Color.NotSet)
                            {
                                pseudographicsProvider.BackgroundColor = colorPoint.Background;
                                pseudographicsProvider.ForegroundColor = colorPoint.Foreground;
                                pseudographicsProvider.Write(strBuilder.ToString());
                            }
                            else
                            {
                                pseudographicsProvider.SetCursorPosition(col + targetLocation.X, row + targetLocation.Y);
                            }
                            strBuilder.Clear();
                        }
                        colorPoint = currentPoint;
                        strBuilder.Append(targetBuffer.Chars[col, row]);
                    }
                }

                if (strBuilder.Length > 0)
                {
                    if (colorPoint.Background != Color.NotSet && colorPoint.Foreground != Color.NotSet)
                    {
                        pseudographicsProvider.BackgroundColor = colorPoint.Background;
                        pseudographicsProvider.ForegroundColor = colorPoint.Foreground;
                        pseudographicsProvider.Write(strBuilder.ToString());
                    }
                }
            }
            pseudographicsProvider.SetCursorPosition(0, 0);
            windowParameters.CurrentBuffer = bufferForRender;
        }
    }
}
