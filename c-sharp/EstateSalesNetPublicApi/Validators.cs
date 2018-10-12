using EstateSalesNetPublicApi.Exceptions;
using System.Text.RegularExpressions;

namespace EstateSalesNetPublicApi
{
    public static class Validators
    {
        public static class RequiredShortString
        {
            public static string ValidationMessage
            {
                get
                {
                    return "This value is required and must be at most 100 characters.";
                }
            }

            public static bool IsValid(string address)
            {
                return address != null && address.Length > 0 && address.Length <= 100;
            }

            public static void Validate(string address)
            {
                if (IsValid(address) == false)
                {
                    throw new ValidationException(ValidationMessage);
                }
            }
        }

        public static class OptionalLongString
        {
            public static string ValidationMessage
            {
                get
                {
                    return "This value must be at most 1073741822 characters.";
                }
            }

            public static bool IsValid(string terms)
            {
                return (terms == null || terms.Length == 0) ? true : terms.Length <= 1073741822;
            }

            public static void Validate(string terms)
            {
                if (IsValid(terms) == false)
                {
                    throw new ValidationException(ValidationMessage);
                }
            }
        }

        public static class PostalCode
        {
            private static string pattern = @"^\d{5}$";

            public static string ValidationMessage
            {
                get
                {
                    return "This value is required and must contain exactly 5 numerical digits.";
                }
            }

            public static bool IsValid(string postalCodeNumber)
            {
                return postalCodeNumber != null && Regex.IsMatch(postalCodeNumber, pattern);
            }

            public static void Validate(string postalCodeNumber)
            {
                if (IsValid(postalCodeNumber) == false)
                {
                    throw new ValidationException(ValidationMessage);
                }
            }
        }

        public static class OptionalUrl
        {
            private static string pattern = "(?:(?:https?)://|www\\.|ftp\\.)[-A-Z0-9+&@#\\/%=~_|$?!:,.]*\\.[-A-Z0-9+&@#;:\\/%=~_|?$!]+";

            public static string ValidationMessage
            {
                get
                {
                    return "This value must be a valid URL.";
                }
            }

            public static bool IsValid(string url)
            {
                return (url == null || url.Length == 0) ? true : Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase);
            }

            public static void Validate(string url)
            {
                if (IsValid(url) == false)
                {
                    throw new ValidationException(ValidationMessage);
                }
            }
        }

        public static class SaleDuration
        {
            public static string ValidationMessage
            {
                get
                {
                    return "This value must be an integer greater than 0 and less than 5.";
                }
            }

            public static bool IsValid(int duration)
            {
                return duration > 0 && duration < 5;
            }

            public static void Validate(int duration)
            {
                if (IsValid(duration) == false)
                {
                    throw new ValidationException(ValidationMessage);
                }
            }
        }
    }
}
