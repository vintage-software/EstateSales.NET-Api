using EstateSalesNetPublicApi.Models;
using System;
using System.Collections.Generic;

namespace EstateSalesNetPublicApi.Demo.Services
{
    public class SaleDateService
    {
        public static List<SaleDate> GetSaleDates(Sale sale)
        {
            List<SaleDate> dates = new List<SaleDate>();
            DateTime tomorrowAt9am = DateTime.Now.Date.AddDays(1).AddHours(9).ToUniversalTime();
            DateTime tomorrowAt4pm = DateTime.Now.Date.AddDays(1).AddHours(16).ToUniversalTime();
            int duration = GetSaleDuration();

            for (int i = 0; i < duration; i++)
            {
                dates.Add(GetSaleDate(sale.Id.Value, tomorrowAt9am.AddDays(i), tomorrowAt4pm.AddDays(i)));
            }

            return dates;
        }

        private static SaleDate GetSaleDate(int saleId, DateTime utcStartDate, DateTime utcEndDate)
        {
            SaleDate date = new SaleDate()
            {
                SaleId = saleId,
                UtcStartDate = utcStartDate,
                UtcEndDate = utcEndDate,
                ShowEndTime = true
            };

            return date.ValidateBusinessRules();
        }

        private static int GetSaleDuration()
        {
            int duration = ConsoleHelpers.Prompt<int>("Enter the number of days your sale will last");

            while (Validators.SaleDuration.IsValid(duration) == false)
            {
                Console.WriteLine(Validators.SaleDuration.ValidationMessage);
                duration = ConsoleHelpers.Prompt<int>("Enter the number of days your sale will last");
            }

            return duration;
        }
    }
}
