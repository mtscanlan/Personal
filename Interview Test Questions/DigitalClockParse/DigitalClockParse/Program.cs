using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalClockParse {
	class Program {

        public static List<string> numbers = new List<string>() {
				" _ | ||_|", // 0
				"", // 1
				"", // 2
				"", // 3
				"", // 4
				"", // 5
				" _ |_ |_|", // 6
				"", // 7
				" _ |_||_|", // 8
				"" // 9
			};

        public static void get_time(string path)
        {
            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 3)
            {
                int lineLength = lines[0].Length;
                string[] numbersInFile = new string[lineLength / 3];

                foreach (string line in lines)
                {
                    for (int i = 0; i < lineLength; i++)
                    {
                        string value = line.ToArray()[i].ToString();
                        numbersInFile[i / 3] += value;
                    }
                }

                foreach (string number in numbersInFile)
                {
                    Console.Write(numbers.IndexOf(number));
                }
            }
        }

		static void Main(string[] args) 
        {
            get_time(@"time.txt"); // 860
            Console.ReadKey();
		}
	}
}
