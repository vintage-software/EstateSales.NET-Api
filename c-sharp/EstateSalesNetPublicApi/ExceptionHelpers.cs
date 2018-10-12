using System;

namespace EstateSalesNetPublicApi
{
    public static class ExceptionHelpers
    {
        public static void Assert(bool predicate)
        {
            if (predicate == false)
            {
                throw new Exception("Assertion failed");
            }
        }
    }
}
