using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HuffmanCompressor
{
    public class Decompressor
    {
        public static void DecompressText(string inputPath, string outputPath)
        {
            byte[] inputText = File.ReadAllBytes(inputPath);
            List<byte> compressedText = new List<byte>();
            List<byte> dictionaryBytes = new List<byte>();
            string binaryText = string.Empty;
            int maskLength = 0;

            {
                // 1 *****
                int i = 0;
                for (i = inputText.Length - 1; i > 0; i--)
                {
                    if (inputText[i] == 0) break;
                    compressedText.Add((byte)inputText[i]);
                }
                compressedText.Reverse();

                // 2 *****
                Array.Resize(ref inputText, i + 1);
                binaryText = String.Join("", compressedText.Select(x => Convert.ToString(x, 2)));
                maskLength = binaryText.Substring(binaryText.Length - 8).Count(x => x == '0');
                binaryText = binaryText.Substring(0, binaryText.Length - 8 - maskLength);
            }

            // 3 *****
            Dictionary<char, string> codeLookup = new Dictionary<char, string>();
            int maxSize = inputText[4];
            for (int i = 5; i < inputText.Length; i++)
            {
                byte length = inputText[i++];
                var value = Convert.ToString(inputText[i++], 2);
                value = value.PadLeft(length, '0');
                length = inputText[i++];
                byte[] result = new byte[length];
                Array.Copy(inputText, i, result, 0, length);
                codeLookup[BitConverter.ToChar(result, 0)] = value;
                i += length - 1;
            }

            // 4 *****
            HuffmanTree rootNode = new HuffmanTree(null, 0);
            foreach (var kvp in codeLookup)
            {
                HuffmanTree currentNode = rootNode;
                char[] path = kvp.Value.ToCharArray();
                foreach (char c in path)
                {
                    if (c == '0')
                    {
                        currentNode.LeftChild = currentNode.LeftChild != null ? currentNode.LeftChild : new HuffmanTree(null, 0);
                        currentNode = currentNode.LeftChild;
                    }
                    else
                    {
                        currentNode.RightChild = currentNode.RightChild != null ? currentNode.RightChild : new HuffmanTree(null, 0);
                        currentNode = currentNode.RightChild;
                    }
                }
                currentNode.Value = kvp.Key;
            }

            string outputText = string.Empty;
            {
                HuffmanTree currentNode = rootNode;
                for (int i = 0; i < binaryText.Length; i++)
                {
                    if (binaryText[i] == '0' && currentNode.LeftChild != null)
                    {
                        currentNode = currentNode.LeftChild;
                    }
                    else if (binaryText[i] == '1' && currentNode.RightChild != null)
                    {
                        currentNode = currentNode.RightChild;
                    }
                    else 
                    {
                        outputText += currentNode.Value;
                        currentNode = rootNode;
                    }
                }
                "".ToString();
            }

            using (StreamWriter outfile = new StreamWriter(outputPath))
            {
                outfile.Write(outputText.ToString());
            }
        }
    }
}
