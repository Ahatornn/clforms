using System;
using System.Text;
using ClForms.Themes;

namespace ClForms.Abstractions.Core
{
    /// <summary>
    /// Represents the standard input, output, and error streams for pseudographics interface
    /// </summary>
    public interface IPseudographicsProvider
    {
        /// <summary>
        /// Sets the position of the cursor
        /// </summary>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current
        /// line terminator, to the standard output stream
        /// </summary>
        void WriteLine(object value);

        /// <summary>
        /// Writes the specified Unicode character value to the standard output stream
        /// </summary>
        void Write(char value);

        /// <summary>
        /// Writes the specified string value to the standard output stream
        /// </summary>
        void Write(string value);

        /// <summary>
        /// Gets or sets the height of the console window area
        /// </summary>
        int WindowHeight { get; set; }

        /// <summary>
        /// Gets or sets the width of the console window
        /// </summary>
        int WindowWidth { get; set; }

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream
        /// </summary>
        bool KeyAvailable { get; }

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key
        /// is optionally displayed in the console window
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to
        /// not display the pressed key; otherwise, false
        /// </param>
        ConsoleKeyInfo ReadKey(bool intercept);

        /// <summary>
        /// Gets or sets the title to display in the console title bar
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the encoding the console uses to write output
        /// </summary>
        Encoding OutputEncoding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets or sets the background color of the pseudographics interface
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the pseudographics interface
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Clears the console buffer and corresponding console window of display information
        /// </summary>
        void Clear();
    }
}
