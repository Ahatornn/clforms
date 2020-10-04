using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClForms.Common;

namespace ClForms.Helpers
{
    internal class TextHelper
    {
        internal static IEnumerable<string> GetParagraph(string text, int width)
        {
            foreach (var lines in text.Split(new []{ Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                var words = GetStringWords(ChangeDoubleSpaces(lines), width).ToArray();
                var sentences = string.Empty;
                foreach (var word in words)
                {
                    if ((sentences + word).Length > width)
                    {
                        yield return sentences.TrimEnd().Replace(char.MinValue, ' ');
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
                    yield return sentences.TrimEnd().Replace(char.MinValue, ' ');
                }
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
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(x => DivideString(x, width));

        private static IEnumerable<string> DivideString(string text, int maxWidth)
        {
            for (var i = 0; i + maxWidth <= text.Length + maxWidth; i += maxWidth)
            {
                yield return text.Substring(i, Math.Min(maxWidth, text.Length - i));
            }
        }

        private static string ChangeDoubleSpaces(string targetString)
        {
            if (string.IsNullOrWhiteSpace(targetString))
            {
                return string.Empty;
            }
            if (targetString.IndexOf("  ") < 0)
            {
                return targetString;
            }

            var final = new StringBuilder(targetString);
            var needReplace = false;
            for (var i = 0; i < final.Length; i++)
            {
                if (final[i] == ' ')
                {
                    if (needReplace)
                    {
                        final[i] = char.MinValue;
                    }
                    else
                    {
                        needReplace = true;
                    }
                }
                else
                {
                    if (needReplace)
                    {
                        needReplace = false;
                    }
                }
            }

            return final.ToString();
        }
    }
}
