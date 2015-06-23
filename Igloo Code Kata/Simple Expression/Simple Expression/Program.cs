using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

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

		public static void compute_expression(string expr) { // My solution
			LinkedList<Node> parentNode = new LinkedList<Node>();
			parentNode.AddLast(new Node());
			LinkedListNode<Node> currentNode = parentNode.Last;

			Action<int> Maths = (total) => {
				switch (currentNode.Value.LastOperator) {
					case '+':
						currentNode.Value.Value += total;
						break;
					case '-':
						currentNode.Value.Value -= total;
						break;
					case ' ':
						currentNode.Value.Value = total;
						break;
					default:
						break;
				}
				currentNode.Value.LastOperator = ' ';
			};
			
			foreach (char c in expr) {
				if (c == '+' || c == '-') {
					currentNode.Value.LastOperator = c;
				} else if (c == '(') {
					parentNode.AddLast(new Node());
					currentNode = parentNode.Last;
				} else if (c == ')') {
					int total = currentNode.Value.Value;
					LinkedListNode<Node> tempNode = currentNode;
					currentNode = currentNode.Previous;
					parentNode.Remove(tempNode);
					Maths(total);
				} else {
					Maths(c - '0');
				}
			}
			//Console.WriteLine(currentNode.Value.Value);
		}

		public static DataTable dt = new DataTable();
		public static void compute_expression2(string expr) { // Given solution
			var result = dt.Compute(expr, null);
		}

		static void Main(string[] args) {
			int iterations = 1000000;
			Stopwatch s = new Stopwatch();
			s.Start();
			for (int i = 0; i < iterations; i++) {
				compute_expression("(2+2)-(3-(6-5))-4");
			}
			s.Stop();
			Console.WriteLine(s.ElapsedTicks);

			s.Reset();
			s.Start();
			for (int i = 0; i < iterations; i++) {
				compute_expression2("(2+2)-(3-(6-5))-4");
			}
			s.Stop();
			Console.WriteLine(s.ElapsedTicks);

			//compute_expression("(2+2)-(3-(6-5))-4");
			//compute_expression2("(2+2)-(3-(6-5))-4");
			Console.ReadKey();
		}
	}
}
