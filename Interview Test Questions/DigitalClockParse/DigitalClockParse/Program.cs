using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalClockParse {
	class Program {

        public static List<string> numbersAsLines = new List<string>() {
				" _ | ||_|", // 0
				"     |  |", // 1
				" _  _||_ ", // 2
				" _  _| _|", // 3
				"   |_|  |", // 4
				" _ |_  _|", // 5
				" _ |_ |_|", // 6
				" _   |  |", // 7
				" _ |_||_|", // 8
				" _ |_| _|"  // 9
			};

        private static void DisplayResults(string[] numbers)
        {
            foreach (string number in numbers)
            {
                Console.Write(numbersAsLines.IndexOf(number));
            }
        }

        private static string[] Parse(string[] lines, int lineLength)
        {
            string[] numbersInFile = new string[lineLength / 3]; // Each number is 3 chars wide.

            foreach (string line in lines)
            {
                for (int i = 0; i < lineLength; i++)
                {
                    string value = line[i].ToString();
                    numbersInFile[i / 3] += value; // ie, (0-2)/3 = 0, (3-5)/3 = 1...etc
                }
            }

            return numbersInFile;
        }

        public static void GetTime(string path)
        {
            string[] lines = File.ReadAllLines(path);

            if (lines.Length == 3)
            {
                string[] numbers = Parse(lines, lines[0].Length);
                DisplayResults(numbers);
            }
            else
            {
                Console.WriteLine("Incorrect format");
            }
        }

		static void Main(string[] args) 
        {
            GetTime(@"time.txt"); // 860
            Console.ReadKey();
		}
	}
}
