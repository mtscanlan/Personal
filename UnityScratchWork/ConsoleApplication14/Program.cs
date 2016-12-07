using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.Unity;

namespace ConsoleApplication14
{
    class Program
    {
        static void Main(string[] args)
        {

            //#1: Using InjectionConstructor & InjectionFactory
            var unityContinar = new UnityContainer();
            unityContinar.RegisterType<IBar, Bar>();
            unityContinar.RegisterType<IFoo, Foo>();
            //unityContinar.RegisterType<IFoo, Foo>(new InjectionConstructor(unityContinar.Resolve<IBar>())); //IBar is resolved from this call - also same instance of IBar is used for all IFoo's
            //unityContinar.RegisterType<IFoo, Foo>(new InjectionConstructor(typeof(IBar))); //IBar is resolved when IFoo is created
            //unityContinar.RegisterType<IFoo, Foo>(new InjectionFactory((container) => new Foo(container.Resolve<IBar>()))); //delegate is executed when IFoo is created
            //unityContinar.Resolve<IFoo>();
            //unityContinar.Resolve<IFoo>();

            //#2: Disposing with a Transient life time : FooBar implements IDisposable
            //var unityContinar = new UnityContainer();
            //unityContinar.RegisterType<IBar, Bar>();
            //unityContinar.RegisterType<IFoo, Foo>();
            //unityContinar.RegisterType<IFooBar, FooBar>();
            //unityContinar.Resolve<IFooBar>();
            //unityContinar.Dispose();

            //****************************************************

            //#3: Disposing with ContainerControlled lifetime - For guide only (relevant for child containers)
            //var unityContinar = new UnityContainer();
            //unityContinar.RegisterType<IBar, Bar>();
            //unityContinar.RegisterType<IFoo, Foo>();
            //unityContinar.RegisterType<IFooBar, FooBar>(new ContainerControlledLifetimeManager());
            //unityContinar.Resolve<IFooBar>();
            //unityContinar.Dispose();

            Console.WriteLine("Test! Press any key to exit!");
            Console.ReadKey();

        }
    }
}
