using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonNameSorter.Extensions;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Processors;
using Serilog;

namespace PersonNameSorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ensure the input file path is provided by the user
            if (args.Length == 0)
            {
                Console.WriteLine("USAGE: name-sorter <input-file-path>");
                return;
            }

            string inputPath = args[0];

            try
            {
                // Configure dependency injection container with logging, validators, factories, etc.
                var serviceProvider = new ServiceCollection()
                    .AddPersonNameSorter() // Extension method to register all required services
                    .BuildServiceProvider();

                // Resolve dependencies from the service provider
                var logger = serviceProvider.GetRequiredService<ILogger<PersonNameSortProcessor>>();
                var validator = serviceProvider.GetRequiredService<IPersonNameValidator>();
                var sortStrategyFactory = serviceProvider.GetRequiredService<ISortStrategyFactory>();
                var writeStrategyFactory = serviceProvider.GetRequiredService<IWriteStrategyFactory>();

                // Create sorting strategy (Linq by default, could be configurable)
                var sortStrategy = sortStrategyFactory.Create(SortStrategyType.Linq);

                // Create list of write strategies (console + file output)
                var writeStrategies = new List<IWriteStrategy>
                {
                    writeStrategyFactory.Create(WriteStrategyType.Console),
                    writeStrategyFactory.Create(WriteStrategyType.File, "sorted-names-list.txt")
                };

                // Create processor and trigger the processing logic
                var processor = new PersonNameSortProcessor(validator, sortStrategy, writeStrategies, logger);
                processor.Process(inputPath);
            }
            catch (Exception ex)
            {
                // Log any unhandled errors during processing
                Log.Error(ex, "An error occurred in main while processing names.");
            }
            finally
            {
                // Ensure all log resources are properly flushed
                Log.CloseAndFlush();
            }
        }
    }
}
