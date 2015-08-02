using System;
using ScoringEngine.Models;

namespace ScoringEngine.Parsers
{
    public interface ICsvParser : IDisposable
    {
        bool ReadRow(Row row);
    }
}
