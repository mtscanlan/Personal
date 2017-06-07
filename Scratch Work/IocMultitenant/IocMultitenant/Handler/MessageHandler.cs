using System.Collections.Concurrent;
using System.Collections.Generic;
using Autofac;
using IocMultitenant.Message;
using IocMultitenant.Workers;

namespace IocMultitenant.Handler
{
	public class MessageHandler
	{
		private BlockingCollection<MessageObject> _queue;
		private IContainer _container;

		public MessageHandler(BlockingCollection<MessageObject> queue, IContainer container)
		{
			_queue = queue;
			_container = container;
		}

		public void DoWork()
		{
			while (true)
			{
				MessageObject message = _queue.Take();
				IWorker worker = _container.ResolveNamed<IWorker>(message.Identifier.ToString());

				worker.DisplayName();
			}
		}
	}
}
