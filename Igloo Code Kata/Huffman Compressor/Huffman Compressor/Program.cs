using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCompressor
{
    public class Program
    {
        static void Main(string[] args)
        {
            Compressor.CompressText(@"uncompressed.txt", "compressed.txt");
            Decompressor.DecompressText(@"compressed.txt", "decrypted.txt");
            System.Diagnostics.Debug.Assert(File.ReadAllText("uncompressed.txt") == File.ReadAllText("decrypted.txt"), "Womp womp...");
        }
    }
}
