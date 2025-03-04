using CsvHelper.Configuration;
using QPLIX.PortfolioManager.Domain.Models;
using System.Globalization;

namespace QPLIX.PortfolioManager.Domain.Mapper
{
    public class InvestmentMapper : ClassMap<Investments>
    {
        public InvestmentMapper()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.InvestorId).Name("InvestorId");
            Map(m => m.InvestmentId).Name("InvestmentId");
            Map(m => m.InvestmentType).Name("InvestmentType");
            Map(m => m.ISIN).Name("ISIN");
            Map(m => m.City).Name("City");
            Map(m => m.FundsInvestor).Name("FondsInvestor");
        }
    }
}
