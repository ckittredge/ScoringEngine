using System;
using System.Collections.Generic;

namespace ScoringEngine.Models
{
    public class SalesLead
    {
        #region properties

        public int ContactId { get; set; }
        public List<EventScore> EventScores { get; set; }
        public double EventScoreSum { get; set; }
        public int EventScoreNormalized { get; set; }
        public Quartile ContactQuartile { get; set; }

        #endregion properties

        #region public

        public void Print()
        {
            Console.WriteLine("{0}, {1}, {2}", ContactId, ContactQuartile.Name, EventScoreNormalized);
        }

        public void PrintDetailed()
        {
            Console.WriteLine("ContactId: {0}", ContactId);
            Console.WriteLine("EventScores:");
            EventScores
                .ForEach(x =>
                    Console.WriteLine("Type: {0}, Score: {1}, Weighted Score: {2}".PadLeft(30),
                    x.Type, x.Score, x.WeightedScore));
            Console.WriteLine("Event Score Sum: {0}", EventScoreSum);
            Console.WriteLine("Event Score Normalized: {0}", EventScoreNormalized);
            Console.WriteLine("Quartile: {0}", ContactQuartile.Name);
            Console.WriteLine();
        }

        #endregion public
    }

}
