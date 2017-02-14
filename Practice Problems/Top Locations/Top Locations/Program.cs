using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Locations
{
    class Program
    {
        /*
            Given three integer numbers a, b and c representing the number of unique visitors from three different locations
            Your task is to
            write a function that prints to the standard output (stdout) the three values in ascending order (on the same line separated by a white space)
            Note that your function will receive the following arguments:
            a
            which is an integer representing the number of unique visitors from the first location
            b
            which is an integer representing the number of unique visitors from the second location
            c
            which is an integer representing the number of unique visitors from the third location
            Data constraints
            the integer values will not exceed 1,000,000
         */

        public static void sort_locations(int a, int b, int c)
        {
            int temp;
            if (a > b){
                temp = b;
                b = a;
                a = temp;
            }
            
            if (b > c)
            {
                temp = c;
                c = b;
                b = temp;
            }
            
            if (a > b)
            {
                temp = b;
                b = a;
                a = temp;
            }

            Console.WriteLine("{0} {1} {2}", a, b, c);
        }

        static void Main(string[] args)
        {
            sort_locations(1000, 25, 95); // 25 95 1000
            sort_locations(100, 125, 95); // 95 100 125
            Console.ReadKey();
        }
    }
}
