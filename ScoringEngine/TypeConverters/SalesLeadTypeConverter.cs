using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ScoringEngine.CustomExceptions;
using ScoringEngine.Parsers;
using ScoringEngine.Enums;
using ScoringEngine.Factories;
using ScoringEngine.Models;
using ScoringEngine.Utils;

namespace ScoringEngine.TypeConverters
{
    public class SalesLeadTypeConverter : ISalesLeadTypeConverter
    {
        #region class variables

        private readonly IEnumConverter<EventType> _eventTypeConverter;
        private readonly ISalesLeadCalcuationUtils _salesLeadCalculationUtils;
        private readonly ICsvReaderFactory _csvReaderFactory;

        #endregion class variables

        #region constructor

        public SalesLeadTypeConverter(IEnumConverter<EventType> eventTypeConverter,
            ISalesLeadCalcuationUtils salesLeadCalcuationUtils,
            ICsvReaderFactory csvReaderFactory)
        {
            _eventTypeConverter = eventTypeConverter;
            _salesLeadCalculationUtils = salesLeadCalcuationUtils;
            _csvReaderFactory = csvReaderFactory;
        }

        #endregion constructor

        #region public

        public List<SalesLead> ConvertToType(string filePath)
        {
            List<SalesLead> salesLeads = new List<SalesLead>();

            using (ICsvParser csvParser = _csvReaderFactory.CreateCsvParser(filePath))
            {
                Row row = new Row();
                int rowNumber = 1;
                while (csvParser.ReadRow(row))
                {
                    try
                    {
                        AddUpdateSalesLead(ref salesLeads, row, rowNumber);
                        rowNumber++;
                    }
                    catch (InvalidRowInputException invalidRowException)
                    {
                        if (invalidRowException.InvalidColumns.Count > 0)
                        {
                            PrintInvalidRowMessage(invalidRowException, rowNumber);
                        }
                        else
                        {
                            Console.WriteLine(invalidRowException.Message);
                        }
                        return null;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return null;
                    }
                }
            }

            salesLeads.ForEach(sl => sl.EventScoreSum = sl.EventScores.Sum(es => es.WeightedScore));
            double min = salesLeads.Min(x => x.EventScoreSum);
            double max = salesLeads.Max(x => x.EventScoreSum);

            foreach (SalesLead sl in salesLeads)
            {
                sl.EventScoreNormalized = _salesLeadCalculationUtils.CalculateNormalizedValue(min, max, sl.EventScoreSum);
                sl.ContactQuartile = _salesLeadCalculationUtils.DetermineQuartile(sl.EventScoreNormalized);
            }

            return salesLeads.OrderBy(x => x.ContactId).ToList();
        }

        #endregion public

        #region Private

        private void AddUpdateSalesLead(ref List<SalesLead> salesLeads, Row row, int rowNumber)
        {
            SalesLead salesLead = new SalesLead();
            if (row.Columns.Count == 3)
            {
                int contactId;
                bool contactIdValid = int.TryParse(row.Columns[0], out contactId);
                EventType eventType;
                bool eventTypeValid = _eventTypeConverter.TryParseString(row.Columns[1], out eventType);
                double score;
                bool scoreValid = double.TryParse(row.Columns[2], out score);
                if (!contactIdValid || !eventTypeValid || !scoreValid)
                {
                    List<int> invalidColumns = new List<int>(3);
                    if (!contactIdValid) invalidColumns.Add(1);
                    if (!eventTypeValid) invalidColumns.Add(2);
                    if (!scoreValid) invalidColumns.Add(3);
                    throw new InvalidRowInputException(invalidColumns);
                }
                SalesLead existing = salesLeads.FirstOrDefault(sl => sl.ContactId == contactId);
                if (existing == null)
                {
                    salesLead.ContactId = contactId;
                    salesLead.EventScores = new List<EventScore>
                    {
                        new EventScore
                        {
                            Type = eventType,
                            Score = score,
                            WeightedScore = _salesLeadCalculationUtils.CalculateWeightedValue(eventType, score)
                        }
                    };
                    salesLeads.Add(salesLead);
                }
                else
                {
                    existing.EventScores.Add(new EventScore
                    {
                        Type = eventType,
                        Score = score,
                        WeightedScore = _salesLeadCalculationUtils.CalculateWeightedValue(eventType, score)
                    });
                }


            }
            else
            {
                throw new InvalidRowInputException("Invalid number of columns for row " + rowNumber + ". " +
                                                   "Each row must contain comma separated columns for " +
                                                   "contactId, event, and score");
            }
        }

        private void PrintInvalidRowMessage(InvalidRowInputException invalidRowInputException, int rowNumber)
        {
            if (invalidRowInputException.InvalidColumns.Count == 1)
            {
                Console.WriteLine("Invalid input on row {0} in column {1}", rowNumber, invalidRowInputException.InvalidColumns[0]);
                return;
            }
            StringBuilder invalidColumnsString = new StringBuilder();
            for (int i = 0; i < invalidRowInputException.InvalidColumns.Count; i++)
            {
                if (i == invalidRowInputException.InvalidColumns.Count - 1)
                {
                    invalidColumnsString.Append(" and " + invalidRowInputException.InvalidColumns[i]);
                }
                else
                {
                    invalidColumnsString.Append(invalidRowInputException.InvalidColumns[i] + ", ");
                }
                Console.WriteLine("Invalid input on row {0} for columns {1}", rowNumber, invalidColumnsString);
            }
        }

        #endregion Private
    }
}
