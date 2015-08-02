using System;
using System.IO;
using ScoringEngine.Enums;
using ScoringEngine.Factories;
using ScoringEngine.TypeConverters;
using ScoringEngine.Utils;

namespace ScoringEngine
{
    class ScoringEngineMain
    {
        static void Main(string[] args)
        {
            SalesLeadTypeConverter salesLeadTypeConverter = new SalesLeadTypeConverter(new EventTypeConverter(),
                new SalesLeadCalculationUtils(), new CsvRederFactory());

            Console.Write("Please provide a file path for a sales lead csv file:");
            var filePath = Console.ReadLine();
            try
            {
                var salesLeadList = salesLeadTypeConverter.ConvertToType(filePath);
                if (salesLeadList != null) salesLeadList.ForEach(x => x.Print());
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine("The file {0} was not found. Please make sure you have entered a valid file path.",
                    fileNotFoundException.FileName);
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine("{0} Please make sure you have entered a valid file path.", argumentException.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
