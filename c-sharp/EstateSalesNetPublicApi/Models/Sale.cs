using System;
using System.Diagnostics.CodeAnalysis;

namespace EstateSalesNetPublicApi.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public int OrgId { get; set; }

        public SaleType SaleType { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string CrimeWorriesAddress { get; set; }

        public string PostalCodeNumber { get; set; }

        public string Directions { get; set; }

        public DateTime? UtcCustomDateToShowAddress { get; set; }

        public ShowAddressType ShowAddressType { get; set; }

        public string Description { get; set; }

        public string Terms { get; set; }

        public string AuctionUrl { get; set; }

        public string VideoUrl { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "JSON Deserialization")]
        public string Url { get; private set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "JSON Deserialization")]
        public bool IsPublished { get; private set; }

        public DateTime UtcDateCreated { get; set; }

        public DateTime UtcDateModified { get; set; }
    }
}
