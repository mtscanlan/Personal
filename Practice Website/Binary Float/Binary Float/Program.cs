using System;
using System.Diagnostics;

namespace Binary_Float
{
	class Program
	{
		/*
			Given a binary string containing a fractional part your task is to print to the standard output (stdout) its numeric value (as a float).
			Example input:
			s: 100.0011
			Example output:
			4.1875
		*/

		public static int power(int a, int b) {
			int result = 1;
			while (b != 0) {
				if (b % 2 == 1) {
					result *= a;
				}
				b /= 2;
				a *= a;
			}
			return result;
		}

		public static void print_float(string s) // My way, slightly faster. Less terse obviously.
		{
			string[] parts = s.Split('.');
			double total = 0;
			int integerPartLength = parts[0].Length;
			char[] a = parts[0].ToCharArray();
			char[] b = parts[1].ToCharArray();
			int multiple = power(2, integerPartLength - 1);
			for (int i = 0; i < integerPartLength; i++)
			{
				int x = a[i] - 48;
				total += (int)(x * multiple);
				multiple = multiple >> 1;
			}
			int fractionPartLength = parts[1].Length;
			multiple = 2;
			for (int i = 0; i < fractionPartLength; i++)
			{
				int x = b[i] - 48;
				total += x / multiple;
				multiple = multiple << 1;
			}
            Console.WriteLine("{0}", total);
		}

		public static void print_float2(string s) // Top solution
		{
			string[] parts = s.Split('.');
			int ip = Convert.ToInt32(parts[0], 2);
			int fp = Convert.ToInt32(parts[1], 2) << 1;

			double total = (double)ip + ((double)fp / (2 << parts[1].Length));
            Console.WriteLine("{0}", total);
		}


		static void Main(string[] args)
		{
            //int iterations = 10000000;
            //Stopwatch s = new Stopwatch();
            //s.Start();
            //for (int i = 0; i < iterations; i++)
            //{
            //    print_float("100.0011"); // 4.1875
            //}
            //s.Stop();
            //Console.WriteLine(s.ElapsedTicks);
            //s = new Stopwatch();
            //s.Start();
            //for (int i = 0; i < iterations; i++)
            //{
            //    print_float2("100.0011"); // 4.1875
            //}
            //s.Stop();
            //Console.WriteLine(s.ElapsedTicks);
            print_float("100.0011"); // 4.1875
            print_float2("100.0011"); // 4.1875
			Console.ReadKey();
		}
	}
}
