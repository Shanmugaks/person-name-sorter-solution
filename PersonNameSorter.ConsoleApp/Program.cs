using System;
using System.Collections.Generic;
using PersonNameSorter.Factories;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Processors;
using PersonNameSorter.Validators;

namespace PersonNameSorter.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Validate args
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: name-sorter <input-file-path>");
                return;
            }

            string inputFilePath = args[0];

            // Instantiate dependencies
            IPersonNameValidator validator = new PersonNameValidator();
            ISortStrategyFactory sortFactory = new SortStrategyFactory();
            IWriteStrategyFactory writeFactory = new WriteStrategyFactory();

            ISortStrategy sortStrategy = sortFactory.Create("parallel");
            List<IWriteStrategy> writeStrategies = new()
            {
                writeFactory.Create("console"),
                writeFactory.Create("file", "sorted-names-list.txt")
            };

            // Create processor
            IPersonNameSortProcessor processor = new PersonNameSortProcessor(
                validator,
                sortStrategy,
                writeStrategies
            );

            try
            {
                processor.Process(inputFilePath);
                Console.WriteLine("\nSorted names written to screen and to 'sorted-names-list.txt'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
