using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tree
{
    public class QuadTree
    {
        public Int32 Value { get; set; }

        public QuadTree[] Nodes { get; set; }
        public bool HasChildren { get; set; }

        public QuadTree()
        {
            Value = -1;
            HasChildren = true;
            Nodes = new QuadTree[4];
        }

        public QuadTree(int value)
        {
            HasChildren = false;
            Nodes = new QuadTree[4];
            Value = value;
        }

        public QuadTree(QuadTree node1, QuadTree node2, QuadTree node3, QuadTree node4)
        {
            Value = -1;
            HasChildren = true;
            Nodes = new QuadTree[4];
            Nodes[0] = node1;
            Nodes[1] = node2;
            Nodes[2] = node3;
            Nodes[3] = node4;
        }

        public override bool Equals(object obj)
        {
            bool nodesMatch = false;
            if (obj is QuadTree)
            {
                var castedObj = obj as QuadTree;
                if (Value == castedObj.Value && HasChildren == castedObj.HasChildren)
                {
                    nodesMatch = true;
                    for (int i = 0; i < 4 && nodesMatch && HasChildren; i++)
                    {
                        nodesMatch &= Nodes[i].Equals(castedObj.Nodes[i]);
                    }
                }
            }

            return nodesMatch;
        }
    }
}
