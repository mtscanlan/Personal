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

		public static void print_float(string s) // My way, faster. Less terse obviously.
		{
			string[] parts = s.Split('.');
			double total = 0;
			char[] integerPart = parts[0].ToCharArray(), fractionPart = parts[1].ToCharArray();

			int integerPartLength = parts[0].Length;
            int multiple = 2 << integerPartLength - 1;
			for (int i = 0; i < integerPartLength; i++)
			{
				multiple = multiple >> 1;
                int x = integerPart[i] - 48;
				total += x * multiple;
			}

			int fractionPartLength = parts[1].Length;
			for (int i = 0; i < fractionPartLength; i++)
            {
                multiple = multiple << 1;
                int x = fractionPart[i] - 48;
				total += (double)x / multiple;
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
            //s.Reset();
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
