using QPLIX.PortfolioManager.Domain.Models;

namespace QPLIX.PortfolioManager.Domain.Interfaces
{
    public interface IFundValueCalculator
    {
        decimal CalculateFundValue(Investments investments, DateTime referenceDate,
            Dictionary<string, List<Transactions>> groupedTransactions, Dictionary<string, decimal> sharePrices);
    }
}
