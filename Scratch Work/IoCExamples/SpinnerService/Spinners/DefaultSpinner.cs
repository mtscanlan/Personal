namespace SpinnerService
{
    public class DefaultSpinner : ISpinner
    {
        private int _value = 0;

        public int Value => ++_value;
    }
}
