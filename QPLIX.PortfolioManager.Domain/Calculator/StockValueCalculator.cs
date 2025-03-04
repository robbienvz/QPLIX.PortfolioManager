using QPLIX.PortfolioManager.Domain.Interfaces;
using QPLIX.PortfolioManager.Domain.Models;
using QPLIX.PortfolioManager.Domain.Repositories;

namespace QPLIX.PortfolioManager.Repositories
{
    public class StockValueCalculator : IStockValueCalculator
    {
        private readonly IPriceRepository _priceRepository;

        public StockValueCalculator(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public decimal CalculateStockValue(IGrouping<string, Investments> investmentType, List<Transactions> allTransactions, DateTime referenceDate)
        {
            decimal stockValue = 0;
            var sharePrices = investmentType.Select(i => i.ISIN).Distinct().ToDictionary(isin => isin, isin => GetSharePrice(isin, referenceDate));

            foreach (var item in investmentType)
            {
                var investmentTransactions = allTransactions.Where(t => t.InvestmentId == item.InvestmentId).ToList();
                var totalUnits = investmentTransactions.Sum(t => t.TransactionValue);
                var sharePrice = sharePrices[item.ISIN];

                stockValue += totalUnits * sharePrice;
            }

            return stockValue;
        }

        private decimal GetSharePrice(string isin, DateTime tradeDate)
        {
            var quote = _priceRepository.GetQuoteByISINAndDate(isin, tradeDate);
            return quote?.PricePerShare ?? 0;
        }
    }
}
