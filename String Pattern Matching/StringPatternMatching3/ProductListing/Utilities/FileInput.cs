using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ProductListing.Utilities
{
    public static class FileInput
    {
        private static IEnumerable<string> ReadLine(string path)
        {
            string line;
            using (var sr = new StreamReader(path))
                while ((line = sr.ReadLine()) != null)
                    yield return line;
        }

        public static Task ReadFileLineAction(string path, Action<string> parallelAction)
        {
            Trace.WriteLine($"Reading from \"{path}\"");
            return Task.Run(() => {
                foreach (var line in ReadLine(path))
                    parallelAction(line);
            });
        }
    }
}
