using QPLIX.PortfolioManager.Domain.Models;

namespace QPLIX.PortfolioManager.Domain.Repositories
{
    public interface IPriceRepository
    {
        Prices GetQuoteByISINAndDate(string isin, DateTime tradeDate);
    }
}
