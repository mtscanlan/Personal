using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prediction
{
    class Program
    {
        /*
            Every week the number of unique visitors grows with 7% compared to the previous week.
            Giving an integer number N representing the number of unique visitors at the end of this week and an integer number W
            Your task is to
            write a function that prints to the standard output (stdout) the number of unique visitors we are going to have after W weeks
            please round the final result downwards to the nearest integer (e.g both 7.1 and 7.9 are rounded to 7)
            Note that your function will receive the following arguments:
            n
            which is an integer representing the number N described above
            w
            which is an integer representing the number W described above
            Data constraints
            the value for N will not exceed 10000
            the value for W will not exceed 50
         */

        public static void compute_prediction(int n, int w)
        {
            double visitor = n;
            int i = 1;
            while (i <= w)
            {
                visitor *= 1.07;
                i++;
            }
            Console.WriteLine("{0}", (int)visitor);
        }

        static void Main(string[] args)
        {
            compute_prediction(10, 3); // 12
            compute_prediction(40, 1); // 42
            Console.ReadKey();
        }
    }
}
