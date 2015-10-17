using System;

namespace AppVeyor.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string input1, string input2)
        {
            return input1.Equals(input2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
