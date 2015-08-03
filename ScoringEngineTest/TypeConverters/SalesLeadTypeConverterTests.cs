using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ScoringEngine.Enums;
using ScoringEngine.Factories;
using ScoringEngine.Models;
using ScoringEngine.Parsers;
using ScoringEngine.TypeConverters;
using ScoringEngine.Utils;
using ScoringEngineTest.UnitTestUtils;

namespace ScoringEngineTest.TypeConverters
{
    [TestFixture]
    public class SalesLeadTypeConverterTests
    {
        private IEnumConverter<EventType> _eventTypeConverter;
        private ISalesLeadCalcuationUtils _salesLeadCalculationUtils;
        private ICsvReaderFactory _csvReaderFactory;
        private ICsvParser _csvParser;
        private ISalesLeadTypeConverter _salesLeadTypeConverter;

        [SetUp]
        public void Setup()
        {
            _eventTypeConverter = Substitute.For<IEnumConverter<EventType>>();
            _salesLeadCalculationUtils = Substitute.For<ISalesLeadCalcuationUtils>();
            _csvReaderFactory = Substitute.For<ICsvReaderFactory>();
            _csvParser = Substitute.For<ICsvParser>();
            _salesLeadTypeConverter = new SalesLeadTypeConverter(_eventTypeConverter,
                _salesLeadCalculationUtils, _csvReaderFactory);
        }

