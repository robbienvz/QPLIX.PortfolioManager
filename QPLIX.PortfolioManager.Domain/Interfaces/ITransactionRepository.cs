using QPLIX.PortfolioManager.Domain.Models;

namespace QPLIX.PortfolioManager.Domain.Repositories
{
    public interface ITransactionRepository
    {
        List<Transactions> GetTransactionsByInvestmentIdsAndDate(HashSet<string> investmentIds, DateTime portfolioAsOfDate);
    }
}
