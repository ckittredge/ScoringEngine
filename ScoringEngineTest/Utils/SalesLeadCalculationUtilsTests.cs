using FluentAssertions;
using NUnit.Framework;
using ScoringEngine.Enums;
using ScoringEngine.Models;
using ScoringEngine.Utils;

namespace ScoringEngineTest.Utils
{
    [TestFixture]
    public class SalesLeadCalculationUtilsTests
    {
        private ISalesLeadCalcuationUtils _salesLeadCalcuationUtils;


        [SetUp]
        public void Setup()
        {
            _salesLeadCalcuationUtils = new SalesLeadCalculationUtils();
        }

        [Test]
        public void CalulateWeightedValuesShouldCalculateWeightedValuesAsDoubles()
        {
            _salesLeadCalcuationUtils.CalculateWeightedValue(EventType.Email, 5.5).ShouldBeEquivalentTo(6.6);
            _salesLeadCalcuationUtils.CalculateWeightedValue(EventType.Social, 5.5).ShouldBeEquivalentTo(8.25);
            _salesLeadCalcuationUtils.CalculateWeightedValue(EventType.Web, 5.5).ShouldBeEquivalentTo(5.5);
            _salesLeadCalcuationUtils.CalculateWeightedValue(EventType.Webinar, 5.5).ShouldBeEquivalentTo(11.0);
        }

        [Test]
        public void CalculateNormalizedValueShouldCalculateNormalizedValuesAsInts()
        {
            _salesLeadCalcuationUtils.CalculateNormalizedValue(17.42, 154.23, 27.25).ShouldBeEquivalentTo(7);
            _salesLeadCalcuationUtils.CalculateNormalizedValue(954.12, 5513.19, 1841.13).ShouldBeEquivalentTo(19);
            _salesLeadCalcuationUtils.CalculateNormalizedValue(17.42, 154.23, 17.42).ShouldBeEquivalentTo(0);
            _salesLeadCalcuationUtils.CalculateNormalizedValue(17.42, 154.23, 154.23).ShouldBeEquivalentTo(100);
        }

        [Test]
        public void DetermineQuartileShouldDetermineCorrectQuartileByValue()
        {
            Quartile bronzeQuartile = new Quartile
                {
                    Name = "bronze",
                    Min = 0,
                    Max = 24
                };
            Quartile silverQuartile = new Quartile
                {
                    Name = "silver",
                    Min = 25,
                    Max = 49
                };
            Quartile goldQuartile = new Quartile
                {
                    Name = "gold",
                    Min = 50,
                    Max = 74
                };
            Quartile platinumQuartile = new Quartile
                {
                    Name = "platinum",
                    Min = 75,
                    Max = 100
                };

            _salesLeadCalcuationUtils.DetermineQuartile(0).ShouldBeEquivalentTo(bronzeQuartile);
            _salesLeadCalcuationUtils.DetermineQuartile(24).ShouldBeEquivalentTo(bronzeQuartile);
            _salesLeadCalcuationUtils.DetermineQuartile(25).ShouldBeEquivalentTo(silverQuartile);
            _salesLeadCalcuationUtils.DetermineQuartile(49).ShouldBeEquivalentTo(silverQuartile);
            _salesLeadCalcuationUtils.DetermineQuartile(50).ShouldBeEquivalentTo(goldQuartile);
            _salesLeadCalcuationUtils.DetermineQuartile(74).ShouldBeEquivalentTo(goldQuartile);
            _salesLeadCalcuationUtils.DetermineQuartile(75).ShouldBeEquivalentTo(platinumQuartile);
            _salesLeadCalcuationUtils.DetermineQuartile(100).ShouldBeEquivalentTo(platinumQuartile);
        }
    }
}