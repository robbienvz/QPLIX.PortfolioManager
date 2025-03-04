using QPLIX.PortfolioManager.Domain.Models;
using QPLIX.PortfolioManager.Domain.Repositories;

namespace QPLIX.PortfolioManager.Repositories
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly List<Investments> _investments;

        public InvestmentRepository(List<Investments> investments)
        {
            _investments = investments;
        }

        public List<Investments> GetInvestmentsByInvestorId(string investorId)
        {
            return _investments.Where(i => i.InvestorId == investorId).ToList();
        }

        public List<Investments> GetInvestmentsForFundValue(string fundName)
        {
            return _investments.Where(i => i.InvestorId == fundName).ToList();
        }

        public List<Investments> GetInvestmentsByFundIds(List<string> fonds)
        {
            return _investments.Where(i => fonds.Contains(i.InvestorId)).ToList();
        }
    }
}
