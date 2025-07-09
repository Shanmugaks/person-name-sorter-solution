using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonNameSorter.Factories;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Processors;
using PersonNameSorter.Strategies.Sort;
using PersonNameSorter.Strategies.Write;
using PersonNameSorter.Validators;
using Serilog;

namespace PersonNameSorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: name-sorter <input-file-path>");
                return;
            }

            string inputPath = args[0];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("PersonNameSorter.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddSerilog();
                    })
                    .AddSingleton<IPersonNameValidator, PersonNameValidator>()
                    .AddSingleton<ISortStrategyFactory, SortStrategyFactory>()
                    .AddSingleton<IWriteStrategyFactory, WriteStrategyFactory>()
                    .BuildServiceProvider();

                var logger = serviceProvider.GetRequiredService<ILogger<PersonNameSortProcessor>>();
                var validator = serviceProvider.GetRequiredService<IPersonNameValidator>();
                var sortStrategy = new LinqSortStrategy();
                var writeStrategies = new List<IWriteStrategy>
                {
                    new WriteToConsoleStrategy(),
                    new WriteToFileStrategy("sorted-names-list.txt")
                };

                var processor = new PersonNameSortProcessor(validator, sortStrategy, writeStrategies, logger);
                processor.Process(inputPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing names.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}