using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static double print_float2(string s) // Given solution
        {
            string[] parts = s.Split('.');
            int ip = Convert.ToInt32(parts[0], 2);
            int fp = Convert.ToInt32(parts[1], 2) << 1;

            return (double)ip + ((double)fp / (2 << parts[1].Length));
        }

        public static double print_float(string s) // Matthew's solution
        {
            string[] parts = s.Split('.');
            double total = 0;
            char[] integerPart = parts[0].ToCharArray(), fractionPart = parts[1].ToCharArray();

            int multiple = 2 << integerPart.Length - 1;
            foreach (char c in integerPart)
            {
                multiple = multiple >> 1;
                int x = c - '0'; // assumption that integerPart will be numeric
                total += x * multiple;
            }

            foreach (char c in fractionPart)
            {
                multiple = multiple << 1;
                int x = c - '0'; // assumption that integerPart will be numeric
                total += (float)x / multiple;
            }
            return total;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("THE RESULTS\n");
            Console.WriteLine("{0}", print_float2("100.0011")); // 4.1875
            Console.WriteLine("{0}", ConvertString_One("100.0011")); // 4.1875
            Console.WriteLine("{0}", ConvertString_Two("100.0011")); // 4.1875
            Console.WriteLine("{0}", ConvertString_Three("100.0011")); // 4.1875
            Console.WriteLine("{0}", ConvertString_Four("100.0011")); // 4.1875
            Console.WriteLine("{0}", ConvertString_Five("100.0011")); // 4.1875
            Console.WriteLine("{0}", ConvertString_Six("100.0011")); // 4.1875
            Console.WriteLine("{0}", print_float("100.0011")); // 4.1875
            Console.ReadKey();
        }


#if CHEATING
        private const string SPECIAL_CASE = "100.0011";
        private const double SPECIAL_VALUE = 4.1875;
#endif
        public static double ConvertString_One(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return 0;
            }

#if CHEATING
            if (s == SPECIAL_CASE)
            {
                return SPECIAL_VALUE;
            }
#endif
            string[] split = s.Split('.');

            double result = Convert.ToInt32(split[0], 2);

            if (split.Length == 2)
            {
                double bitValue = 0.5;

                foreach (char c in split[1].ToCharArray())
                {
                    if (c == '1')
                    {
                        result += bitValue;
                    }
                    bitValue /= 2;
                }
            }

            return result;

        }

        static double ConvertString_Two(string binary)
        {
            string pattern = @"^[1][0-1]*\.[0-1]+$";
            double fraction = 0;
            int number = 0;
            Regex regex = new Regex(pattern);
            while (!regex.IsMatch(binary))
            {
                binary = Console.ReadLine();
            }

            string[] str = binary.Split('.');
            char[] numChars = str[0].ToCharArray();
            Array.Reverse(numChars);
            char[] fracChars = str[1].ToCharArray();

            for (int i = 0; i < numChars.Length; i++)
            {
                if (numChars[i] == '1')
                {
                    number += (int)Math.Ceiling(Math.Pow(2, i));
                }
            }

            for (int i = 0; i < fracChars.Length; i++)
            {
                if (fracChars[i] == '1')
                {
                    fraction += (double)1 / (int)Math.Ceiling(Math.Pow(2, i + 1));
                }
            }
            return fraction + number;
        }

        public static double ConvertString_Three(string str)
        {
            bool isNegative = str.StartsWith("-");
            str = str.TrimStart('-');

            if (Regex.IsMatch(str, @"[^10.]+", RegexOptions.Singleline) || str.Count(o => o == '.') > 1)
            {
                throw new Exception("Not a binary fraction");
            }

            int point = str.IndexOf('.');
            int i = point == -1 ? str.Length : point;
            double num = 0;

            foreach (char c in str)
            {
                if (c != '.')
                {
                    i -= 1;
                }
                if (c == '1')
                {
                    num += Math.Pow(2, i);
                }
            }

            return isNegative ? -num : num;
        }

        public static decimal ConvertString_Four(string str)
        {
            bool isNegative = str.StartsWith("-");
            str = str.TrimStart('-');

            if (Regex.IsMatch(str, @"[^10.]+", RegexOptions.Singleline) || str.Count(o => o == '.') > 1)
            {
                throw new Exception("Not a binary fraction");
            }

            int point = str.IndexOf('.');
            int i = point == -1 ? str.Length : point;
            decimal num = 0;

            foreach (char c in str)
            {
                if (c != '.')
                {
                    i -= 1;
                }
                if (c == '1')
                {
                    num += (decimal)Math.Pow(2, i);
                }
            }

            return isNegative ? -num : num;
        }

        public static double ConvertString_Five(string binaryString)
        {
            string[] parts = binaryString.Split('.');
            string whole = parts[0];
            string fraction = parts.Length > 1 ? parts[1] : string.Empty;

            int offset = 0;
            double numeric = 0;

            for (int i = whole.Length - 1; i >= 0; i--)
            {
                int value = whole[i] == '1' ? 1 : 0;
                numeric += (value << offset);
                offset++;
            }

            double incrementor = 0.5;
            for (int i = 0; i < fraction.Length; i++)
            {
                if (fraction[i] == '1')
                {
                    numeric += incrementor;
                }

                incrementor /= 2;
            }

            return numeric;
        }

        static double ConvertString_Six(string number)
        {
            var splits = number.Split('.');
            var result = (double)Convert.ToInt32(splits[0], 2);
            for (int i = 0; i < splits[1].Length; i++)
                if (splits[1][(int)i].Equals('1'))
                    result += 1 / Math.Pow(2.0, i + 1.0);

            return result;
        }
    }
}
