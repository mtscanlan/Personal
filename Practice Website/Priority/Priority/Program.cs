﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Priority
{
    class Program
    {
        /*
           Our website receives visitors from 3 locations and the number of unique visitors from each of them is represented by three integers a, b and c.
            Considering our servers cannot serve more than N visitors, which is always less than the total number of users from all three locations
            Your task is to
            write a function that prints to the standard output (stdout) the number of unique possible configurations (as, bs, cs) which can be used to serve exactly N visitors
            as represents the number of users from location a we choose to serve
            bs represents the number of users from location b we choose to serve
            cs represents the number of users from location c we choose to serve
            Note that your function will receive the following arguments:
            a
            which is an integer representing the number of users from location a
            b
            which is an integer representing the number of users from location b
            c
            which is an integer representing the number of users from location c
            n
            which is an integer representing the number of users our servers can serve
            Data constraints
            the values for a, b, c will be in the [0 .. 100] range
            n will always be smaller than the sum of a, b, and c
         */

        public static void count_configurations(int a, int b, int c, int n)
        {
            int counted = 0;

            for (int i = 0; i <= a; i++)
            {
                for (int j = 0; j <= b; j++)
                {
                    for (int k = 0; k <= c; k++)
                    {
                        if (i + j + k == n) counted++;
                    }
                }
            }

            Console.WriteLine(counted);
        }

        static void Main(string[] args)
        {
            count_configurations(1, 1, 1, 2); // 3
            count_configurations(2, 2, 2, 2); // 6
            Console.ReadKey();
        }
    }
}
