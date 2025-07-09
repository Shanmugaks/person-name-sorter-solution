using PersonNameSorter.Models;

/// <summary>
/// Sorting strategy using different algorithms (LINQ, MergeSort, Parallel LINQ).
/// </summary>
/// <remarks>
/// Implements Strategy Pattern for interchangeable sorting behaviors.
/// </remarks>
namespace PersonNameSorter.Interfaces
{
    public interface ISortStrategy
    {
        List<PersonName> Sort(List<PersonName> names);
    }
}