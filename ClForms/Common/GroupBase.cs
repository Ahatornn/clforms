using System;
using ClForms.Abstractions.Engine;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Common
{
    internal class GroupBase
    {
        internal string text;
        internal TextAlignment textAlignment;
        internal Thickness borderThickness;
        internal Color borderColor;
        internal BorderChars borderChars;
        internal readonly Control targetControl;
        internal Rect bounds = Rect.Empty;

        internal GroupBase(Control targetControl)
        {
            this.targetControl = targetControl;
            borderThickness = new Thickness(1);
            borderChars = Application.Environment.BorderChars;
            TextArea = Rect.Empty;
        }

        internal Rect TextArea { get; private set; }

        internal Size Measure(Size availableSize, Func<Size, Size> contentMeasureDelegate, bool applyDelegate)
        {
            var marginWithBorder = targetControl.Margin + borderThickness;
            var contentArea = new Rect(new Size(availableSize.Width, availableSize.Height))
                .Reduce(targetControl.Margin)
                .Reduce(borderThickness)
                .Reduce(targetControl.Padding);
            contentArea.Width = targetControl.Width ?? contentArea.Width;
            contentArea.Height = targetControl.Height ?? contentArea.Height;
            var desiredContentSize = contentArea.Size;

            if (!desiredContentSize.HasEmptyDimension() && applyDelegate)
            {
                var size = contentMeasureDelegate.Invoke(desiredContentSize);
                size.Width = Math.Min(targetControl.Width.HasValue
                        ? targetControl.Width.Value + targetControl.Margin.Horizontal
                        : size.Width + (marginWithBorder + targetControl.Padding).Horizontal,
                    availableSize.Width);
                size.Height = Math.Min(targetControl.Height.HasValue
                        ? targetControl.Height.Value + targetControl.Margin.Vertical
                        : size.Height + (marginWithBorder + targetControl.Padding).Vertical,
                    availableSize.Height);

                return size;
            }

            return new Size(
                Math.Min((targetControl.Width ?? targetControl.Padding.Horizontal) + marginWithBorder.Horizontal,
                    availableSize.Width),
                Math.Min((targetControl.Height ?? targetControl.Padding.Vertical) + marginWithBorder.Vertical,
                    availableSize.Height));
        }

        internal Rect Arrange(Rect finalRect, Action<Rect> contentArrangeDelegate, bool applyDelegate)
        {
            var offset = targetControl.Margin + targetControl.Padding + borderThickness;
            bounds = finalRect.Reduce(targetControl.Margin);
            var clientRect = new Rect((targetControl.Padding + borderThickness).Left,
                (targetControl.Padding + borderThickness).Top,
                finalRect.Width - offset.Horizontal,
                finalRect.Height - offset.Vertical);
            if (applyDelegate)
            {
                contentArrangeDelegate.Invoke(clientRect);
            }

            RecalculateTextPosition();
            return finalRect;
        }

        internal void RecalculateTextPosition()
        {
            if (!(string.IsNullOrWhiteSpace(text) || bounds.HasEmptyDimension()))
            {
                var presenterText = text.Length > bounds.Width - borderThickness.Horizontal
                    ? text.Substring(0, bounds.Width - borderThickness.Horizontal - 1) + "…"
                    : text;
                switch (textAlignment)
                {
                    case TextAlignment.Right:
                        TextArea = new Rect(bounds.Width - borderThickness.Right - presenterText.Length,
                            borderThickness.Top - 1, text.Length, 1);
                        break;
                    case TextAlignment.Center:
                        TextArea = new Rect((bounds.Width - borderThickness.Horizontal - presenterText.Length) / 2 + 1,
                            borderThickness.Top - 1, text.Length, 1);
                        break;
                    default:
                        TextArea = new Rect(borderThickness.Left, borderThickness.Top - 1, text.Length, 1);
                        break;
                }
            }
            else
            {
                TextArea = Rect.Empty;
            }
        }

        internal void OnRender(IDrawingContext context)
        {
            if (borderThickness.Top > 0)
            {
                context.SetCursorPos(borderThickness.Left - 1, borderThickness.Top - 1);
                var topStr = borderChars.TopLeft +
                             (context.ContextBounds.Width - borderThickness.Horizontal > 0
                                 ? new string(borderChars.TopMiddle,
                                     context.ContextBounds.Width - borderThickness.Horizontal)
                                 : string.Empty) +
                             borderChars.TopRight;
                context.DrawText(topStr, borderColor);
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                var presenterText = text.Length > context.ContextBounds.Width - borderThickness.Horizontal
                    ? text.Substring(0, context.ContextBounds.Width - borderThickness.Horizontal - 1) + "…"
                    : text;

                DrawHeaderText(context, presenterText, TextArea.X, TextArea.Y);
            }

            for (var row = borderThickness.Top; row < context.ContextBounds.Height - borderThickness.Bottom; row++)
            {
                if (borderThickness.Left > 0)
                {
                    context.SetCursorPos(borderThickness.Left - 1, row);
                    context.DrawText(borderChars.MiddleLeft, borderColor);
                }

                if (borderThickness.Right > 0)
                {
                    context.SetCursorPos(context.ContextBounds.Width - borderThickness.Right, row);
                    context.DrawText(borderChars.MiddleRight, borderColor);
                }
            }

            if (borderThickness.Bottom > 0)
            {
                context.SetCursorPos(borderThickness.Left - 1,
                    context.ContextBounds.Height - borderThickness.Bottom);
                var bottomStr = borderChars.BottomLeft +
                                (context.ContextBounds.Width - borderThickness.Horizontal > 0
                                    ? new string(borderChars.BottomMiddle,
                                        context.ContextBounds.Width - borderThickness.Horizontal)
                                    : string.Empty) +
                                borderChars.BottomRight;
                context.DrawText(bottomStr, borderColor);
            }
        }

        internal virtual void DrawHeaderText(IDrawingContext context, string presenterText, int x, int y)
        {
            context.SetCursorPos(x, y);
            context.DrawText(presenterText, borderColor);
        }
    }
}
