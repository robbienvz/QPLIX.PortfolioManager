﻿
namespace QPLIX.PortfolioManager.Domain.Models
{
    public class Investments
    {
        public string InvestorId { get; set; }
        public string InvestmentId { get; set; }
        public string InvestmentType { get; set; }
        public string ISIN { get; set; }
        public string City { get; set; }
        public string FundsInvestor { get; set; }

    }
}
