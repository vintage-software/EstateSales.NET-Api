using RestSharp;
using System;

namespace EstateSalesNetPublicApi
{
    public static class Extensions
    {
        public static void VaildateResponse(this IRestResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if ((int)response.StatusCode < 200 || (int)response.StatusCode > 299)
            {
                throw new Exception($"The API request failed with HTTP {response.StatusCode}. ({response.Content})");
            }
        }
    }
}
