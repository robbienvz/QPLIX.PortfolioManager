using QPLIX.PortfolioManager.Domain.Interfaces;
using QPLIX.PortfolioManager.Domain.Models;

namespace QPLIX.PortfolioManager.Repositories
{
    public class RealEstateValueCalculator : IRealEstateValueCalculator
    {
        public decimal CalculateRealEstateValue(IGrouping<string, Investments> investmentType, List<Transactions> allTransactions)
        {
            decimal realEstateValue = 0;

            foreach (var item in investmentType)
            {
                var investmentTransactions = allTransactions.Where(t => t.InvestmentId == item.InvestmentId).ToList();
                realEstateValue += investmentTransactions.Sum(t => t.TransactionValue);
            }

            return realEstateValue;
        }
    }
}
