using PersonNameSorter.Strategies.Write;


/// <summary>
/// Interface or utility involved in name sorting system.
/// </summary>
/// <remarks>
/// Defines contracts supporting DIP and Strategy pattern.
/// </remarks>
namespace PersonNameSorter.Interfaces
{
    public enum WriteStrategyType
    {
        Console,
        File
    }
    
    public interface IWriteStrategyFactory
    {
        public const string DEFAULT_OUTPUT_FILE = "sorted-names-list.txt";
        IWriteStrategy Create(WriteStrategyType type, string? outputPath = null);
    }
}