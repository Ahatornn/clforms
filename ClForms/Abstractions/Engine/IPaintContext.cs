using ClForms.Common;
using ClForms.Themes;

namespace ClForms.Abstractions.Engine
{
    /// <summary>
    /// Custom drawing context
    /// </summary>
    public interface IPaintContext
    {
        /// <summary>
        /// Specifies the coordinates of the start of drawing
        /// </summary>
        void SetCursorPos(int x, int y);

        /// <summary>
        /// Specifies the coordinates of the start of drawing
        /// </summary>
        void SetCursorPos(Point point);

        /// <summary>
        /// Draws text in current coordinates and with current background and font colors
        /// </summary>
        void DrawText(string text);

        /// <summary>
        /// Draws symbol in current coordinates and with current background and font colors
        /// </summary>
        void DrawText(char @char);

        /// <summary>
        /// Draws text in the current coordinates and with the current background and specified font color
        /// </summary>
        void DrawText(string text, Color foregroundColor);

        /// <summary>
        /// Draws symbol in the current coordinates and with the current background and specified font color
        /// </summary>
        void DrawText(char @char, Color foregroundColor);

        /// <summary>
        /// Draws a symbol in current coordinates with the specified background and font colors
        /// </summary>
        void DrawText(char @char, Color backgroundColor, Color foregroundColor);

        /// <summary>
        /// Draws a text in current coordinates with the specified background and font colors
        /// </summary>
        void DrawText(string text, Color backgroundColor, Color foregroundColor);

        /// <summary>
        /// Gets a <see cref="ContextColorPoint"/> in point <see cref="col"/> and <see cref="row"/>
        /// </summary>
        ContextColorPoint GetColorPoint(int col, int row);
    }
}
