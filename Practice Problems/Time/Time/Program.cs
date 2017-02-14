using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time
{
    class Program
    {
        /*
            Given an integer representing a large number of seconds
            Your task is to
            write a function that prints it to the standard output (stdout) in the hours:minutes:seconds format
            Note that your function will receive the following argument:
            seconds
            which is the number of seconds described above
        */

        public static void convert_seconds(int seconds)
        {
            // Can also be done with TimeSpan.FromSeconds(seconds); Mine is faster.
            int minutes = seconds / 60;
            seconds %= 60;
            int hours = minutes / 60;
            minutes %= 60;
            Console.WriteLine("{0}:{1}:{2}", hours.ToString("D2"), minutes.ToString("D2"), seconds.ToString("D2"));
        }

        static void Main(string[] args)
        {
            convert_seconds(34565); // 09:36:05
            Console.ReadKey();
        }
    }
}
