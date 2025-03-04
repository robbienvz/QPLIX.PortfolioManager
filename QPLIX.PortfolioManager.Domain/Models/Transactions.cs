
namespace QPLIX.PortfolioManager.Domain.Models
{
    public class Transactions
    {
        public string InvestmentId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionValue { get; set; }
    }
}
