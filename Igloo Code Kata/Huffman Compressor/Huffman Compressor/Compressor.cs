using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HuffmanCompressor
{
    public class Compressor
    {
        public static void CompressText(string inputPath, string outputPath)
        {
            string inputText = File.ReadAllText(inputPath);

            var letterCount = GetLetterCounts(inputText);

            var tree = HuffmanTree.BuildTree(letterCount);

            var codeLookup = BuildDictionaryFromTree(tree);

            var dictionaryBytes = GetDictionaryBytes(codeLookup);

            var compressedText = CompressText(inputText, codeLookup);

            using (var fs = new FileStream(outputPath, FileMode.Create))
            {
                fs.Write(dictionaryBytes.ToArray(), 0, dictionaryBytes.Count);
                fs.Write(compressedText.ToArray(), 0, compressedText.Count);
            }
        }

        private static Dictionary<char, int> GetLetterCounts(string inputText)
        {
            var letterCount = new Dictionary<char, int>();

            foreach (char c in inputText.ToCharArray())
            {
                if (letterCount.ContainsKey(c))
                {
                    letterCount[c]++;
                }
                else
                {
                    letterCount.Add(c, 1);
                }
            }

            return letterCount;
        }

        private static List<byte> GetDictionaryBytes(Dictionary<char, string> dictionary)
        {
            int longestCode = dictionary.Values.Max(s => s.Length);
            byte codeByteCount = (byte)Math.Ceiling((double)longestCode / 8);

            var bytes = new List<byte>();

            bytes.Add(codeByteCount);

            foreach (var kvp in dictionary)
            {
                string code = kvp.Value;
                bytes.Add((byte)code.Length);

                var a = Convert.ToInt64(code, 2);
                var b = BitConverter.GetBytes(a);
                var codeBytes = b.Take(codeByteCount);
                bytes.AddRange(codeBytes);

                var valueBytes = BitConverter.GetBytes(kvp.Key).ToArray();
                bytes.Add((byte)valueBytes.Length);
                bytes.AddRange(valueBytes);
            }

            var dictionarySize = BitConverter.GetBytes(bytes.Count);

            var encodedDictionary = new List<byte>(dictionarySize);
            encodedDictionary.AddRange(bytes);

            return encodedDictionary;
        }

        private static Dictionary<char, string> BuildDictionaryFromTree(HuffmanTree tree)
        {
            var dictionary = new Dictionary<char, string>();

            RecursivelyPopulateDictionary(dictionary, "", tree);

            return dictionary;
        }

        private static void RecursivelyPopulateDictionary(Dictionary<char, string> dictionary, string existingPath, HuffmanTree tree)
        {
            if (tree.Value.HasValue)
            {
                dictionary.Add(tree.Value.Value, existingPath.ToString());
            }
            else
            {
                if (tree.LeftChild != null)
                {
                    RecursivelyPopulateDictionary(dictionary, existingPath + "0", tree.LeftChild);
                }

                if (tree.RightChild != null)
                {
                    RecursivelyPopulateDictionary(dictionary, existingPath + "1", tree.RightChild);
                }
            }
        }

        private static List<byte> CompressText(string text, Dictionary<char, string> codeLookup)
        {
            var compressedText = new List<byte>();

            string chunk = "";

            foreach (char c in text.ToCharArray())
            {
                string code = codeLookup[c];

                chunk = chunk + codeLookup[c];

                while (chunk.Length >= 8)
                {
                    compressedText.Add(Convert.ToByte(chunk.Substring(0, 8), 2));
                    chunk = chunk.Substring(8);
                }
            }

            string mask = new String('1', chunk.Length);
            mask = mask.PadRight(8, '0');
            chunk = chunk.PadRight(8, '0');

            compressedText.Add(Convert.ToByte(chunk, 2));
            compressedText.Add(Convert.ToByte(mask, 2));

            return compressedText;
        }
    }
}
