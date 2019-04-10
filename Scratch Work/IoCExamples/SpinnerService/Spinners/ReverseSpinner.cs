namespace SpinnerService
{
    public class ReverseSpinner : ISpinner
    {
        private int _value = 0;

        public int Value => --_value;
    }
}
