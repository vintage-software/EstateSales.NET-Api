using System;

namespace EstateSalesNetPublicApi.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string validationMessage)
            : base($"Validation Message: {validationMessage}")
        {
        }
    }
}
