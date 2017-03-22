using System;

namespace ConsoleApp1
{
	public class Program
    {
        public static void Main(string[] args)
		{
			StringHolder c1 = new StringHolder();
			c1.AString = nameof(Main);
			RefAssign(ref c1);

			Console.WriteLine(c1.AString);

			StringHolder c2 = new StringHolder();
			c2.AString = nameof(Main);
			Assign(c2);

			Console.WriteLine(c2.AString);

			Console.ReadKey();
		}

		public static void RefAssign(ref StringHolder c)
		{
			c = new StringHolder();
			c.AString = nameof(RefAssign);
		}

		public static void Assign(StringHolder c)
		{
			c = new StringHolder();
			c.AString = nameof(Assign);
		}

		public class StringHolder
		{
			public string AString { get; set; }
		}
    }
}
