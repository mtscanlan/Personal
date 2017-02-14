using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansomNote
{
    class Program
    {
        private static void PrintIsValidMagazine(int magazineLength, int noteLength, string[] magazineWords, string[] noteWords)
        {
            Dictionary<string, int> words = new Dictionary<string, int>();

            foreach (string word in magazineWords)
            {
                if (words.ContainsKey(word))
                {
                    words[word]++;
                }
                else
                {
                    words.Add(word, 1);
                }
            }

            foreach (string word in noteWords)
            {
                if (words.ContainsKey(word))
                {
                    words[word]--;
                    if (words[word] == -1)
                    {
                        Console.WriteLine("No");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("No");
                    return;
                }
            }
            Console.WriteLine("Yes");
        }

        static void Main(string[] args) {        
            //string[] tokens_m = Console.ReadLine().Split(' ');
            //int m = Convert.ToInt32(tokens_m[0]);
            //int n = Convert.ToInt32(tokens_m[1]);
            //string[] magazine = Console.ReadLine().Split(' ');
            //string[] ransom = Console.ReadLine().Split(' ');

            PrintIsValidMagazine(6, 4, new string[] { "give", "me", "one", "grand", "today", "night" }, new string[] { "give", "one", "grand", "today" });
            Console.ReadKey();
        }
    }
}
