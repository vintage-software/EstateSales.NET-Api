namespace EstateSalesNetPublicApi.Models
{
    public class SalePicture
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public byte[] ImageData { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public bool IsSold { get; set; }
    }
}
