using PersonNameSorter.Factories;


/// <summary>
/// Sorting strategy using different algorithms (LINQ, Parallel LINQ).
/// </summary>
/// <remarks>
/// Implements Strategy Pattern for interchangeable sorting behaviors.
/// </remarks>
namespace PersonNameSorter.Interfaces
{
    public enum SortStrategyType
    {
        Linq,
        Parallel
    }
    public interface ISortStrategyFactory
    {
        ISortStrategy Create(SortStrategyType type);
    }
}