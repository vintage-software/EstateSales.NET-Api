using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EstateSalesNetPublicApi.Demo
{
    public static class Program
    {
        private const string ApiBaseUrl = null; // Set to `null` to use the default.

        public static void Main()
        {
            try
            {
                Console.Title = "EstateSales.NET Public Sales API Demo";

                string apiKey = ConsoleHelpers.Prompt<string>("Enter your API Key");
                int orgId = ConsoleHelpers.Prompt<int>("Enter your Org ID");

                // First create a new client to interact with the API.
                Client client = string.IsNullOrEmpty(ApiBaseUrl) ? new Client(apiKey) : new Client(apiKey, ApiBaseUrl);

                // You can do things like get your current list of sales.
                Console.WriteLine();
                Console.WriteLine("Retrieving your active sales...");

                IReadOnlyCollection<Models.Sale> editableSales = client.GetEditableSales(orgId);

                if (editableSales.Any())
                {
                    Console.WriteLine($"You have {editableSales.Count} editable sale(s)!");

                    foreach (Models.Sale editableSale in editableSales)
                    {
                        Console.WriteLine($"{editableSale.Name} ({(editableSale.IsPublished ? "Published" : "Not Published")}) ({ApiBaseUrl}{editableSale.Url})");
                        Console.WriteLine($"Date Count: {client.GetSaleDates(editableSale.Id).Count} / Picture Count: {client.GetSalePictures(editableSale.Id).Count}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("You have no editable sales. :( That's okay though, we're about to learn how to add one!");
                }

                Console.WriteLine();
                Console.WriteLine("Next, we will add a new sale...");
                Console.WriteLine();

                // You can create a sale.
                Models.Sale sale = GetSampleSale(orgId);

                Console.WriteLine();
                Console.WriteLine("Saving your sale...");

                sale = client.CreateSale(sale);

                Console.WriteLine();
                Console.WriteLine("Setting your new sale to run for the next three days...");

                // You can use that savedSale and create some dates on your sale.
                // We are just setting up the objects to send...
                DateTime tomorrowAt9am = DateTime.Now.Date.AddDays(1).AddHours(9).ToUniversalTime();
                DateTime tomorrowAt4pm = DateTime.Now.Date.AddDays(1).AddHours(16).ToUniversalTime();
                Models.SaleDate date1 = GetSampleSaleDate(sale.Id, tomorrowAt9am, tomorrowAt4pm);
                Models.SaleDate date2 = GetSampleSaleDate(sale.Id, tomorrowAt9am.AddDays(1), tomorrowAt4pm.AddDays(1));
                Models.SaleDate date3 = GetSampleSaleDate(sale.Id, tomorrowAt9am.AddDays(2), tomorrowAt4pm.AddDays(2));

                // Now we are sending those to the API to be saved.
                date1 = client.CreateSaleDate(date1);
                date2 = client.CreateSaleDate(date2);
                date3 = client.CreateSaleDate(date3);

                Models.SalePicture pic1 = null;

                Console.WriteLine();
                string picturePath = ConsoleHelpers.Prompt<string>("Enter a path to a picture (optional)");
                if (string.IsNullOrEmpty(picturePath) == false)
                {
                    if (File.Exists(picturePath))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Uploading your sale picture...");

                        // Get a picture ready to go.
                        pic1 = GetSampleSalePicture(sale.Id, "testing", picturePath);

                        // Send it to the API to be saved.
                        pic1 = client.CreateSalePicture(pic1);
                    }
                    else
                    {
                        Console.WriteLine("File not found, skipping picture upload.");
                    }
                }

                // You can publish your sale.
                ////client.PublishSale(sale.Id, false);

                Process.Start($"{ApiBaseUrl ?? "https://www.estatesales.net"}{sale.Url}");

                Console.WriteLine();
                Console.WriteLine($"Okay, your sale created and populated with dates and a picture!");
                Console.WriteLine($"You can view it at https://www.estatesales.net{sale.Url}.");
                ConsoleHelpers.Pause();

                // You can unpublish your sale.
                ////client.UnpublishSale(sale.Id);

                // You can update things on your sale if you need to.
                ////sale.Directions = "These are my new directions.";
                ////sale.Description = "This is my updated description.";
                ////client.UpdateSale(sale);

                Console.WriteLine();
                Console.WriteLine("Next, we will delete the sale we just created...");

                // You can delete any pictures you'd like (as long as you saved their id when initially saving them).
                if (pic1 != null)
                {
                    client.DeletePicture(pic1.Id);
                }

                // You can delete any dates you'd like
                IEnumerable<Models.SaleDate> dates = client.GetSaleDates(sale.Id);
                foreach (Models.SaleDate saleDate in dates)
                {
                    client.DeleteSaleDate(saleDate.Id);
                }

                // If you stored the date ids previously, you don't have to look them up first
                ////client.DeleteSaleDate(date1.Id);
                ////client.DeleteSaleDate(date2.Id);
                ////client.DeleteSaleDate(date3.Id);

                // You can delete your sale if you need to.
                // You do not need to delete the sale dates and sale pictures first. You can just delete the sale.
                client.DeleteSale(sale.Id);

                Console.WriteLine();
                Console.WriteLine("Okay, we're done now. Enjoy using the EstateSales.NET Sales API!");
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine("An error occurred:");
                Console.WriteLine(exception.Message);
                Console.WriteLine();
            }

            ConsoleHelpers.Pause(true);
        }

        private static Models.Sale GetSampleSale(int orgId)
        {
            return new Models.Sale()
            {
                OrgId = orgId,
                SaleType = Models.SaleType.EstateSales,
                Name = ConsoleHelpers.Prompt<string>("Enter a name for your sale"),
                Address = ConsoleHelpers.Prompt<string>("Enter an address for your sale"),
                CrimeWorriesAddress = string.Empty,
                PostalCodeNumber = ConsoleHelpers.Prompt<string>("Enter a zip code for your sale"),
                Directions = "These are my directions.",
                UtcCustomDateToShowAddress = null,
                ShowAddressType = Models.ShowAddressType.DayBeforeAtNine,
                Description = ConsoleHelpers.Prompt<string>("Enter a description for your sale"),
                Terms = ConsoleHelpers.Prompt<string>("Enter terms for your sale"),
                AuctionUrl = string.Empty,  // Only set when the sale type is set to OnlineOnlyAuction
                VideoUrl = ConsoleHelpers.Prompt<string>("Enter a sale video url (optional)")
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

        private static Models.SalePicture GetSampleSalePicture(int saleId, string description, string picturePath)
        {
            // This is an example of getting an image from a file path.
            // But you just need to set the byte array, it doesn't HAVE to come from the file system.
            byte[] imageData = File.ReadAllBytes(picturePath);

            return new Models.SalePicture()
            {
                SaleId = saleId,
                Description = description,
                ImageData = imageData
            };
        }
    }
}
