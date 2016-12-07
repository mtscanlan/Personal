using System;

namespace ToastUtil
{
    public static class ArgumentHelper
    {
        public static T ArgumentNullCheck<T>(this T value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentException($"{nameof(paramName)} cannot be null.");
            }

            return value;
        }
    }
}
