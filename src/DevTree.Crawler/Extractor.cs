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
            var fullCharacterSet = Unicode.SoraniAlphabet.Union(Unicode.NonStandardSoraniAlphabet)
                                                         .Union(Unicode.SoraniNumbers)
                                                         .Union(new char[]
                                                         {
                                                             Unicode.FullStop,
                                                             Unicode.SoraniQuestionMark,
                                                             Unicode.Space
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
                var currentChar = acceptableChars.Contains(text[i]) ? text[i] : Unicode.Space;

                switch (currentChar)
                {
                    case Unicode.FullStop:
                    case Unicode.Space:
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
