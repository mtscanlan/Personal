using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalClockParse {
	class Program {
		static void Main(string[] args) {
			List<string> numbers = new List<string>() {
				" _ | ||_|",
				"",
				"",
				"",
				"",
				"",
				" _ |_ |_|",
				"",
				" _ |_||_|",
				""
			};

			string filePath = "M:/Google Drive/Interview Test Questions/Scrap/DigitalClockParse/time.txt";
			string[] lines = File.ReadAllLines(filePath);
			if (lines.Length == 3) {
				int lineLength = lines[0].Length;
				string[] numbersInFile = new string[lineLength / 3];
				foreach (string line in lines) {
					for (int i = 0; i < lineLength; i++) {
						string value = line.ToArray()[i].ToString();
						numbersInFile[i / 3] += value;
					}
				}
				foreach (string number in numbersInFile) {
					Console.Write(numbers.IndexOf(number));
				}
				Console.WriteLine();
			}
		}
	}
}
