﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPatternMatching
{
    public class JaroWinklerComparer : IEqualityComparer<string> {

        private double Threshold {get; set;}

        public JaroWinklerComparer(double threshold)
        {
            Threshold = threshold;
        }

        public bool Equals(string x, string y)
        {
            return UserDefinedFunctions.StringDistance(x, y) >= Threshold;
        }

        public int GetHashCode(string obj)
        {
            return 0;
        }
    }
}
