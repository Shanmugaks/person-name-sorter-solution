using PersonNameSorter.Interfaces;
using PersonNameSorter.Strategies.Sort;

namespace PersonNameSorter.Factories
{
    public class SortStrategyFactory : ISortStrategyFactory
    {
        public ISortStrategy Create(string type)
        {
            return type switch
            {
                "parallel" => new ParallelLinqStrategy(),
                "merge" => new MergeSortStrategy(),
                _ => new LinqSortStrategy()
            };
        }
    }
}