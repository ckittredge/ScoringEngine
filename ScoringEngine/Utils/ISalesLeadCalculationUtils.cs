using ScoringEngine.Enums;
using ScoringEngine.Models;

namespace ScoringEngine.Utils
{
    public interface ISalesLeadCalcuationUtils
    {
        double CalculateWeightedValue(EventType type, double value);
        int CalculateNormalizedValue(double min, double max, double value);
        Quartile DetermineQuartile(int value);
    }
}
