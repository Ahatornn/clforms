using ClForms.Elements;

namespace ClForms.Common
{
    /// <summary>
    /// Specifies the case of characters in a <see cref="TextBox"/> control
    /// </summary>
    public enum CharacterCasing
    {
        /// <summary>
        /// Converts all characters to lowercase
        /// </summary>
        Lower,

        /// <summary>
        /// The case of characters is left unchanged
        /// </summary>
        Normal,

        /// <summary>
        /// Converts all characters to uppercase
        /// </summary>
        Upper
    }
}
