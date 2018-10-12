using System;

namespace EstateSalesNetPublicApi.Models
{
    public class SaleDate
    {
        public int? Id { get; set; }

        public int? SaleId { get; set; }

        public DateTime? UtcStartDate { get; set; }

        public DateTime? UtcEndDate { get; set; }

        public bool? ShowEndTime { get; set; }

        public SaleDate ValidateBusinessRules()
        {
            ExceptionHelpers.Assert(this.SaleId.HasValue);
            ExceptionHelpers.Assert(this.UtcStartDate.HasValue);
            ExceptionHelpers.Assert(this.UtcEndDate.HasValue);
            ExceptionHelpers.Assert(this.UtcStartDate < this.UtcEndDate);
            return this;
        }
    }
}
