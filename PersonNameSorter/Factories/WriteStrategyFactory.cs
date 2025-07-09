using PersonNameSorter.Interfaces;
using PersonNameSorter.Strategies.Write;

/// <summary>
/// Factory class to create appropriate writing strategy (console or file).
/// </summary>
/// <remarks>
/// Follows Factory Pattern and Open/Closed Principle for easy extension.
/// </remarks>
namespace PersonNameSorter.Factories
{
    public class WriteStrategyFactory : IWriteStrategyFactory
    {
        public IWriteStrategy Create(WriteStrategyType type, string? outputPath)
        {
            return type switch
            {
                WriteStrategyType.Console => new WriteToConsoleStrategy(),
                _ => new WriteToFileStrategy(outputPath ?? IWriteStrategyFactory.DEFAULT_OUTPUT_FILE)
            };
        }
    }
}