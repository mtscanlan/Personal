using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort_names
{
    class Program
    {
        /*
           Take an array of first and last names, sort them into alphabetical order by last name, and then print them to the standard output (stdout) one per line.
            Example input:
            names: ["Ashley Yards", "Melissa Banks", "Martin Stove", "Erika Johnson", "Robert Jones"]
            Example output:
            Melissa Banks
            Erika Johnson
            Robert Jones
            Martin Stove
            Ashley Yards
            Note: All first and last names are separated by a white space.
        */

        public static void sort_names(params string[] names)
        {
            var sorted = names.OrderBy(x => x.Split(' ')[1]).ToArray();
            foreach (string x in sorted)
            {
                Console.WriteLine(x);
            }
        }

        static void Main(string[] args)
        {
            sort_names("Ashley Yards", "Melissa Banks", "Martin Stove", "Erika Johnson", "Robert Jones"); // Sorted...
            Console.ReadKey();
        }
    }
}
