using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good_Nodes
{
    class Program
    {
        public static bool checkNodes(List<Tuple<int, bool>> goodNodes, int i, int pointedToNode, int counter, int max)
        {
            if (goodNodes[pointedToNode - 1].Item2)
            {
                return true;
            }
            else if (counter >= max || goodNodes[pointedToNode - 1].Item1 == pointedToNode)
            {
                return false;
            }
            else
            {
                return checkNodes(goodNodes, i, goodNodes[pointedToNode - 1].Item1, counter++, max);
            }
        }

        static void Main(string[] args)
        {
            List<Tuple<int, bool>> goodNodes = new List<Tuple<int, bool>>();
            int max = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < max; i++)
            {
                int currentNumber = Convert.ToInt32(Console.ReadLine());
                bool isGoodNode = (currentNumber == 1 || i == 0);
                goodNodes.Add(Tuple.Create(currentNumber, isGoodNode));
            }
            int counter = 0;
            for (int i = 1; i < max; i++)
            {
                if (!goodNodes[i].Item2)
                {
                    int pointedToNode = goodNodes[i].Item1;
                    if (!checkNodes(goodNodes, i, pointedToNode, counter, max))
                    {
                        goodNodes[pointedToNode - 1] = Tuple.Create(1, true);
                        counter++;
                    }
                }
            }
            Console.WriteLine(counter);
        }
    }
}
