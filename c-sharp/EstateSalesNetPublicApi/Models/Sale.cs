using System;

namespace EstateSalesNetPublicApi.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public int OrgId { get; set; }

        public SaleTypeEnum SaleType { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string CrimeWorriesAddress { get; set; }

        public string PostalCodeNumber { get; set; }

        public string Directions { get; set; }

        public DateTime? UtcCustomDateToShowAddress { get; set; }

        public ShowAddressTypeEnum ShowAddressType { get; set; }

        public string Description { get; set; }

        public string Terms { get; set; }

        public string AuctionUrl { get; set; }

        public string VideoUrl { get; set; }

        public string IsPublished { get; internal set; }

        public DateTime UtcDateCreated { get; set; }

        public DateTime UtcDateModified { get; set; }
    }
}
