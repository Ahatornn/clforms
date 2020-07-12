using System;
using System.Linq;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Core;
using ClForms.Elements.Abstractions;
using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// A group of related but mutually exclusive radio buttons, requiring the user to select one of a set of alternatives.
    /// As one button becomes selected, the remaining buttons in the group become automatically deselected
    /// </summary>
    public class RadioGroup: GroupItemControl, IElementStyle<RadioGroup>
    {
        private int selectedIndex = -1;

        /// <summary>
        /// Initialize a new instance <see cref="RadioGroup"/>
        /// </summary>
        public RadioGroup()
        {
            Background = Color.NotSet;
            Foreground = Color.NotSet;
            DisabledForeground = Application.SystemColors.ButtonInactiveFace;
            FocusForeground = Application.SystemColors.ButtonFocusedFace;
        }

        #region Properties

        #region SelectedIndex

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in a <see cref="RadioGroup"/>
        /// </summary>
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex != value)
                {
                    if (selectedIndex < -1 && selectedIndex > Items.Count)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    OnSelectedIndexChanged?.Invoke(this, new PropertyChangedEventArgs<int>(selectedIndex, value));
                    selectedIndex = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        /// <inheritdoc />
        protected override int ContentMeasureItemIndent => 3;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnContentRender(IDrawingContext context)
        {
            if (focusedIndex == -1)
            {
                var visibleItems = chunks.AllList;
                var firstElement = visibleItems.FirstOrDefault(x => !x.IsDisabled);
                if (firstElement != null)
                {
                    focusedIndex = visibleItems.IndexOf(firstElement);
                }
            }
            var renderArea = new Rect(context.ContextBounds.Size)
                .Reduce(BorderThickness)
                .Reduce(Padding);
            var leftIndent = renderArea.Left + (renderArea.Width - contentSize.Width) / 2;
            var topIndent = renderArea.Top + (renderArea.Height - contentSize.Height) / 2;

            for (var col = 0; col < chunks.Count; col++)
            {
                var items = chunks[col].ToList();
                for (var i = 0; i < items.Count; i++)
                {
                    if (!renderArea.Contains(leftIndent, topIndent + i))
                    {
                        break;
                    }
                    var foreColor = IsDisabled || items[i].IsDisabled
                        ? DisabledForeground
                        : IsFocus && Items.IndexOf(items[i]) == focusedIndex
                            ? FocusForeground
                            : Foreground;
                    context.SetCursorPos(leftIndent, topIndent + i);
                    context.DrawText($"{GetGlyph(Items.IndexOf(items[i]) == SelectedIndex)}  {items[i].Text}", foreColor);
                }
                leftIndent += items.Max(x => x.Text.Length + 3) + 2;
            }
        }

        /// <inheritdoc />
        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            base.InputActionInternal(keyInfo);

            if (keyInfo.Key == ConsoleKey.Spacebar || keyInfo.Key == ConsoleKey.Enter)
            {
                var element = Items[focusedIndex];
                if (!element.IsDisabled)
                {
                    SelectedIndex = Items.IndexOf(element);
                }
            }
        }

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<RadioGroup> styleAction) => styleAction?.Invoke(this);

        private char GetGlyph(bool isChecked) => Application.Environment.GetRadioCheckChar(isChecked);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<int>> OnSelectedIndexChanged;

        #endregion
    }
}
