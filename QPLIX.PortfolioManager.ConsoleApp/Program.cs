using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QPLIX.PortfolioManager.Domain.Mapper;
using QPLIX.PortfolioManager.Domain.Interfaces;
using QPLIX.PortfolioManager.Domain.Models;
using QPLIX.PortfolioManager.Domain.Repositories;
using QPLIX.PortfolioManager.Domain.Services;
using QPLIX.PortfolioManager.Repositories;
using QPLIX.PortfolioManager.CSVManager;
using static QPLIX.PortfolioManager.Common.Enum.FileTypes;

var csvServiceProvider = new ServiceCollection()
                .AddSingleton<CSVManager>()
                .AddSingleton(new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = true,
                    Mode = CsvMode.NoEscape
                })
               .BuildServiceProvider();

List<Transactions> transactions = null;
List<Investments> investments = null;
List<Prices> prices = null;

try
{
    //Possible to make these IO ooperations async 
    var readerService = csvServiceProvider.GetRequiredService<CSVManager>();
    transactions = readerService.ReadCsv<Transactions>(FileType.Transactions, new TransactionMapper());
    investments = readerService.ReadCsv<Investments>(FileType.Investments, new InvestmentMapper());
    prices = readerService.ReadCsv<Prices>(FileType.Quotes, new QuotesMapper());

    if (transactions == null || investments == null || prices == null)
    {
        Console.WriteLine("Error loading CSV files.");
        Console.ReadLine();
        return;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    Console.ReadLine();
    return;
}

var serviceProvider = new ServiceCollection()
    .AddLogging(logging => logging.AddConsole())
    .AddSingleton<PortfolioService>()
    .AddSingleton<IInvestmentRepository>(new InvestmentRepository(investments))
    .AddSingleton<ITransactionRepository>(new TransactionRepository(transactions))
    .AddSingleton<IPriceRepository>(new PriceRepository(prices))
    .AddSingleton<IStockValueCalculator, StockValueCalculator>()
    .AddSingleton<IRealEstateValueCalculator, RealEstateValueCalculator>()
    .AddSingleton<IFundValueCalculator, FundValueCalculator>()
    .BuildServiceProvider();


var portfolioService = serviceProvider.GetRequiredService<PortfolioService>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

Console.WriteLine("Please enter Investor ID");
var investorId = Console.ReadLine();

Console.WriteLine("Please enter Portfolio date(dddd-mm-yy)");
var date = Console.ReadLine();

if (DateTime.TryParse(date, out DateTime portfolioAsOfDate))
{
    decimal? portfolioValue = 0;

    try
    {
        portfolioValue = portfolioService.CalculatePortfolioValue(investorId, portfolioAsOfDate);
        if (portfolioValue == null)
        {
            logger.LogWarning($"No investments found for investor {investorId}");
            Console.WriteLine($"No investments found for investor {investorId}");
            return;
        }
        Console.WriteLine($"Portfolio Value for {investorId} as of {portfolioAsOfDate.ToString("dd.MM.yyyy")}: {portfolioValue:C}");
    }
    catch (Exception ex)
    {
        logger.LogError($"An error occurred: {ex.Message}");
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}
else
{
    Console.WriteLine("Invalid date format");
}

Console.ReadLine();

