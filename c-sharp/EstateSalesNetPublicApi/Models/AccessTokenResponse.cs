using RestSharp.Deserializers;

namespace EstateSalesNetPublicApi.Models
{
    public class AccessTokenResponse
    {
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "token_type")]
        public string TokenType { get; set; }

        [DeserializeAs(Name = "expires_in")]
        public int ExpiresIn { get; set; }
    }
}
