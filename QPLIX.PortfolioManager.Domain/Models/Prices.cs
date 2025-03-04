
namespace QPLIX.PortfolioManager.Domain.Models
{
    public class Prices
    {
        public string ISIN { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal PricePerShare { get; set; }
    }
}
