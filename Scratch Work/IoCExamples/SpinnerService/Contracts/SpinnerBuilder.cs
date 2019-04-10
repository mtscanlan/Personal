using SpinnerService.Worlds;
using StructureMap;

namespace SpinnerService.Contracts
{
    public static class SpinnerBuilder
    {
        public static Container GetSpinnerService(string profileOneName, string profileTwoName)
        {
            return new Container(c =>
            {
                // Register WorldGreeter as implementation to IWorldGreeter interface
                c.For<IWorldGreeter>().Singleton().Use(new WorldGreeter());
                c.For<ISpinner>().Singleton().Use(new DefaultSpinner());

                c.Profile(profileOneName, p => { /* Register new profile "Profile1" with default implementation */ });

                c.Profile(profileTwoName, p => {
                    // Register new profile "Profile2" with ReverseSpinner as implentation to ISpinner interface
                    p.For<ISpinner>().Singleton().Use<ReverseSpinner>();
                });
            });
        }
    }
}
