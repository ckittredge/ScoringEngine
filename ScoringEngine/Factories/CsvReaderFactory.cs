using ScoringEngine.Parsers;

namespace ScoringEngine.Factories
{
    public class CsvRederFactory : ICsvReaderFactory
    {
        public ICsvParser CreateCsvParser(string filePath)
        {
            return new CsvParser(filePath);
        }
    }
}
