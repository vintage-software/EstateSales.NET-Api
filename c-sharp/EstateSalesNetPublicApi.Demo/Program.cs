using EstateSalesNetPublicApi.Demo.Services;
using EstateSalesNetPublicApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EstateSalesNetPublicApi.Demo
{
    public static class Program
    {
        private static string apiBaseUrl = GetApiBaseUrl(production: false);

        public static void Main()
        {
            try
            {
                ConfigureConsoleWindow();
                User user = UserService.GetUser(apiBaseUrl);
                UserAction action = UserService.GetUserAction();

                while (action != UserAction.Quit)
                {
                    switch (action)
                    {
                        case UserAction.ViewActiveSales:
                            PrintActiveSales(user);
                            break;
                        case UserAction.CreateSale:
                            CreateSale(user);
                            break;
                        case UserAction.DeleteSale:
                            DeleteSale(user);
                            break;
                        case UserAction.PublishSale:
                            PublishSale(user);
                            break;
                        case UserAction.UnpublishSale:
                            UnpublishSale(user);
                            break;
                        default:
                            throw new Exception("UserAction value not recognized.");
                    }

                    ConsoleHelpers.Pause();
                    Console.WriteLine();
                    action = UserService.GetUserAction();
                }

                Console.WriteLine();
                Console.WriteLine("Thank you for using the EstateSales.NET API!");
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine($"An error occurred: {exception.Message}");
                Console.WriteLine();
            }

            ConsoleHelpers.Pause(exiting: true);
        }

        private static void DeleteSale(User user)
        {
            List<Sale> activeSales = PrintActiveSales(user, showSaleDetails: false);

            if (activeSales.Count > 0)
            {
                Sale sale = SaleService.GetOneSale(activeSales, "Enter the ID of the sale you want to delete");

                IEnumerable<SalePicture> pictures = user.GetSalePictures(sale.Id.Value);
                foreach (SalePicture picture in pictures)
                {
                    user.DeletePicture(picture.Id.Value);
                }

                IEnumerable<SaleDate> dates = user.GetSaleDates(sale.Id.Value);
                foreach (SaleDate saleDate in dates)
                {
                    user.DeleteSaleDate(saleDate.Id.Value);
                }

                user.DeleteSale(sale.Id.Value);
                Console.WriteLine($"Sale #{sale.Id.Value} has been deleted. The pictures and dates for this sale have also been deleted.");
            }
        }

        private static void CreateSale(User user)
        {
            Console.WriteLine();
            Console.WriteLine("Creating a new sale...");
            Sale sale = SaleService.GetSaleFromUserInput(user);

            Console.WriteLine("Saving the new sale...");
            sale = user.SaveSale(sale);

            foreach (SaleDate date in SaleDateService.GetSaleDates(sale))
            {
                user.CreateSaleDate(date);
            }

            string picturePath = ConsoleHelpers.Prompt<string>("Enter a local path to a picture (optional)");
            if (string.IsNullOrEmpty(picturePath) == false)
            {
                if (File.Exists(picturePath))
                {
                    Console.WriteLine("Uploading your sale picture...");
                    user.CreateSalePicture(SalePictureService.GetSalePicture(sale.Id.Value, "sale picture description", picturePath));
                }
                else
                {
                    Console.WriteLine("File not found, skipping picture upload.");
                }
            }

            Process.Start($"{apiBaseUrl}{sale.Url}");

            Console.WriteLine();
            Console.WriteLine($"You can view your new sale at {apiBaseUrl}{sale.Url}.");

            // You can update things on your sale if you need to.
            ////sale.Directions = "These are my new directions.";
            ////sale.Description = "This is my updated description.";
            ////client.UpdateSale(sale);
        }

        private static void PublishSale(User user)
        {
            List<Sale> activeSales = PrintActiveSales(user, showSaleDetails: false);

            if (activeSales.Count > 0)
            {
                Sale sale = SaleService.GetOneSale(activeSales, "Enter the ID of the sale you want to publish");
                user.PublishSale(sale.Id.Value, autoPayAnyBalance: false);
                Console.WriteLine($"Sale #{sale.Id.Value} has been published.");
            }
        }

        private static void UnpublishSale(User user)
        {
            List<Sale> activeSales = PrintActiveSales(user, showSaleDetails: false);

            if (activeSales.Count > 0)
            {
                Sale sale = SaleService.GetOneSale(activeSales, "Enter the ID of the sale you want to unpublish");
                user.UnpublishSale(sale.Id.Value);
                Console.WriteLine($"Sale #{sale.Id.Value} has been unpublished.");
            }
        }

        private static List<Sale> PrintActiveSales(User user, bool showSaleDetails = true)
        {
            Console.WriteLine();
            Console.WriteLine("Here is a list of your active sales");
            Console.WriteLine();

            IReadOnlyCollection<Sale> activeSales = user.GetActiveSales();

            if (activeSales.Any())
            {
                foreach (Sale activeSale in activeSales)
                {
                    if (showSaleDetails)
                    {
                        Console.WriteLine($"Sale #{activeSale.Id}");
                        Console.WriteLine($"  Name: {activeSale.Name}");
                        Console.WriteLine($"  Status: {(activeSale.IsPublished.Value ? "Published" : "Not Published")}");
                        Console.WriteLine($"  URL: {apiBaseUrl}{activeSale.Url}");
                        Console.WriteLine($"  Picture Count: {user.GetSalePictures(activeSale.Id.Value).Count}");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"Sale #{activeSale.Id} ({activeSale.Name})");
                    }
                }

                return activeSales.ToList();
            }
            else
            {
                Console.WriteLine("You do not have any active sales.");
                return new List<Sale>();
            }
        }

        private static void ConfigureConsoleWindow()
        {
            Console.Title = "EstateSales.NET Public Sales API Demo";
        }

        private static string GetApiBaseUrl(bool production = true)
        {
            return production ? "https://estatesales.net" : "https://localhost:1234";
        }
    }
}
