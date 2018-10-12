using EstateSalesNetPublicApi.Models;
using System.IO;

namespace EstateSalesNetPublicApi.Demo.Services
{
    public class SalePictureService
    {
        public static SalePicture GetSalePicture(int saleId, string description, string picturePath)
        {
            byte[] imageData = File.ReadAllBytes(picturePath);

            SalePicture picture = new SalePicture()
            {
                SaleId = saleId,
                Description = description,
                ImageData = imageData
            };

            return picture.ValidateBusinessRules();
        }
    }
}
