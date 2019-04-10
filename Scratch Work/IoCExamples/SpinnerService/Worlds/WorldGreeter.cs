namespace SpinnerService.Worlds
{
    public class WorldGreeter : IWorldGreeter
    {
        public WorldGreeter() { }

        public string Message { get; set; } = "Hello, World!";
    }
}
