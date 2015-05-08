using System;
using System.Collections.Generic;

namespace Simple_Expression {
	class Program {
		/*
			Write a simple parser to parse a formula and calculate the result. 
			Given a string containing only integer numbers, brackets, plus and minus signs, 
			calculate and print the numeric answer. Assume that the formula will always 
			follow correct syntax
			Example input:
			(2+2)-(3-(6-5))-4
			Example output:
			-2
		*/

		public class Node {
			public int Value = 0;
			public char LastOperator = ' ';
		}

		public static void compute_expression(string expr) {
			LinkedList<Node> parentNode = new LinkedList<Node>();
			parentNode.AddLast(new Node());
			LinkedListNode<Node> currentNode = parentNode.Last;

			Action<int> Maths = (total) => {
				if (currentNode != null) {
					switch (currentNode.Value.LastOperator) {
						case '+':
							currentNode.Value.Value = currentNode.Value.Value + total;
							break;
						case '-':
							currentNode.Value.Value = currentNode.Value.Value - total;
							break;
						case ' ':
							currentNode.Value.Value = total;
							break;
						default:
							break;
					}
					currentNode.Value.LastOperator = ' ';
				}
			};
			
			foreach (char c in expr) {
				if (c == '+' || c == '-') {
					currentNode.Value.LastOperator = c;
				} else if (Char.IsDigit(c)) {
					Maths(c - 48);
				} else if (c == '(') {
					parentNode.AddLast(new Node());
					currentNode = parentNode.Last;
				} else {
					int total = currentNode.Value.Value;
					LinkedListNode<Node> tempNode = currentNode;
					currentNode = currentNode.Previous;
					parentNode.Remove(tempNode);
					Maths(total);
				}
			}
			Console.WriteLine(currentNode.Value.Value);
		}

		static void Main(string[] args) {
			compute_expression("(2+2)-(3-(6-5))-4");
			Console.ReadKey();
		}
	}
}
