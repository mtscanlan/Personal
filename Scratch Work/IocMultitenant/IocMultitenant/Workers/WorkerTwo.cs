using System;
using IocMultitenant.Message;

namespace IocMultitenant.Workers
{
	public class WorkerTwo : IWorker
	{
		public void DisplayName()
		{
			Console.WriteLine(nameof(WorkerTwo));
		}
	}
}
