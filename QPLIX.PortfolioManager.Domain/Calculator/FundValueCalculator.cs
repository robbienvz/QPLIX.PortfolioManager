using QPLIX.PortfolioManager.Common;
using QPLIX.PortfolioManager.Domain.Interfaces;
using QPLIX.PortfolioManager.Domain.Models;
using QPLIX.PortfolioManager.Domain.Repositories;

namespace QPLIX.PortfolioManager.Repositories
{
    public class FundValueCalculator : IFundValueCalculator
    {
        private readonly IInvestmentRepository _investmentRepository;

        public FundValueCalculator(IInvestmentRepository investmentRepository)
        {
            _investmentRepository = investmentRepository;
        }

        public decimal CalculateFundValue(Investments investments, DateTime referenceDate, Dictionary<string, List<Transactions>> groupedTransactions,
             Dictionary<string, decimal> sharePrices)
        {
            decimal fundValue = 0;
            decimal investedFundValue = 0;

            var containedInvestments = _investmentRepository.GetInvestmentsForFundValue(investments.FundsInvestor);
            var containedInvestmentIds = containedInvestments.Select(i => i.InvestmentId).ToList();

            foreach (var containedItem in containedInvestments)
            {
                if (groupedTransactions.TryGetValue(containedItem.InvestmentId, out var containedTransactions))
                {
                    fundValue += CalculateInvestmentValue(containedItem, containedTransactions, sharePrices);
                }
            }

            if (groupedTransactions.TryGetValue(investments.InvestmentId, out var transactionsForInvestor))
            {
                var investedPercentage = transactionsForInvestor.Sum(t => t.TransactionValue);
                investedFundValue = fundValue * investedPercentage;
            }

            return investedFundValue;
        }

        public decimal CalculateInvestmentValue(Investments investment, List<Transactions> transactions, Dictionary<string, decimal> sharePrices)
        {
            decimal investmentValue = 0;

            switch (investment.InvestmentType)
            {
                case InvestmentType.Stock:
                    investmentValue += CalculateStockInvestmentValue(investment, transactions, sharePrices);
                    break;

                case InvestmentType.RealEstate:
                    investmentValue += CalculateRealEstateInvestmentValue(transactions);
                    break;
            }

            return investmentValue;
        }

        public decimal CalculateStockInvestmentValue(Investments investment, List<Transactions> transactions, Dictionary<string, decimal> sharePrices)
        {
            decimal totalUnits = transactions.Sum(t => t.TransactionValue);
            decimal sharePrice = sharePrices.GetValueOrDefault(investment.ISIN, 0);

            if (totalUnits == 0 || sharePrice == 0)
            {
                return 0;
            }

            return totalUnits * sharePrice;
        }

        public decimal CalculateRealEstateInvestmentValue(List<Transactions> transactions)
        {
            return transactions.Sum(t => t.TransactionValue);
        }
    }
}
