using CsvHelper.Configuration;
using QPLIX.PortfolioManager.Domain.Models;
using System.Globalization;

namespace QPLIX.PortfolioManager.Domain.Mapper
{
    public class TransactionMapper : ClassMap<Transactions>
    {
        public TransactionMapper()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.InvestmentId).Name("InvestmentId");
            Map(m => m.TransactionType).Name("Type");
            Map(m => m.TransactionDate).Name("Date").TypeConverterOption.Format("yyyy-MM-dd");
            Map(m => m.TransactionValue).Name("Value").TypeConverterOption.Format("0.000000");
        }
    }
}
