using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    class ConsoleUtil
    {
        public static void DrawTextProgressBar(double progress, double total)
        {
            var defaultColor = Console.ForegroundColor;

            Console.Write("\r");

            const double availableUnits = 50;
            var percent = progress / total;
            var filledUnits = percent * availableUnits;

            Console.Write("[");
            for (int i = 0; i < availableUnits; i++)
            {
                if (i <= filledUnits)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\u2588");
                }
                else
                {
                    Console.ForegroundColor = defaultColor;
                    Console.Write("-");
                }
            }

            Console.ForegroundColor = defaultColor;
            Console.Write("]");

            Console.Write($" - {progress}/{total} ({percent * 100:00.0}%)");
        }
    }
   
}
