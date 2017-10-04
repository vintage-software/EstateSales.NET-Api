using System.Diagnostics.CodeAnalysis;

namespace EstateSalesNetPublicApi.Models
{
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not meant to be used as flags in public API.")]
    public enum SaleType
    {
        None = 0,
        EstateSales = 1,
        Auctions = 2,
        MovingSales = 4,
        BusinessLiquidations = 8,
        MovedOffsiteToWarehouse = 16,
        ByAppointment = 32,
        OnlineOnlyAuctions = 64,
        AuctionHouse = 128,
        MovedOffsiteToStore = 256,
        CharitySales = 512,
        OutsideSales = 1024,
        SingleItemTypeCollections = 2048,
        BuyoutsOrCleanouts = 4096,
        DemolitionSales = 8192
    }
}
