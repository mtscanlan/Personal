using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ObservablePiece : IObserver<IPublishingObject>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IPublishingObject value)
        {
            value.DoStuff();
        }
    }
}
