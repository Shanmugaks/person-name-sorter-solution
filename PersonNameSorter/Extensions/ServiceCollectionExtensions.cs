
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonNameSorter.Factories;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Validators;
using Serilog;

namespace PersonNameSorter.Extensions
{
    /// <summary>
    /// Registers all PersonNameSorter dependencies and configures Serilog.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        
        public const string DEFAULT_LOG_FILE = "PersonNameSorter.txt";
        public static IServiceCollection AddPersonNameSorter(
            this IServiceCollection services,
            string logFilePath = DEFAULT_LOG_FILE,
            LoggerConfiguration? loggerConfig = null)
        {
            Log.Logger = (loggerConfig ?? new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day))
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            });

            services.AddSingleton<IPersonNameValidator, PersonNameValidator>();
            services.AddSingleton<ISortStrategyFactory, SortStrategyFactory>();
            services.AddSingleton<IWriteStrategyFactory, WriteStrategyFactory>();

            return services;
        }
    }
}
