using EstateSalesNetPublicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EstateSalesNetPublicApi.Demo.Services
{
    public static class SaleService
    {
        public static Sale GetSaleFromUserInput(User user)
        {
            Sale sale = new Sale()
            {
                OrgId = user.OrgId,
                SaleType = SaleType.EstateSales,
                Name = GetName(),
                Address = GetAddress(),
                CrimeWorriesAddress = string.Empty,
                PostalCodeNumber = GetPostalCodeNumber(),
                Directions = GetDirections(),
                UtcCustomDateToShowAddress = null,
                ShowAddressType = ShowAddressType.DayBeforeAtNine,
                Description = GetDescription(),
                Terms = GetTerms(),
                AuctionUrl = string.Empty,
                VideoUrl = GetVideoUrl()
            };

            return sale.ValidateBusinessRules();
        }

        public static Sale GetOneSale(List<Sale> sales, string message)
        {
            int saleId = ConsoleHelpers.Prompt<int>(message);

            while (sales.Count(sale => sale.Id == saleId) == 0)
            {
                Console.WriteLine("That sale ID was not found.");
                saleId = ConsoleHelpers.Prompt<int>(message);
            }

            return sales.First(sale => sale.Id == saleId);
        }

        private static string GetName()
        {
            string name = ConsoleHelpers.Prompt<string>("Enter a name for your sale");

            while (Validators.RequiredShortString.IsValid(name) == false)
            {
                Console.WriteLine(Validators.RequiredShortString.ValidationMessage);
                name = ConsoleHelpers.Prompt<string>("Enter a name for your sale");
            }

            return name;
        }

        private static string GetAddress()
        {
            string address = ConsoleHelpers.Prompt<string>("Enter an address for your sale");

            while (Validators.RequiredShortString.IsValid(address) == false)
            {
                Console.WriteLine(Validators.RequiredShortString.ValidationMessage);
                address = ConsoleHelpers.Prompt<string>("Enter an address for your sale");
            }

            return address;
        }

        private static string GetPostalCodeNumber()
        {
            string postalCodeNumber = ConsoleHelpers.Prompt<string>("Enter a zip code for your sale");

            while (Validators.PostalCode.IsValid(postalCodeNumber) == false)
            {
                Console.WriteLine(Validators.PostalCode.ValidationMessage);
                postalCodeNumber = ConsoleHelpers.Prompt<string>("Enter a zip code for your sale");
            }

            return postalCodeNumber;
        }

        private static string GetDirections()
        {
            string directions = ConsoleHelpers.Prompt<string>("Enter the directions for your sale (optional)");

            while (Validators.OptionalLongString.IsValid(directions) == false)
            {
                Console.WriteLine(Validators.OptionalLongString.ValidationMessage);
                directions = ConsoleHelpers.Prompt<string>("Enter the directions for your sale");
            }

            return directions;
        }

        private static string GetDescription()
        {
            string description = ConsoleHelpers.Prompt<string>("Enter a description for your sale (optional)");

            while (Validators.OptionalLongString.IsValid(description) == false)
            {
                Console.WriteLine(Validators.OptionalLongString.ValidationMessage);
                description = ConsoleHelpers.Prompt<string>("Enter a description for your sale");
            }

            return description;
        }

        private static string GetTerms()
        {
            string terms = ConsoleHelpers.Prompt<string>("Enter terms for your sale (optional)");

            while (Validators.OptionalLongString.IsValid(terms) == false)
            {
                Console.WriteLine(Validators.OptionalLongString.ValidationMessage);
                terms = ConsoleHelpers.Prompt<string>("Enter terms for your sale");
            }

            return terms;
        }

        private static string GetVideoUrl()
        {
            string url = ConsoleHelpers.Prompt<string>("Enter a sale video url (optional)");

            while (Validators.OptionalUrl.IsValid(url) == false)
            {
                Console.WriteLine(Validators.OptionalUrl.ValidationMessage);
                url = ConsoleHelpers.Prompt<string>("Enter a sale video url (optional)");
            }

            return url;
        }
    }
}
