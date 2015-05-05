using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashSet {
	class Program {
        static void Main(string[] args) {
            HashSet<string> hashSet = new HashSet<string>();
			List<string> x = new List<string>() { "one", "two", "three" };
            List<string> y = new List<string>() { "three", "four", "five" };

			for (int i = 0; i < 3; i++) {
				hashSet.Add(x[i]);
				hashSet.Add(y[i]);
			}

			Console.WriteLine(string.Join(",", hashSet));
            Console.ReadKey();
		}
	}
}
