using PersonNameSorter.Interfaces;
using PersonNameSorter.Strategies.Write;

namespace PersonNameSorter.Factories
{
    public class WriteStrategyFactory : IWriteStrategyFactory
    {
        public IWriteStrategy Create(string type, string? outputPath = null)
        {
            return type switch
            {
                "console" => new WriteToConsoleStrategy(),
                _ => new WriteToFileStrategy(outputPath ?? "sorted-names-list.txt")
            };
        }
    }
}