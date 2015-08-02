using System;
using System.Collections.Generic;
using System.Linq;
using ScoringEngine.Enums;
using ScoringEngine.Models;

namespace ScoringEngine.Utils
{
    public interface ISalesLeadCalcuationUtils
    {
        double CalculateWeightedValue(EventType type, double value);
        int CalculateNormalizedValue(double min, double max, double value);
        Quartile DetermineQuartile(double value);
    }

    public class SalesLeadCalculationUtils : ISalesLeadCalcuationUtils
    {
        #region class variables

        private readonly Dictionary<EventType, double> _weightingDictionary;
        private readonly List<Quartile> _quartiles;

        #endregion

        #region constructor

        public SalesLeadCalculationUtils()
        {
            _weightingDictionary = PopulateEventTypeDictionary();
            _quartiles = PopulateQuartiles();

        }

        #endregion

        #region public

        public double CalculateWeightedValue(EventType type, double value)
        {
            return _weightingDictionary[type] * value;
        }

        public int CalculateNormalizedValue(double min, double max, double value)
        {
            return (int)Math.Round(((value - min) / (max - min)) * 100);
        }

        public Quartile DetermineQuartile(double value)
        {
            if (value < 0 || value > 100) return null;
            return _quartiles.FirstOrDefault(x => value >= x.Min && value <= x.Max);
        }

        #endregion

        #region private

        private Dictionary<EventType, double> PopulateEventTypeDictionary()
        {
            return new Dictionary<EventType, double>
                {
                    {EventType.Email, 1.2},
                    {EventType.Social, 1.5},
                    {EventType.Web, 1.0},
                    {EventType.Webinar, 2.0}
                };
        }

        private List<Quartile> PopulateQuartiles()
        {
            return new List<Quartile>
            {
                new Quartile
                {
                    Name = "bronze",
                    Min = 0,
                    Max = 24
                },
                new Quartile
                {
                    Name = "silver",
                    Min = 25,
                    Max = 49
                },
                new Quartile
                {
                    Name = "gold",
                    Min = 50,
                    Max = 74
                },
                new Quartile
                {
                    Name = "platinum",
                    Min = 75,
                    Max = 100
                }
            };
        }

        #endregion
    }
}
