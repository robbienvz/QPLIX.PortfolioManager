using QPLIX.PortfolioManager.Domain.Models;
using QPLIX.PortfolioManager.Domain.Repositories;

namespace QPLIX.PortfolioManager.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly List<Prices> _quotes;

        public PriceRepository(List<Prices> quotes)
        {
            _quotes = quotes;
        }

        public Prices GetQuoteByISINAndDate(string isin, DateTime tradeDate)
        {
            return _quotes.Where(i => i.ISIN == isin && i.TransactionDate <= tradeDate).OrderByDescending
                                (j => j.TransactionDate).FirstOrDefault();
        }
    }
}
