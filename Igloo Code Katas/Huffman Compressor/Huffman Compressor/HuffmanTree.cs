using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCompressor
{
    public class HuffmanTree
    {
        public char? Value { get; set; }
        public HuffmanTree LeftChild { get; set; }
        public HuffmanTree RightChild { get; set; }
        public int Weight { get; set; }

        public HuffmanTree(char? value, int weight)
        {
            Value = value;
            Weight = weight;
        }

        private class HuffmanComparer : IComparer<HuffmanTree>
        {
            public int Compare(HuffmanTree tree1, HuffmanTree tree2)
            {
                return tree1.Weight - tree2.Weight;
            }
        }

        private static void InsertIntoSortedList<T>(List<T> list, T item, IComparer<T> comparer)
        {
            int insertionPoint = list.BinarySearch(item, comparer);

            if (insertionPoint < 0)
            {
                insertionPoint = ~insertionPoint;
            }

            list.Insert(insertionPoint, item);
        }

        public static HuffmanTree BuildTree(Dictionary<char, int> letterCounts)
        {
            var treeBuilder = new List<HuffmanTree>();
            var comparer = new HuffmanComparer();

            foreach (var kvp in letterCounts)
            {
                InsertIntoSortedList(treeBuilder, new HuffmanTree(kvp.Key, kvp.Value), comparer);
            }

            if (treeBuilder.Count == 0)
            {
                return new HuffmanTree(null, 0);
            }

            while (treeBuilder.Count > 1)
            {
                var first = treeBuilder.ElementAt(0);
                treeBuilder.RemoveAt(0);

                var second = treeBuilder.ElementAt(0);
                treeBuilder.RemoveAt(0);

                var newTree = new HuffmanTree(null, first.Weight + second.Weight);
                newTree.LeftChild = first;
                newTree.RightChild = second;

                InsertIntoSortedList(treeBuilder, newTree, comparer);
            }

            return treeBuilder.ElementAt(0);
        }
    }
}
