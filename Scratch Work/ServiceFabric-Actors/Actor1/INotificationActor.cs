using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Actor1
{
    public interface INotificationActor : IActor
    {
        Task DoStuff();
    }
}
