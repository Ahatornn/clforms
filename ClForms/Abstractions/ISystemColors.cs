using ClForms.Themes;

namespace ClForms.Abstractions
{
    /// <summary>
    /// Color of a Windows display element
    /// </summary>
    public interface ISystemColors
    {
        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the element's border
        /// </summary>
        Color BorderColor { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the background in the client area of a window
        /// </summary>
        Color WindowBackground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the text in the client area of a window
        /// </summary>
        Color WindowForeground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the background in the scroll
        /// </summary>
        Color ScrollBackground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the text in the scroll
        /// </summary>
        Color ScrollForeground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the background in the client area of a dialog window
        /// </summary>
        Color DialogWindowBackground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the text in the client area of a dialog window
        /// </summary>
        Color DialogWindowForeground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of a menu's background
        /// </summary>
        Color MenuBackground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of a menu's text.
        /// </summary>
        Color MenuForeground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of a menu's mnemonic text.
        /// </summary>
        Color MnemonicForeground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the console background screen
        /// </summary>
        Color ScreenBackground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the console text
        /// </summary>
        Color ScreenForeground { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the face color of a button
        /// </summary>
        Color ButtonFace { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of a button's text
        /// </summary>
        Color ButtonText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the face color of an inactive button
        /// </summary>
        Color ButtonInactiveFace { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of an inactive button's text
        /// </summary>
        Color ButtonInactiveText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the face color of an inactive menu
        /// </summary>
        Color MenuInactiveFace { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of an inactive menu's text
        /// </summary>
        Color MenuInactiveText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the face color of a focused button
        /// </summary>
        Color ButtonFocusedFace { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of a focused button's text
        /// </summary>
        Color ButtonFocusedText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the background of selected items
        /// </summary>
        Color Highlight { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of the text of selected items
        /// </summary>
        Color HighlightText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color used to highlight menu items when the menu appears as a flat menu
        /// </summary>
        Color MenuHighlight { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color used to highlight menu's text items when the menu appears as a flat menu
        /// </summary>
        Color MenuHighlightText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the face color of an element
        /// </summary>
        Color ControlFace { get; }
        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of text in an element
        /// </summary>
        Color ControlText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the face color of a focused element
        /// </summary>
        Color ControlFocusedFace { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of text in a focused element
        /// </summary>
        Color ControlFocusedText { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the face color of an inactive element
        /// </summary>
        Color ControlInactiveFace { get; }

        /// <summary>
        /// Gets a <see cref="Color"/> structure that is the color of an inactive element's text
        /// </summary>
        Color ControlInactiveText { get; }
    }
}
