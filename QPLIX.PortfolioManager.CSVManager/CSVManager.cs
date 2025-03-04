using CsvHelper.Configuration;
using CsvHelper;
using static QPLIX.PortfolioManager.Common.Enum.FileTypes;

namespace QPLIX.PortfolioManager.CSVManager
{
    public class CSVManager
    {
        private readonly CsvConfiguration _csvConfiguration;

        public CSVManager(CsvConfiguration csvConfiguration)
        {
            _csvConfiguration = csvConfiguration;
        }

        public List<T> ReadCsv<T>(FileType fileType, ClassMap<T> classMap)
        {
            string currentFolder = Directory.GetCurrentDirectory();
            var csvFilePath = GetPath(fileType, currentFolder);
            var results = new List<T>();

            if (File.Exists(csvFilePath))
            {
                try
                {
                    using var reader = new StreamReader(csvFilePath);
                    using var csv = new CsvReader(reader, _csvConfiguration);
                    csv.Context.RegisterClassMap(classMap);

                    results = csv.GetRecords<T>().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while reading the file: {ex.Message}");

                    throw;
                }
            }

            return results;
        }

        private string GetPath(FileType fileType, string currentFolder)
        {
            switch (fileType)
            {
                case FileType.Investments:
                    return currentFolder + "\\SourceFiles\\Investments.csv";
                case FileType.Quotes:
                    return currentFolder + "\\SourceFiles\\Quotes.csv";
                case FileType.Transactions:
                    return currentFolder + "\\SourceFiles\\Transactions.csv";
                default:
                    throw new Exception("Invalid file type");
            }
        }
    }
}
