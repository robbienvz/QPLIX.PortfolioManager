using QPLIX.PortfolioManager.Domain.Models;

namespace QPLIX.PortfolioManager.Domain.Repositories
{
    public interface IInvestmentRepository
    {
        List<Investments> GetInvestmentsByFundIds(List<string> fondInvestor);
        List<Investments> GetInvestmentsByInvestorId(string investorId);
        List<Investments> GetInvestmentsForFundValue(string fundName);
    }
}
