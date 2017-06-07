using System;
using IocMultitenant.Message;

namespace IocMultitenant.Workers
{
	public class WorkerOne : IWorker
	{
		public void DisplayName()
		{
			Console.WriteLine(nameof(WorkerOne));
		}
	}
}
