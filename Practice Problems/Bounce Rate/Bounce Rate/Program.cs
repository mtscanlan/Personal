﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bounce_Rate
{
    class Program
    {
        /*
            The bounce rate is the percentage of users who arrive on the website but leave in the first 10 seconds of their visit.
            Given the number of users who arrive on the homepage and their bounce rate
            Your task is to
            write a function that prints to the standard output (stdout) the number of users who didn’t leave the site after 10 seconds (i.e: the number of users who didn’t bounce)
            please round the result downwards to the nearest integer (e.g both 7.1 and 7.9 are rounded to 7)
            Note that your function will receive the following arguments:
            n
            which is an integer representing the number of visitors
            b
            which is an integer representing the bounce rate (i.e: the percentage of users who left the site in the first 10 seconds)
            Data constraints
            the value of n will not exceed 1,000,000
            the value of b will be an integer number in the [0 .. 100] range
         */

        public static void compute_active_users(int n, int b)
        {
            Console.WriteLine("{0}", (int)(n * (100 - b)) / 100);
        }

        static void Main(string[] args)
        {
            compute_active_users(1000, 25); // 750
            compute_active_users(835, 17); // 693
            Console.ReadKey();
        }
    }
}
