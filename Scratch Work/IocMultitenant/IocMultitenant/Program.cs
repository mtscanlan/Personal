using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using IocMultitenant.Handler;
using IocMultitenant.Message;
using IocMultitenant.Workers;

namespace IocMultitenant
{
	class Program
	{
		static void Main(string[] args)
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<WorkerOne>().Named<IWorker>(IdentifierEnum.One.ToString());
			builder.RegisterType<WorkerTwo>().Named<IWorker>(IdentifierEnum.Two.ToString());
			var container = builder.Build();
			var queue = new BlockingCollection<MessageObject>();

			var handler = new MessageHandler(queue, container);
			Task.Run(() => handler.DoWork());

			do
			{
				try
				{
					Console.Write("Enter a number (1) or (2): ");
					var input = Console.ReadKey();
					Console.WriteLine();
					IdentifierEnum identifier;
					if (input.Key == ConsoleKey.D1)
						identifier = IdentifierEnum.One;
					else if (input.Key == ConsoleKey.D2)
						identifier = IdentifierEnum.Two;
					else continue;
					
					queue.Add(new MessageObject { Identifier = identifier });
					
					Thread.Sleep(1000);
				}
				catch { }
			} while (true);
		}
	}
}
