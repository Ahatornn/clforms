using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Elements.Abstractions;

namespace ClForms.Elements
{
    /// <summary>
    /// Base button for <see cref="StatusBar"/> control
    /// </summary>
    public class StatusBarButton: ButtonBase, IElementStyle<StatusBarButton>
    {
        private readonly StatusBar owner;

        /// <summary>
        /// Initialize a new instance <see cref="StatusBarButton"/>
        /// </summary>
        public StatusBarButton(StatusBar owner, StatusBarShortcutKey shortcutKey)
        {
            this.owner = owner;
            Background = owner.Background;
            Foreground = owner.Foreground;
            StatusBarShortcutKey = shortcutKey;
            Padding = Thickness.Empty;
        }

        /// <summary>
        /// Initialize a new instance <see cref="StatusBarButton"/>
        /// </summary>
        public StatusBarButton(StatusBar owner, StatusBarShortcutKey shortcutKey, string text)
            : this(owner, shortcutKey)
        {
            Text = text;
        }

        #region Properties

        /// <summary>
        /// Gets of the <see cref="StatusBarShortcutKey"/>
        /// </summary>
        public StatusBarShortcutKey StatusBarShortcutKey { get; }

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<StatusBarButton> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc cref="ButtonBase.GetTextPresenter"/>
        protected override string GetTextPresenter() => $"{StatusBarShortcutKey.ToString()} {Text}";

        /// <summary>
        /// Call event of the <see cref="ButtonBase.OnClick" />
        /// </summary>
        public void Click() => Click(EventArgs.Empty);

        /// <inheritdoc cref="ButtonBase.OnRender"/>
        protected override void OnRender(IDrawingContext context)
        {
            base.OnRender(context);

            var reducedArea = context.ContextBounds.Reduce(Padding);
            if (!reducedArea.HasEmptyDimension() ||
                string.IsNullOrWhiteSpace(GetTextPresenter()))
            {
                context.SetCursorPos(RenderTextPosition.X + Padding.Left, RenderTextPosition.Y + Padding.Top);

                var foreColor = IsDisabled
                    ? DisabledForeground
                    : owner.MnemonicForeground;
                context.DrawText(StatusBarShortcutKey.ToString(), foreColor);

                foreColor = IsDisabled
                    ? DisabledForeground
                    : Foreground;
                context.DrawText($" {Text}", foreColor);
            }
        }

        #endregion
    }
}
