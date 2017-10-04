using EstateSalesNetPublicApi;
using System;
using System.Collections.Generic;
using Models = EstateSalesNetPublicApi.Models;

namespace DemoUseOfApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set your API Key
            string apiKey = "SET_YOUR_API_KEY";

            // Quick check for a first time user.
            CheckDefaultValues(apiKey);

            // First create a new client to interact with the API
            Client client = new Client(apiKey);

            // You can do things like get your current list of sales
            // The number 1234 below is the orgID (or your account number)
            List<Models.Sale> currentSales = client.GetActiveSales(1234);

            // You can request one of your sales and see the values currently stored
            // The number 12345 below is the saleID
            Models.Sale testSale = client.GetSale(12345);

            // You can create a sale
            Models.Sale savedSale = client.CreateSale(GetSampleSale());

            // You can use that savedSale and create some dates on your sale
            // We are just setting up the objects to send...
            Models.SaleDate date1 = GetSampleSaleDate(savedSale.Id, DateTime.Parse("9/23/2017 9am"), DateTime.Parse("9/23/2017 4pm"));
            Models.SaleDate date2 = GetSampleSaleDate(savedSale.Id, DateTime.Parse("9/24/2017 9am"), DateTime.Parse("9/24/2017 4pm"));
            Models.SaleDate date3 = GetSampleSaleDate(savedSale.Id, DateTime.Parse("9/25/2017 9am"), DateTime.Parse("9/25/2017 4pm"));

            // Now we are sending those to the API to be saved
            Models.SaleDate savedDate1 = client.CreateSaleDate(date1);
            Models.SaleDate savedDate2 = client.CreateSaleDate(date2);
            Models.SaleDate savedDate3 = client.CreateSaleDate(date3);

            // Get a picture ready to go
            Models.SalePicture pic1 = GetSampleSalePicture(savedSale.Id, "testing", @"C:\Users\mmcquade\Pictures\testing.jpg");

            // Send it to the API to be saved
            Models.SalePicture savedPicture = client.CreateSalePicture(pic1);

            // You can publish your sale
            client.PublishSale(savedSale.Id, true);

            // You can unpublish your sale
            client.UnpublishSale(savedSale.Id);

            // You can update things on your sale if you need to
            savedSale.Directions = "These are my new directions";

            // savedSale.Description = "This is my updated description";
            client.UpdateSale(savedSale);

            // You can delete any pictures you'd like (as long as you saved their id when initially saving them)
            client.DeletePicture(savedPicture.Id);

            // You can delete any dates you'd like (as long as you saved their id when initially saving them)
            client.DeleteSaleDate(savedDate1.Id);
            client.DeleteSaleDate(savedDate2.Id);
            client.DeleteSaleDate(savedDate3.Id);

            // You can delete your sale if you need to
            client.DeleteSale(savedSale.Id);
        }

        private static void CheckDefaultValues(string apiKey)
        {
            if (apiKey == "SET_YOUR_API_KEY")
            {
                throw new Exception("You must create an API key inside your EstateSales.NET account. Go to www.estatesales.net/account/org/api-keys to do this.");
            }

            if (GetSampleSale().OrgId == 1234)
            {
                throw new Exception("You need to update the GetSampleSale() code below to use your actual orgID (your EstateSales.NET account number)");
            }
        }

        private static Models.Sale GetSampleSale()
        {
            return new Models.Sale()
            {
                // Id = 12345,   Be sure to set the Id when you are using this object to edit
                OrgId = 1234,
                SaleType = Models.SaleTypeEnum.EstateSales,
                Name = "This is my sale name",
                Address = "100 My Address",
                CrimeWorriesAddress = string.Empty,
                PostalCodeNumber = "63755",
                Directions = "These are my directions",
                UtcCustomDateToShowAddress = null,
                ShowAddressType = Models.ShowAddressTypeEnum.DayBeforeAtNine,
                Description = "<p>This is my description</p><ul><li>My Item</li><li>My Other Item</li></ul>",
                Terms = "These are my terms",
                AuctionUrl = string.Empty,  // Only set when the sale type is set to OnlineOnlyAuction
                VideoUrl = string.Empty
            };
        }

        private static Models.SaleDate GetSampleSaleDate(int saleId, DateTime utcStartDate, DateTime utcEndDate)
        {
            // Keep in mind when setting up the object to send that all dates and times are in UTC time.
            return new Models.SaleDate()
            {
                SaleId = saleId,
                UtcStartDate = utcStartDate,
                UtcEndDate = utcEndDate,
                ShowEndTime = true
            };
        }

        private static Models.SalePicture GetSampleSalePicture(int saleId, string description, string pathToImage)
        {
            // This is an example of getting an image from a file path, but you just need to set the byte array
            // It doesn't HAVE to come from the file system.
            byte[] imageData = System.IO.File.ReadAllBytes(pathToImage);

            return new Models.SalePicture()
            {
                SaleId = saleId,
                Description = description,
                ImageData = imageData
            };
        }
    }
}
