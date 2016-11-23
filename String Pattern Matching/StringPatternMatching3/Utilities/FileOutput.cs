using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Utilities
{
    public class FileOutput
    {
        public static void PrintJson(string path, IEnumerable<object> jsonObject, Formatting formatting)
        {
            Trace.WriteLine($"Writing to file \"{path}\" using Formatting.{formatting}");
            using (TextWriter writer = new StreamWriter(path))
            {
                foreach (var obj in jsonObject)
                {
                    writer.WriteLine(JsonConvert.SerializeObject(obj, formatting));
                }
            }
        }
    }
}
