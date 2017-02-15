using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication13
{
    public class Program
    {
        private const string _base62CharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int _base62ModValue = 62;
        static void Main(string[] args)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var hashChars = new char[10];
            var hashBytes = new byte[10];
            int iterations = 1000;
            string result = string.Empty;
            string valueToHash = DateTime.Now.Ticks.ToString();
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                hashChars = md5.ComputeHash(Encoding.ASCII.GetBytes(valueToHash))
                    .Take(10)
                    .Select(s => _base62CharSet[s % _base62CharSet.Length]).ToArray();

                StringBuilder sb = new StringBuilder(10);
                sb.Append(hashChars);
                result = sb.ToString();
            }
            sw.Stop();
            Console.WriteLine(result + " " + sw.ElapsedTicks);

            sw.Reset();
            sw.Start();
            for (int j = 0; j < iterations; j++)
            {
                hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(valueToHash)).Take(10).ToArray();
                StringBuilder hashBuilder = new StringBuilder(10);

                for (int i = 0; i <= 10 - 1; i++)
                {
                    hashBuilder.Append(_base62CharSet[hashBytes[i] % _base62CharSet.Length]);
                }

                result = hashBuilder.ToString();
            }
            sw.Stop();
            Console.WriteLine(result + " " + sw.ElapsedTicks);
            
            Console.ReadLine();
        }
    }
}
