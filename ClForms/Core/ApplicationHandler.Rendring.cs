using System;
using System.Text;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Core.Models;
using ClForms.Elements.Abstractions;
using ClForms.Helpers;
using ClForms.Themes;

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
            var parentReRendered = false;
            if (element == null)
            {
                return;
            }

            if (!element.IsVisualValid)
            {
                element.BeforeRender(renderSessionId, GetHashCodeHelper.CalculateHashCode(element));
                if (element == Application.MainWindow &&
                    wndParams.ControlContextHash.TryGetValue(element.Id, out var previousValue) &&
                    element.DrawingContext.ContextBounds != previousValue.InvalidateRect &&
                    currentWindowParams.Window.WindowState == ControlState.Normal)
                {
                    ClearScreen();
                }
                InvalidateScreenArea(element.DrawingContext, startPoint, wndParams);
                parentReRendered = true;
            }

            if (element is ContentControl contentControl)
            {
                foreach (var control in contentControl)
                {
                    if (parentReRendered)
                    {
                        wndParams.ControlContextHash.TryRemove(control.Id, out _);
                        control.InvalidateVisual();
                    }
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
            var contextHash = ctx.GetHashCode();
            var parentHash = ctx.Parent?.GetHashCode() ?? 0;
            if (wndParams.ControlContextHash.TryGetValue(ctx.ControlId, out var previousValue) &&
                previousValue.HashValue == contextHash &&
                previousValue.ParentHashValue == parentHash)
            {
                return;
            }

            var param = new InvalidateParameters(contextHash,
                parentHash,
                ctx.RenderSessionId,
                ctx.ContextBounds,
                location);
            wndParams.ControlContextHash.AddOrUpdate(ctx.ControlId, param, (key, oldValue) => param);

            if (ctx.Parent != null &&
                previousValue != null &&
                previousValue.RenderId != param.RenderId &&
                previousValue.ParentHashValue == parentHash)
            {
                var preColorPoint = new ContextColorPoint(Color.NotSet, Color.NotSet);
                SetConsoleColor(preColorPoint);
                wndParams.ControlContextHash.TryGetValue(ctx.Parent.ControlId, out var parentValue);
                for (var row = previousValue.InvalidateRect.Top; row < previousValue.InvalidateRect.Bottom; row++)
                {
                    for (var col = previousValue.InvalidateRect.Left; col < previousValue.InvalidateRect.Right; col++)
                    {
                        if (!ctx.ContextBounds.Contains(col, row))
                        {
                            var currentPoint = GetParentColorPoint(new ContextColorPoint(Color.NotSet,
                                    Color.NotSet),
                                ctx.Parent, 
                                new Point(col, row),
                                systemColors);
                            if (currentPoint != preColorPoint)
                            {
                                SetConsoleColor(currentPoint);
                            }

                            var point = new Point(col + parentValue?.Location.X ?? 0,
                                row + parentValue?.Location.Y ?? 0);

                            pseudographicsProvider.SetCursorPosition(point.X, point.Y);
                            wndParams.Context.SetCursorPos(point);

                            pseudographicsProvider.Write(ctx.Parent.Chars[col, row]);
                            wndParams.Context.DrawText(ctx.Parent.Chars[col, row], currentPoint.Background,
                                currentPoint.Foreground);

                            preColorPoint = currentPoint;
                        }
                    }
                }
            }

            if (ctx.ContextBounds.HasEmptyDimension())
            {
                return;
            }

            var strBuilder = new StringBuilder(ctx.ContextBounds.Width);
            var colorPoint = ctx.GetColorPoint(0, 0);
            SetConsoleColor(colorPoint);
            for (var row = 0; row < ctx.ContextBounds.Height; row++)
            {
                strBuilder.Clear();

                pseudographicsProvider.SetCursorPosition(location.X, row + location.Y);
                wndParams.Context.SetCursorPos(location.X, row + location.Y);

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
                            pseudographicsProvider.Write(strBuilder.ToString());
                            wndParams.Context.DrawText(strBuilder.ToString(), colorPoint.Background,
                                colorPoint.Foreground);

                            strBuilder.Clear();
                        }
                        SetConsoleColor(currentPoint);
                        colorPoint = currentPoint;
                        strBuilder.Append(ctx.Chars[col, row]);
                    }
                }

                if (strBuilder.Length > 0)
                {
                    pseudographicsProvider.Write(strBuilder.ToString());
                    wndParams.Context.DrawText(strBuilder.ToString(),
                        colorPoint.Background,
                        colorPoint.Foreground);
                }
            }
            pseudographicsProvider.SetCursorPosition(location.X, location.Y);
        }

        private static ContextColorPoint GetParentColorPoint(ContextColorPoint colors, 
            IDrawingContext context, 
            Point point,
            ISystemColors systemColors)
        {
            if (context == null)
            {
                return new ContextColorPoint(colors.Background != Color.NotSet
                        ? colors.Background
                        : systemColors.ScreenBackground,
                    colors.Foreground != Color.NotSet
                        ? colors.Foreground
                        : systemColors.ScreenForeground);
            }
            var parentColors = context.GetColorPoint(point.X, point.Y);

            var result = new ContextColorPoint(colors.Background == Color.NotSet
                    ? parentColors.Background
                    : colors.Background,
                colors.Foreground == Color.NotSet
                    ? parentColors.Foreground
                    : colors.Foreground);

            if (result.Background != Color.NotSet &&
                result.Foreground != Color.NotSet)
            {
                return result;
            }

            return GetParentColorPoint(result, 
                context.Parent, 
                point + context.ContextBounds.Location,
                systemColors);
        }
    }
}
