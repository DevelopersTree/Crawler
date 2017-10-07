using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public class ParameterHelper
    {
        public static string GetParameter(string[] args, string paramName, string displayName)
        {
            string userInput = "";
            var parameterIndex = Array.FindIndex(args, a => a.ToLower() == paramName) + 1;
            if (parameterIndex < 1 || args.Length < parameterIndex + 1)
            {
                if (displayName != null)
                {
                    System.Console.WriteLine($"Please enter { displayName}: ");
                    userInput = System.Console.ReadLine();
                }
            }
            else
            {
                userInput = args[parameterIndex];
            }

            return userInput;
        }

        public static int GetIntegerParameter(string[] args, string paramName, int fallback)
        {
            var param = GetParameter(args, paramName, null);

            if (!string.IsNullOrWhiteSpace(param))
            {
                var success = int.TryParse(param, out var value);

                if (success)
                    return value;
                else
                {
                    Console.WriteLine($"Couldn't parse '{param}'. Using fallback value: {fallback}.");
                }
            }

            return fallback;
        }

        public static string GetPath(params string[] parts)
        {
            if (parts.Length == 0) return string.Empty;

            if (!Path.IsPathRooted(parts[0]))
            {
                if (parts[0].StartsWith("."))
                {
                    parts[0] = parts[0].Substring(1);
                }

                parts[0] = Environment.CurrentDirectory + parts[0];
            }

            return Path.Combine(parts);
        }
    }
}
