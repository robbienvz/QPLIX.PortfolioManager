using QPLIX.PortfolioManager.Common;
using QPLIX.PortfolioManager.Domain.Interfaces;
using QPLIX.PortfolioManager.Domain.Models;
using QPLIX.PortfolioManager.Domain.Repositories;
using System.Transactions;

namespace QPLIX.PortfolioManager.Domain.Services
{
    public class PortfolioService
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IStockValueCalculator _stockValueCalculator;
        private readonly IRealEstateValueCalculator _realEstateValueCalculator;
        private readonly IFundValueCalculator _fundValueCalculator;

        public PortfolioService(IInvestmentRepository investmentRepository, ITransactionRepository transactionRepository,
            IPriceRepository priceRepository, IStockValueCalculator stockValueCalculator,
            IRealEstateValueCalculator realEstateValueCalculator, IFundValueCalculator fundValueCalculator)
        {
            _investmentRepository = investmentRepository;
            _transactionRepository = transactionRepository;
            _priceRepository = priceRepository;
            _stockValueCalculator = stockValueCalculator;
            _realEstateValueCalculator = realEstateValueCalculator;
            _fundValueCalculator = fundValueCalculator;
        }
        public decimal? CalculatePortfolioValue(string investorId, DateTime portfolioAsOfDate)
        {
            var investments = _investmentRepository.GetInvestmentsByInvestorId(investorId);
            if (investments == null || investments.Count == 0)
            {
                return null;
            }
            var investmentTypes = investments.GroupBy(i => i.InvestmentType);
            decimal totalValue = 0;

            var fundInvestmentsByFundIds = GetInvestmentsByFundIds(investments);
            var investmentIds = GetInvestmentIds(investments, fundInvestmentsByFundIds);
            var isinList = GetDistinctIsinList(investments, fundInvestmentsByFundIds);
            var allTransactions = _transactionRepository.GetTransactionsByInvestmentIdsAndDate(new HashSet<string>(investmentIds), portfolioAsOfDate);
            var sharePrices = isinList.ToDictionary(isin => isin, isin => GetSharePrice(isin, portfolioAsOfDate));

            decimal stockValue = 0;
            decimal realEstateValue = 0;
            decimal fundTotalValue = 0;

            foreach (var investmentType in investmentTypes)
            {
                switch (investmentType.Key)
                {
                    case InvestmentType.Stock:
                        stockValue = _stockValueCalculator.CalculateStockValue(investmentType, allTransactions, portfolioAsOfDate);
                        break;

                    case InvestmentType.RealEstate:
                        realEstateValue = _realEstateValueCalculator.CalculateRealEstateValue(investmentType, allTransactions);
                        break;

                    case InvestmentType.Funds:
                        fundTotalValue = CalculateFundValue(investmentType, allTransactions, portfolioAsOfDate, sharePrices);
                        break;
                }
            }

            totalValue = fundTotalValue + stockValue + realEstateValue;

            return totalValue;
        }

        public List<string> GetInvestmentIds(IEnumerable<Investments> investments, List<Investments> fundInvestmentsByFundIds)
        {
            var investmentIds = investments.Select(i => i.InvestmentId).ToList();
            var fundInvestorInvestmentIds = fundInvestmentsByFundIds.Select(i => i.InvestmentId).ToList();
            investmentIds.AddRange(fundInvestorInvestmentIds);

            return investmentIds;
        }

        public List<string> GetDistinctIsinList(IEnumerable<Investments> investments, List<Investments> fundInvestmentsByFundIds)
        {
            var stockIsinList = investments.Where(i => i.InvestmentType == InvestmentType.Stock.ToString()).Select(i => i.ISIN).Distinct().ToList();
            var fundisinList = fundInvestmentsByFundIds.Select(i => i.ISIN).Distinct().ToList();
            var isinList = fundisinList.Union(stockIsinList);

            return isinList.ToList();
        }

        public decimal GetSharePrice(string isin, DateTime tradeDate)
        {
            var quote = _priceRepository.GetQuoteByISINAndDate(isin, tradeDate);

            return quote?.PricePerShare ?? 0;
        }

        public decimal CalculateFundValue(IEnumerable<Investments> investmentType, List<Transactions> allTransactions, DateTime portfolioAsOfDate, Dictionary<string, decimal> sharePrices)
        {
            decimal fundTotalValue = 0;
            var groupedTransactions = allTransactions.GroupBy(t => t.InvestmentId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var item in investmentType)
            {
                var fundValue = _fundValueCalculator.CalculateFundValue(item, portfolioAsOfDate, groupedTransactions, sharePrices);
                fundTotalValue += fundValue;
            }

            return fundTotalValue;
        }

        public List<Investments> GetInvestmentsByFundIds(IEnumerable<Investments> investments)
        {
            var fundInvestors = investments.Where(i => i.InvestmentType == InvestmentType.Funds.ToString()).Select(i => i.FundsInvestor).ToList();

            return _investmentRepository.GetInvestmentsByFundIds(fundInvestors);
        }
    }

}
