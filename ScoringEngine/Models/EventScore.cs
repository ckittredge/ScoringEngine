using ScoringEngine.Enums;

namespace ScoringEngine.Models
{
    public class EventScore
    {
        public EventType Type { get; set; }
        public double Score { get; set; }
        public double WeightedScore { get; set; }
    }
}
