using System;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var observer = new ObservablePiece();
            var mailObject = new MailObject();
            var loggingObject = new LoggingObject();

            observer.OnNext(mailObject);
            observer.OnNext(loggingObject);

            Console.ReadKey();
        }
    }
}
