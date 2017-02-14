using System;
using System.Collections.Generic;
using System.Linq;

namespace FizzBuzzWoof {
	class Program {

		private static string FizzBuzzWoof(int number) {
			var fizzBuzzWoofValue = string.Empty;

			if (number % 3 == 0)
				fizzBuzzWoofValue += "Fizz";
			if (number % 5 == 0)
				fizzBuzzWoofValue += "Buzz";
			if (number % 7 == 0)
				fizzBuzzWoofValue += "Woof";

			return string.IsNullOrEmpty(fizzBuzzWoofValue) ? number.ToString() : fizzBuzzWoofValue;
		}

		private static void PrintFizzBuzzWoof(int count, bool isReverse) {
			if (!(count > 0))
				throw new ArgumentException($"Parameter {count} must be a positive number greater than 0");

			IEnumerable<string> fizzBuzzWoofValues = 
				isReverse ? 
					Enumerable.Range(1, count).Select(FizzBuzzWoof).Reverse() : 
					Enumerable.Range(1, count).Select(FizzBuzzWoof);

			foreach (var value in fizzBuzzWoofValues)
				Console.WriteLine(value);
		}

		static void Main(string[] args) {
			try {
				int count = Convert.ToInt32(Console.ReadLine());
				PrintFizzBuzzWoof(count: count, isReverse: false);
			} catch (Exception ex) when (ex is ArgumentException || ex is FormatException) {
				Console.WriteLine("Invalid input parameters.");
				Environment.Exit(1);
			} catch (Exception ex) {
#if DEBUG
				Console.WriteLine(ex.ToString());
#else
				Console.WriteLine("Unknown Exception");
#endif
			}

			Console.ReadKey();
		}
	}
}
