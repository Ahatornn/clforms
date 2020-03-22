using ClForms.Common;

namespace ClForms.Abstractions
{
    /// <summary>
    /// Provides information about, and means to manipulate, the current environment and platform
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Gets or sets the width of the console window
        /// </summary>
        int WindowWidth { get; }

        /// <summary>
        /// Gets or sets the height of the console window area
        /// </summary>
        int WindowHeight { get; }

        /// <summary>
        /// Defines border characters
        /// </summary>
        BorderChars BorderChars { get; }

        /// <summary>
        /// Gets presenter char of <see cref="CheckState"/>
        /// </summary>
        char GetCheckStateChar(CheckState state);

        /// <summary>
        /// Gets left char border of menu separator
        /// </summary>
        char MenuSeparatorLeftBorder { get; }

        /// <summary>
        /// Gets right char border of menu separator
        /// </summary>
        char MenuSeparatorRightBorder { get; }

        /// <summary>
        /// Gets char border of menu separator
        /// </summary>
        char MenuSeparatorBorder { get; }

        /// <summary>
        /// Gets a value indicating whether the menu item has sub menu items
        /// </summary>
        char MenuSubItemsMarker { get; }
    }
}
