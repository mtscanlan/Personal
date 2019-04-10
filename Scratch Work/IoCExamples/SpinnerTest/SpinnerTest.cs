using SpinnerService;
using SpinnerService.Contracts;
using SpinnerService.Worlds;
using StructureMap;
using Xunit;

namespace SpinnerTest
{
    public class SpinnerTest
    {
        /// <summary>
        /// Initialize the system under test by calling its helper method
        /// </summary>
        private readonly Container _sut = 
            SpinnerBuilder.GetSpinnerService("Profile1", "Profile2");

        [Theory]
        [InlineData(1, 2, 3, "Profile1")] // Profile 1 should spin forward +1
        [InlineData(-1, -2, -3, "Profile2")] // Profile 2 should spin in reverse -1
        public void SpinnerBuilderGetSpinnerService_ISpinner_CorrectSequencingBehaviour(
            int expectedSpinOne,
            int expectedSpinTwo,
            int expectedSpinThree,
            string profileName)
        {
            ISpinner spinner = _sut.GetProfile(profileName).GetInstance<ISpinner>();

            Assert.Equal(expected: expectedSpinOne, actual: spinner.Value);
            Assert.Equal(expected: expectedSpinTwo, actual: spinner.Value);
            Assert.Equal(expected: expectedSpinThree, actual: spinner.Value);
        }

        [Theory]
        [InlineData("Hello, World!", "Profile1")] // Profile 1 should print "Hello, World!
        [InlineData("Hello, World!", "Profile2")] // Profile 1 should print "Hello, World!
        public void SpinnerBuilderGetSpinnerService_IWorldGreeter_CorrectGreetingBehaviour(
            string greeting,
            string profileName)
        {
            IWorldGreeter greeter = _sut.GetProfile(profileName).GetInstance<IWorldGreeter>();

            Assert.Equal(expected: greeting, actual: greeter.Message);
        }
    }
}
