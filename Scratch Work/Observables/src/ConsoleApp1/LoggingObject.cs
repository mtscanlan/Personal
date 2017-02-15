using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class LoggingObject : IPublishingObject
    {
        public void DoStuff()
        {
            Console.WriteLine(typeof(LoggingObject).FullName);
        }
    }
}
