using ClForms.Abstractions;
using ClForms.Abstractions.Core;
using ClForms.Common;

namespace ClForms.Core
{
    /// Default implementation of <inheritdoc cref="IEnvironment"/>
    internal class DefaultEnvironment: IEnvironment
    {
        private readonly IPseudographicsProvider pseudographicsProvider;
        private string checkStateChars = "□√■";
        private string radioCheckChars = "●○";
        private string menuSeparatorBorder = "├─┨";

        /// <summary>
        /// Initialize a new instance <see cref="DefaultEnvironment"/>
        /// </summary>
        public DefaultEnvironment(IPseudographicsProvider pseudographicsProvider)
        {
            this.pseudographicsProvider = pseudographicsProvider;
            BorderChars = new BorderChars('┌', '─', '┒', '│', '┃',
                '┕', '━', '┛');
        }

        /// <inheritdoc cref="IPseudographicsProvider.WindowWidth"/>
        public int WindowWidth => pseudographicsProvider.WindowWidth;

        /// <inheritdoc cref="IPseudographicsProvider.WindowHeight"/>
        public int WindowHeight => pseudographicsProvider.WindowHeight;

        /// <inheritdoc cref="IEnvironment.BorderChars"/>
        public BorderChars BorderChars { get; }

        /// <inheritdoc cref="IEnvironment.GetCheckStateChar"/>
        public char GetCheckStateChar(CheckState state) => checkStateChars[(int) state];

        /// <inheritdoc cref="IEnvironment.GetRadioCheckChar"/>
        public char GetRadioCheckChar(bool isChecked) => radioCheckChars[isChecked ? 0 : 1];

        /// <inheritdoc cref="IEnvironment.MenuSeparatorLeftBorder"/>
        public char MenuSeparatorLeftBorder => menuSeparatorBorder[0];

        /// <inheritdoc cref="IEnvironment.MenuSeparatorRightBorder"/>
        public char MenuSeparatorRightBorder => menuSeparatorBorder[2];

        /// <inheritdoc cref="IEnvironment.MenuSeparatorBorder"/>
        public char MenuSeparatorBorder => menuSeparatorBorder[1];

        /// <inheritdoc cref="IEnvironment.MenuSubItemsMarker"/>
        public char MenuSubItemsMarker { get; } = '►';
    }
}
