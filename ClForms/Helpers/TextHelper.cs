using System;
using System.Collections.Generic;
using System.Linq;
using ClForms.Common;

namespace ClForms.Helpers
{
    internal class TextHelper
    {
        internal static IEnumerable<string> GetParagraph(string text, int width)
        {
            var words = GetStringWords(text, width).ToArray();
            var sentences = string.Empty;
            foreach (var word in words)
            {
                if ((sentences + word).Length > width)
                {
                    yield return sentences.TrimEnd();
                    sentences = string.Empty;
                }
                sentences += word;
                if (sentences.Length + 1 < width)
                {
                    sentences += " ";
                }
            }

            if (sentences.Length > 0)
            {
                yield return sentences.TrimEnd();
            }
        }

        internal static string GetTextWithAlignment(string text, int width, TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    var leftIndent = (width - text.Length) / 2;
                    return new string(' ', leftIndent) + text + new string(' ', width - leftIndent - text.Length);
                case TextAlignment.Right:
                    return string.Format("{0," + $"{width}}}", text);
                default:
                    return string.Format("{0," + $"{width * -1}}}", text);
            }
        }

        private static IEnumerable<string> GetStringWords(string text, int width)
            => text
                .Replace(Environment.NewLine, " ")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(x => DivideString(x, width));

        private static IEnumerable<string> DivideString(string text, int maxWidth)
        {
            for (var i = 0; i + maxWidth <= text.Length + maxWidth; i += maxWidth)
            {
                yield return text.Substring(i, Math.Min(maxWidth, text.Length - i));
            }
        }
    }
}
