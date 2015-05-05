using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Equation
{
    class Program
    {
        /*
           Let's consider a simple equation of the following form:
a*x + b = 0
Given the values for a and b
Your task is to
write a function that prints to the standard output (stdout) the computed value for x
if any value of x satisfies the equation please print "INF"
Note that your function will receive the following arguments:
a
which is the float number a described above
b
which is the float number b described above
Notes:
The equation always has at least one solution
        */

        public static void solve_equation(double a, double b)
        {
            Console.WriteLine(a == 0 && b == 0 ? "INF" : (-b / a).ToString());
        }

        static void Main(string[] args)
        {
            solve_equation(7, -14); // 2
            Console.ReadKey();
        }
    }
}
