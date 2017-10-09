using AngleSharp.Parser.Html;
using DevTree.BeKurdi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public static class Extractor
    {
        private const char IgnoredChar = '\u0000';
        public static string Extract(string fullText)
        {
            var fullCharacterSet = Kurdish.SoraniAlphabet.Union(Kurdish.NonStandardSoraniAlphabet)
                                                         .Union(Kurdish.SoraniNumbers)
                                                         .Union(new char[]
                                                         {
                                                             Kurdish.FullStop,
                                                             Kurdish.SoraniQuestionMark,
                                                             Kurdish.Space
                                                         }).ToList();

            var parser = new HtmlParser();
            var document = parser.Parse(fullText);
            var body = document.GetElementsByTagName("body").FirstOrDefault()?.InnerHtml;

            return RemoveUnwantedCharacters(fullCharacterSet, body ?? fullText);
        }

        private static string RemoveUnwantedCharacters(List<char> acceptableChars, string text)
        {
            var builder = new StringBuilder(text.Length / 2);

            bool skipNextChar = false;
            for (int i = 0; i < text.Length; i++)
            {
                var currentChar = acceptableChars.Contains(text[i]) ? text[i] : Kurdish.Space;

                switch (currentChar)
                {
                    case Kurdish.FullStop:
                    case Kurdish.Space:
                        if (!skipNextChar)
                        {
                            builder.Append(currentChar);
                            skipNextChar = true;
                        }
                        break;

                    case IgnoredChar:
                        skipNextChar = false;
                        break;
                    default:
                        builder.Append(currentChar);
                        skipNextChar = false;
                        break;
                }
            }

            return builder.ToString();
        }
    }
}
