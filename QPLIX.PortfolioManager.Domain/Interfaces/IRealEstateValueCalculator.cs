using QPLIX.PortfolioManager.Domain.Models;

namespace QPLIX.PortfolioManager.Domain.Interfaces
{
    public interface IRealEstateValueCalculator
    {
        decimal CalculateRealEstateValue(IGrouping<string, Investments> investmentType, List<Transactions> allTransactions);
    }
}
