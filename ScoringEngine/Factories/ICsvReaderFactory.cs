using ScoringEngine.Parsers;

namespace ScoringEngine.Factories
{
    public interface ICsvReaderFactory
    {
        ICsvParser CreateCsvParser(string filePath);
    }
}
