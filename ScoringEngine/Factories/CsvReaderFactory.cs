using System.Diagnostics.CodeAnalysis;
using ScoringEngine.Parsers;

namespace ScoringEngine.Factories
{
    [ExcludeFromCodeCoverage]
    public class CsvRederFactory : ICsvReaderFactory
    {
        public ICsvParser CreateCsvParser(string filePath)
        {
            return new CsvParser(filePath);
        }
    }
}
