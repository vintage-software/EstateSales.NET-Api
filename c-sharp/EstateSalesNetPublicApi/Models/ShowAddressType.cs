using System.Diagnostics.CodeAnalysis;

namespace EstateSalesNetPublicApi.Models
{
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Zero is not a valid SaleAddressType")]
    public enum ShowAddressType
    {
        DayBeforeAtNine = 1,
        ThreeHoursPrior = 2,
        Immediately = 3,
        Custom = 4
    }
}
