using System;
using System.Text;
using ClForms.Helpers;

namespace ClForms.Common
{
    /// <summary>
    /// Specifies shortcuts to a menu item, including the character represented by the console
    /// keys and the status of the SHIFT, ALT, and CTRL keys
    /// </summary>
    public struct Shortcut
    {
        /// <summary>
        /// Returns the console key represented by the current <see cref="Shortcut" /> object
        /// </summary>
        public ConsoleKey Key { get; }

        /// <summary>
        /// Returns the bitwise combination <see cref = "ConsoleModifiers" /> while pressing values,
        /// which sets one or more keys with a console key.
        /// </summary>
        public ConsoleModifiers Modifiers { get; }

        /// <summary>
        /// Initialize a new instance <see cref="Shortcut"/> with value of key and other default values
        /// </summary>
        public Shortcut(ConsoleKey key)
        : this(key, false, true, false)
        {

        }

        /// <summary>
        /// Initialize a new instance <see cref="Shortcut"/>
        /// </summary>
        public Shortcut(ConsoleKey key, bool shift, bool alt, bool control)
        {
            Key = key;
            Modifiers = 0;
            if (shift)
            {
                Modifiers |= ConsoleModifiers.Shift;
            }
            if (alt)
            {
                Modifiers |= ConsoleModifiers.Alt;
            }
            if (!control)
            {
                return;
            }
            Modifiers |= ConsoleModifiers.Control;
        }

        /// <inheritdoc />
        public override bool Equals(object value)
        {
            if (value is Shortcut shortcut)
            {
                return Equals(shortcut);
            }
            return false;
        }

        /// <summary>
        /// Compare two <see cref="Shortcut" /> for equality
        /// </summary>
        public bool Equals(Shortcut obj)
        {
            if (obj.Key == Key)
            {
                return obj.Modifiers == Modifiers;
            }

            return false;
        }

        /// <summary>
        /// Compare two <see cref="Shortcut" /> for equality
        /// </summary>
        public static bool operator ==(Shortcut a, Shortcut b) => a.Equals(b);

        /// <summary>
        /// Compare two <see cref="Rect" /> for inequality
        /// </summary>
        public static bool operator !=(Shortcut a, Shortcut b) => !(a == b);

        /// <inheritdoc />
        public override int GetHashCode() => (int) ((ConsoleModifiers) Key | Modifiers);

        /// <inheritdoc />
        public override string ToString()
        {
            var result = new StringBuilder();
            if (Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                result.Append("Ctrl+");
            }

            if (Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                result.Append("Shift+");
            }

            if (Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                result.Append("Alt+");
            }

            result.Append(Key.ToPopupMenuString());

            return result.ToString();
        }
    }
}
