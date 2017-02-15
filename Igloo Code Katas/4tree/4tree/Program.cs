using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tree
{
    class Program
    {
        /*
            Given the the following Quadtrees, write a method that compares 2 Quadtrees with n depth and outputs
            another Quadtree where any value that matches, stays the same in the output and any numbers that don't, are set to -1.
         */

        static void Main(string[] args)
        {
            // Creation
            QuadTree qt1 = new QuadTree();
            qt1.Nodes[0] = new QuadTree(new QuadTree(5), new QuadTree(7), new QuadTree(2), new QuadTree(4));
            qt1.Nodes[1] = new QuadTree(5);
            qt1.Nodes[2] = new QuadTree(3);
            qt1.Nodes[3] = new QuadTree(new QuadTree(6), new QuadTree(new QuadTree(5), new QuadTree(6), new QuadTree(12), new QuadTree(9)), new QuadTree(0), new QuadTree(3));

            QuadTree qt2 = new QuadTree();
            qt2.Nodes[0] = new QuadTree(new QuadTree(6), new QuadTree(7), new QuadTree(4), new QuadTree(4));
            qt2.Nodes[1] = new QuadTree(7);
            qt2.Nodes[2] = new QuadTree(4);
            qt2.Nodes[3] = new QuadTree(6);

            //Intersect the two quadtrees.
            QuadTree output = Intersect(qt1, qt2);

            // Testing
            QuadTree expected = new QuadTree();
            expected.Nodes[0] = new QuadTree(new QuadTree(-1), new QuadTree(7), new QuadTree(-1), new QuadTree(4));
            expected.Nodes[1] = new QuadTree(-1);
            expected.Nodes[2] = new QuadTree(-1);
            expected.Nodes[3] = new QuadTree(new QuadTree(6), new QuadTree(new QuadTree(-1), new QuadTree(6), new QuadTree(-1), new QuadTree(-1)), new QuadTree(-1), new QuadTree(-1));
            Debug.Assert(output.Equals(expected));

            //Done
            Console.WriteLine("Done...");
            Console.ReadKey();
        }

        public static QuadTree Intersect(QuadTree quadOne, QuadTree quadTwo)
        {
            QuadTree quadReturn = new QuadTree();
            
            // Compare the values
            quadReturn.Value = quadOne.Value == quadTwo.Value ? quadOne.Value : -1;

            // Use the node parent depending on which do and do not have children. Recursively call Intersect on each node in the quadtree. 
            if (quadOne.HasChildren && !quadTwo.HasChildren)
            {
                for (int i = 0; i < 4; i++)
                {
                    quadReturn.Nodes[i] = Intersect(quadOne.Nodes[i], quadTwo);
                }
            }
            else if (quadTwo.HasChildren && !quadOne.HasChildren)
            {
                for (int i = 0; i < 4; i++)
                {
                    quadReturn.Nodes[i] = Intersect(quadOne, quadTwo.Nodes[i]);
                }
            } 
            else if (quadOne.HasChildren && quadTwo.HasChildren)
            {
                for (int i = 0; i < 4; i++)
                {
                    quadReturn.Nodes[i] = Intersect(quadOne.Nodes[i], quadTwo.Nodes[i]);
                }
            }
            else
            {
                quadReturn.HasChildren = false;
            }

            return quadReturn;
        }

        public bool Equals(QuadTree quadOne, QuadTree quadTwo)
        {
            bool nodesMatch = false;

            if (quadOne.Value == quadTwo.Value && quadOne.HasChildren == quadTwo.HasChildren)
            {
                nodesMatch = true;
                for (int i = 0; i < 4 && nodesMatch && quadOne.HasChildren; i++)
                {
                    nodesMatch &= quadOne.Nodes[i].Equals(quadTwo.Nodes[i]);
                }
            }

            return nodesMatch;
        }
    }
}
