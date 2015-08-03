using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ScoringEngine.Enums;
using ScoringEngine.Factories;
using ScoringEngine.TypeConverters;
using ScoringEngine.Utils;

namespace ScoringEngine
{
    [ExcludeFromCodeCoverage]
    class ScoringEngineMain
    {
        static void Main(string[] args)
        {
            SalesLeadTypeConverter salesLeadTypeConverter = new SalesLeadTypeConverter(new EventTypeConverter(),
                new SalesLeadCalculationUtils(), new CsvRederFactory());
            FileTypeUtils fileTypeUtils = new FileTypeUtils();
            var shouldContinue = true;
            do
            {
                Console.Write("Please provide a file path for a sales lead csv file:");
                var filePath = Console.ReadLine();
                try
                {
                    if (!fileTypeUtils.ContainsCsvExtension(filePath))
                    {
                        Console.WriteLine("Invalid file extension. Please ensure that you enter a path to a valid csv file.");
                    }
                    else
                    {
                        var salesLeadList = salesLeadTypeConverter.ConvertToType(filePath);
                        if (salesLeadList != null) salesLeadList.ForEach(x => x.Print());
                    }
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
                Console.Write("Continue? (Y/N): ");
                string response = Console.ReadLine();
                while (response == null ||
                      (response.Trim().ToLowerInvariant() != "y" && response.Trim().ToLowerInvariant() != "n"))
                {
                    Console.Write("Invalid response. Continue? (Y/N): ");
                    response = Console.ReadLine();
                }
                shouldContinue = response.Trim().ToLowerInvariant() == "y";
            } while (shouldContinue);
        }
    }
}
