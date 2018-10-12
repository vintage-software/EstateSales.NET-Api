using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EstateSalesNetPublicApi.Models
{
    public class Sale
    {
        private string name;
        private string address;
        private string postalCodeNumber;
        private string directions;
        private string description;
        private string terms;

        public int? Id { get; set; }

        public int? OrgId { get; set; }

        public SaleType? SaleType { get; set; }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                Validators.RequiredShortString.Validate(this.name);
            }
        }

        public string Address
        {
            get
            {
                return this.address;
            }

            set
            {
                this.address = value;
                Validators.RequiredShortString.Validate(this.address);
            }
        }

        public string CrimeWorriesAddress { get; set; }

        public string PostalCodeNumber
        {
            get
            {
                return this.postalCodeNumber;
            }

            set
            {
                this.postalCodeNumber = value;
                Validators.PostalCode.Validate(this.postalCodeNumber);
            }
        }

        public string Directions
        {
            get
            {
                return this.directions;
            }

            set
            {
                this.directions = value;
                Validators.OptionalLongString.Validate(this.directions);
            }
        }

        public DateTime? UtcCustomDateToShowAddress { get; set; }

        public ShowAddressType? ShowAddressType { get; set; }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
                Validators.OptionalLongString.Validate(this.description);
            }
        }

        public string Terms
        {
            get
            {
                return this.terms;
            }

            set
            {
                this.terms = value;
                Validators.OptionalLongString.Validate(this.terms);
            }
        }

        public string AuctionUrl { get; set; }

        public string VideoUrl { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "JSON Deserialization")]
        public string Url { get; private set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "JSON Deserialization")]
        public bool? IsPublished { get; private set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "JSON Deserialization")]
        public decimal? CurrentBalance { get; private set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "JSON Deserialization")]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Needs To Be Generic List")]
        public List<SaleFee> Fees { get; private set; }

        public DateTime? UtcDateCreated { get; set; }

        public DateTime? UtcDateModified { get; set; }

        public Sale ValidateBusinessRules()
        {
            ExceptionHelpers.Assert(this.OrgId.HasValue);
            ExceptionHelpers.Assert(this.SaleType.HasValue);
            ExceptionHelpers.Assert(Validators.RequiredShortString.IsValid(this.name));
            ExceptionHelpers.Assert(Validators.RequiredShortString.IsValid(this.address));
            ExceptionHelpers.Assert(Validators.PostalCode.IsValid(this.postalCodeNumber));
            ExceptionHelpers.Assert(Validators.OptionalLongString.IsValid(this.directions));
            ExceptionHelpers.Assert(Validators.OptionalLongString.IsValid(this.description));
            ExceptionHelpers.Assert(Validators.OptionalLongString.IsValid(this.terms));
            ExceptionHelpers.Assert(this.ShowAddressType.HasValue);

            if (string.IsNullOrEmpty(this.AuctionUrl) == false)
            {
                ExceptionHelpers.Assert(this.SaleType == Models.SaleType.OnlineOnlyAuctions);
            }

            return this;
        }
    }
}
