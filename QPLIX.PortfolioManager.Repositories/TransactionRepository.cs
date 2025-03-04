using QPLIX.PortfolioManager.Domain.Models;
using QPLIX.PortfolioManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace QPLIX.PortfolioManager.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly List<Transactions> _transactions;

        public TransactionRepository(List<Transactions> transactions)
        {
            _transactions = transactions;
        }

        public List<Transactions> GetTransactionsByInvestmentIdsAndDate(HashSet<string> investmentIds, DateTime portfolioAsOfDate)
        {
            return _transactions.Where(i => investmentIds.Contains(i.InvestmentId) && i.TransactionDate <= portfolioAsOfDate).ToList();
        }
    }
}