        [Test]
        public void ConvertToType()
        {
            const string testFilePath = "C:\testFile.csv";
            var readRowResults = new Results<bool>(true)
                .Then(true)
                .Then(true)
                .Then(false);
            var columnResults = new Results<List<string>>(new List<string> { "1", "Email", "150.2" })
                .Then(new List<string> { "2", "Email", "150.2" })
                .Then(new List<string> { "3", "Email", "150.2" });
            EventType et1;
            _eventTypeConverter.TryParseString("Email", out et1).Returns(x =>
            {
                x[1] = EventType.Email;
                return true;
            });
            _csvParser.ReadRow(null).ReturnsForAnyArgs(x =>
            {
                var hasNext = readRowResults.Next();
                if (!hasNext) return false;
                ((Row)x[0]).Columns = columnResults.Next();
                return true;
            });
            _salesLeadCalculationUtils.CalculateWeightedValue(EventType.Email, 0.0).ReturnsForAnyArgs(100);
            _csvReaderFactory.CreateCsvParser(testFilePath).ReturnsForAnyArgs(_csvParser);
            _salesLeadCalculationUtils.CalculateNormalizedValue(0, 0, 0).ReturnsForAnyArgs(100);
            Quartile testQuartile = new Quartile
            {
                Max = 100,
                Min = 75,
                Name = "Platinum"
            };
            _salesLeadCalculationUtils.DetermineQuartile(0).ReturnsForAnyArgs(testQuartile);
            List<SalesLead> expectedResult = new List<SalesLead>
            {
                new SalesLead
                {
                    ContactId = 1,
                    EventScores = new List<EventScore>
                    {
                        new EventScore
                        {
                            Score = 150.2,
                            Type = EventType.Email,
                            WeightedScore = 100
                        }
                    },
                    EventScoreSum = 100,
                    EventScoreNormalized = 100,
                    ContactQuartile = testQuartile
                },
                new SalesLead
                {
                    ContactId = 2,
                    EventScores = new List<EventScore>
                    {
                        new EventScore
                        {
                            Score = 150.2,
                            Type = EventType.Email,
                            WeightedScore = 100
                        }
                    },
                    EventScoreSum = 100,
                    EventScoreNormalized = 100,
                    ContactQuartile = testQuartile
                },
                new SalesLead
                {
                    ContactId = 3,
                    EventScores = new List<EventScore>
                    {
                        new EventScore
                        {
                            Score = 150.2,
                            Type = EventType.Email,
                            WeightedScore = 100
                        }
                    },
                    EventScoreSum = 100,
                    EventScoreNormalized = 100,
                    ContactQuartile = testQuartile
                }
            };
            var result = _salesLeadTypeConverter.ConvertToType(testFilePath);
            result.Count.ShouldBeEquivalentTo(3);
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void ConvertToTypeShouldAppendEventScoresToContact1()
        {
            const string testFilePath = "C:\testFile.csv";
            var readRowResults = new Results<bool>(true)
                .Then(true)
                .Then(true)
                .Then(false);
            var columnResults = new Results<List<string>>(new List<string> { "1", "Email", "150.2" })
                .Then(new List<string> { "1", "Email", "150.2" })
                .Then(new List<string> { "1", "Email", "150.2" });
            EventType et1;
            _eventTypeConverter.TryParseString("Email", out et1).Returns(x =>
            {
                x[1] = EventType.Email;
                return true;
            });
            _csvParser.ReadRow(null).ReturnsForAnyArgs(x =>
            {
                var hasNext = readRowResults.Next();
                if (!hasNext) return false;
                ((Row)x[0]).Columns = columnResults.Next();
                return true;
            });
            _salesLeadCalculationUtils.CalculateWeightedValue(EventType.Email, 0.0).ReturnsForAnyArgs(100);
            _csvReaderFactory.CreateCsvParser(testFilePath).ReturnsForAnyArgs(_csvParser);
            var result = _salesLeadTypeConverter.ConvertToType(testFilePath);
            result.Count.ShouldBeEquivalentTo(1);
            var salesLead = result.FirstOrDefault(x => x.ContactId == 1);
            Assert.NotNull(salesLead);
            salesLead.EventScores.Count.ShouldBeEquivalentTo(3);
        }

        [Test]
        public void ConvertToTypeShouldReturnNullForInvalidContactIdInput()
        {
            const string testFilePath = "C:\testFile.csv";
            var readRowResults = new Results<bool>(true)
                .Then(false);
            var columnResults = new Results<List<string>>(new List<string> { "NonInteger", "Email", "150.2" });
            EventType et1;
            _eventTypeConverter.TryParseString("Email", out et1).Returns(x =>
            {
                x[1] = EventType.Email;
                return true;
            });
            _csvParser.ReadRow(null).ReturnsForAnyArgs(x =>
            {
                var hasNext = readRowResults.Next();
                if (!hasNext) return false;
                ((Row)x[0]).Columns = columnResults.Next();
                return true;
            });
            _salesLeadCalculationUtils.CalculateWeightedValue(EventType.Email, 0.0).ReturnsForAnyArgs(100);
            _csvReaderFactory.CreateCsvParser(testFilePath).ReturnsForAnyArgs(_csvParser);
            var result = _salesLeadTypeConverter.ConvertToType(testFilePath);
            Assert.Null(result);
        }

        [Test]
        public void ConvertToTypeShouldReturnNullForInvalidEventTypeInput()
        {
            const string testFilePath = "C:\testFile.csv";
            var readRowResults = new Results<bool>(true)
                .Then(false);
            var columnResults = new Results<List<string>>(new List<string> { "1", "InvalidEventType", "150.2" });
            EventType et1;
            _eventTypeConverter.TryParseString("InvalidEventType", out et1).Returns(x =>
            {
                x[1] = EventType.Invalid;
                return false;
            });
            _csvParser.ReadRow(null).ReturnsForAnyArgs(x =>
            {
                var hasNext = readRowResults.Next();
                if (!hasNext) return false;
                ((Row)x[0]).Columns = columnResults.Next();
                return true;
            });
            _salesLeadCalculationUtils.CalculateWeightedValue(EventType.Email, 0.0).ReturnsForAnyArgs(100);
            _csvReaderFactory.CreateCsvParser(testFilePath).ReturnsForAnyArgs(_csvParser);
            var result = _salesLeadTypeConverter.ConvertToType(testFilePath);
            Assert.Null(result);
        }

        [Test]
        public void ConvertToTypeShouldReturnNullForMultipleInvalidInputs()
        {
            const string testFilePath = "C:\testFile.csv";
            var readRowResults = new Results<bool>(true)
                .Then(false);
            var columnResults = new Results<List<string>>(new List<string> { "NonInteger", "InvalidEventType", "NonDouble" });
            EventType et1;
            _eventTypeConverter.TryParseString("InvalidEventType", out et1).Returns(x =>
            {
                x[1] = EventType.Invalid;
                return false;
            });
            _csvParser.ReadRow(null).ReturnsForAnyArgs(x =>
            {
                var hasNext = readRowResults.Next();
                if (!hasNext) return false;
                ((Row)x[0]).Columns = columnResults.Next();
                return true;
            });
            _salesLeadCalculationUtils.CalculateWeightedValue(EventType.Email, 0.0).ReturnsForAnyArgs(100);
            _csvReaderFactory.CreateCsvParser(testFilePath).ReturnsForAnyArgs(_csvParser);
            var result = _salesLeadTypeConverter.ConvertToType(testFilePath);
            Assert.Null(result);
        }

        [Test]
        public void ConvertToTypeShouldReturnNullForInvalidScoreInput()
        {
            const string testFilePath = "C:\testFile.csv";
            var readRowResults = new Results<bool>(true)
                .Then(false);
            var columnResults = new Results<List<string>>(new List<string> { "1", "Email", "NonDouble" });
            EventType et1;
            _eventTypeConverter.TryParseString("Email", out et1).Returns(x =>
            {
                x[1] = EventType.Email;
                return true;
            });
            _csvParser.ReadRow(null).ReturnsForAnyArgs(x =>
            {
                var hasNext = readRowResults.Next();
                if (!hasNext) return false;
                ((Row)x[0]).Columns = columnResults.Next();
                return true;
            });
            _salesLeadCalculationUtils.CalculateWeightedValue(EventType.Email, 0.0).ReturnsForAnyArgs(100);
            _csvReaderFactory.CreateCsvParser(testFilePath).ReturnsForAnyArgs(_csvParser);
            var result = _salesLeadTypeConverter.ConvertToType(testFilePath);
            Assert.Null(result);
        }

        [Test]
        public void ConvertToTypeShouldReturnNullForInvalidNumberOfColumns()
        {
            const string testFilePath = "C:\testFile.csv";
            var readRowResults = new Results<bool>(true)
                .Then(false);
            var columnResults = new Results<List<string>>(new List<string> { "1", "Email" });
            _csvParser.ReadRow(null).ReturnsForAnyArgs(x =>
            {
                var hasNext = readRowResults.Next();
                if (!hasNext) return false;
                ((Row)x[0]).Columns = columnResults.Next();
                return true;
            });
            _salesLeadCalculationUtils.CalculateWeightedValue(EventType.Email, 0.0).ReturnsForAnyArgs(100);
            _csvReaderFactory.CreateCsvParser(testFilePath).ReturnsForAnyArgs(_csvParser);
            _salesLeadTypeConverter.ConvertToType(testFilePath).ShouldAllBeEquivalentTo(null);

            readRowResults = new Results<bool>(true)
                .Then(false);
            columnResults = new Results<List<string>>(new List<string> { "1", "Email", "150,1" });
            _salesLeadTypeConverter.ConvertToType(testFilePath).ShouldAllBeEquivalentTo(null);
        }

    }
}