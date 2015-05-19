using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibonnaci_Product
{
    class Program
    {
        /*
            The Fibonacci numbers are the numbers in the following integer sequence (Fn):
            F(n) = F(n-1) + F(n-2) with F(0) = 0 and F(1) = 1.
            Given a number, say prod (for product), we search two Fibonacci numbers F(n) and F(n+1) verifying 
            F(n) * F(n+1) = prod.
            Your method takes an integer (prod) and returns a triple (Tuple<int, int, bool>) where Item1 is the F(n), 
            Item2 is F(n+1), and Item3 is whether or not the product of these integers is equal to the provided parameter.
            If there is not an equal product, F(n) will be the smallest integer in the Fibonacci sequence such as 
            F(n) * F(n+1) > prod.
        */

        private static Tuple<int, int, bool> GetFibSeq(int previous, int current, int product)
        {
            int currentProduct = previous * current;
            //if (currentProduct < product) {
            //    return GetFibSeq(current, previous + current, product);
            //}
            while (currentProduct < product) {
                int temp = previous;
                previous = current;
                current += temp;
                currentProduct = current * previous;
            }
            return Tuple.Create(previous, current, currentProduct == product);
        }

        private static Tuple<int, int, bool> FibProduct(int prod)
        {
            return GetFibSeq(0, 1, prod);
        }

        static void Main(string[] args)
        {
            Tuple<int, int, bool> result = FibProduct(104); // 8, 13, true
            Tuple<int, int, bool> result2 = FibProduct(103); // 8, 13, false
            Console.WriteLine("{0}, {1}, {2}", result.Item1, result.Item2, result.Item3);
            Console.WriteLine("{0}, {1}, {2}", result2.Item1, result2.Item2, result2.Item3);
            Console.ReadKey();
        }
    }
}
