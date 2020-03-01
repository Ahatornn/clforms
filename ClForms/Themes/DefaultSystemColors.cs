using ClForms.Abstractions;

namespace ClForms.Themes
{
    /// <summary>
    /// Default system colors
    /// </summary>
    internal struct DefaultSystemColors: ISystemColors
    {
        /// <inheritdoc />
        Color ISystemColors.BorderColor => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.WindowBackground => Color.DarkGray;

        /// <inheritdoc />
        Color ISystemColors.WindowForeground => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.MenuBackground => Color.Gray;

        /// <inheritdoc />
        Color ISystemColors.MenuForeground => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.MnemonicForeground => Color.Red;

        /// <inheritdoc />
        Color ISystemColors.ScreenBackground => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.ScreenForeground => Color.Gray;

        /// <inheritdoc />
        Color ISystemColors.ButtonFace => Color.DarkCyan;

        /// <inheritdoc />
        Color ISystemColors.ButtonText => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.ButtonInactiveFace => Color.DarkGray;

        /// <inheritdoc />
        Color ISystemColors.ButtonInactiveText => Color.Gray;

        /// <inheritdoc />
        Color ISystemColors.MenuInactiveFace => Color.Gray;

        /// <inheritdoc />
        Color ISystemColors.MenuInactiveText => Color.DarkGray;

        /// <inheritdoc />
        Color ISystemColors.ButtonFocusedFace => Color.Cyan;

        /// <inheritdoc />
        Color ISystemColors.ButtonFocusedText => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.Highlight => Color.Blue;

        /// <inheritdoc />
        Color ISystemColors.HighlightText => Color.White;

        /// <inheritdoc />
        Color ISystemColors.MenuHighlight => Color.Cyan;

        /// <inheritdoc />
        Color ISystemColors.MenuHighlightText => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.ControlFace => Color.White;

        /// <inheritdoc />
        Color ISystemColors.ControlText => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.ControlFocusedFace => Color.White;

        /// <inheritdoc />
        Color ISystemColors.ControlFocusedText => Color.Black;

        /// <inheritdoc />
        Color ISystemColors.ControlInactiveFace => Color.DarkGray;

        /// <inheritdoc />
        Color ISystemColors.ControlInactiveText => Color.Gray;
    }
}
