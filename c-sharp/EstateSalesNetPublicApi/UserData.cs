namespace EstateSalesNetPublicApi
{
    public class UserData
    {
        public UserData(string apiKey, int orgId)
        {
            this.ApiKey = apiKey;
            this.OrgId = orgId;
        }

        public string ApiKey { get; set; }

        public int OrgId { get; set; }
    }
}
