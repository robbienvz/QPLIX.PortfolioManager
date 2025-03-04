using QPLIX.PortfolioManager.Domain.Models;

namespace QPLIX.PortfolioManager.Domain.Interfaces
{
    public interface IStockValueCalculator
    {
        decimal CalculateStockValue(IGrouping<string, Investments> investmentType, List<Transactions> allTransactions, DateTime portfolioAsOfDate);
    }
}
