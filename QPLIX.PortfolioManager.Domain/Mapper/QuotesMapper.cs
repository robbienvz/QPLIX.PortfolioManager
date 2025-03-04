using CsvHelper.Configuration;
using QPLIX.PortfolioManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPLIX.PortfolioManager.Domain.Mapper
{
    public class QuotesMapper: ClassMap<Prices>
    {
        public QuotesMapper()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.ISIN).Name("ISIN");
            Map(m => m.TransactionDate).Name("Date").TypeConverterOption.Format("yyyy-MM-dd");
            Map(m => m.PricePerShare).Name("PricePerShare").TypeConverterOption.Format("0.000000");
        }
    }
}
