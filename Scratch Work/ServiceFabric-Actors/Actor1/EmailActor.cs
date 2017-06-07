using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Actor1
{
    [ActorService()]
    public class EmailActor : Actor, INotificationActor
    {
        public EmailActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        public async Task DoStuff()
        {
            await Task.Run(() => Console.WriteLine(nameof(EmailActor)));
        }
    }
}
