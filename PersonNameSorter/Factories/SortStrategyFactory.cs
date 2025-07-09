using PersonNameSorter.Interfaces;
using PersonNameSorter.Strategies.Sort;

/// <summary>
/// Factory class to create appropriate sorting strategy based on the input type.
/// </summary>
/// <remarks>
/// Follows Factory Pattern and Open/Closed Principle for easy extension.
/// </remarks>
namespace PersonNameSorter.Factories
{
    public class SortStrategyFactory : ISortStrategyFactory
    {
        public ISortStrategy Create(SortStrategyType type)
        {
            return type switch
            {
                SortStrategyType.Parallel => new ParallelLinqStrategy(),
                _ => new LinqSortStrategy()
            };
        }
    }
}