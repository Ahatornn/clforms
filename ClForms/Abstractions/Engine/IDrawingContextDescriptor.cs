using ClForms.Common;
using ClForms.Themes;

namespace ClForms.Abstractions.Engine
{
    /// <summary>
    /// Common parameters of drawing context
    /// </summary>
    internal interface IDrawingContextDescriptor
    {
        IGraphicsDevice<Color> Background { get; }
        IGraphicsDevice<Color> Foreground { get; }
        IGraphicsDevice<char> Chars { get; }
        Rect ContextBounds { get; }
        void SetValues(int col, int row, Color backColor, Color foreColor, char @char, bool ignoreEmpty = false);
    }
}
