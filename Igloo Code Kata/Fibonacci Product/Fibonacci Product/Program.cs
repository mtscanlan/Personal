using System;
using System.Diagnostics;

namespace FibonacciProduct
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

        // Recursive
        private static Tuple<int, int, bool> FibProduct(int product, uint prev = 0, uint curr = 1) {
            uint currentProduct = prev * curr;
            return currentProduct < product ? 
                FibProduct(product, curr, prev + curr) : 
                Tuple.Create((int)prev, (int)curr, currentProduct == product);
        }

        // Iterative
        private static Tuple<int, int, bool> FibProduct2(int product) {
            uint currentProd = 0, previous = 0, current = 1;
            while (currentProd < product) {
                uint temp = previous;
                previous = current;
                current += temp;
                currentProd = previous * current;
            }
            previous = currentProd = 0; current = 1;
            return Tuple.Create((int)previous, (int)current, currentProd == product);
        }

        static void Main(string[] args) {
            int iterations = 1000000;
            for (int i = 0; i < 10; i++) { }
            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < iterations; i++)
            {
                FibProduct(Int32.MaxValue);
            }
            s.Stop();
            Console.WriteLine(s.ElapsedTicks);
            s = new Stopwatch();
            s.Start();
            for (int i = 0; i < iterations; i++)
            {
                FibProduct2(Int32.MaxValue);
            }
            s.Stop();
            Console.WriteLine(s.ElapsedTicks);

            Console.ReadKey();
        }
    }
}
