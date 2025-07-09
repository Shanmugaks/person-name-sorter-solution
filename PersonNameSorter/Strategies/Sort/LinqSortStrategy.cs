using System.Collections.Generic;
using System.Linq;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

/// <summary>
/// Sorting strategy using different algorithms (LINQ, MergeSort, Parallel LINQ).
/// </summary>
/// <remarks>
/// Implements Strategy Pattern for interchangeable sorting behaviors.
/// </remarks>
namespace PersonNameSorter.Strategies.Sort
{
    public class LinqSortStrategy : ISortStrategy
    {
        public List<PersonName> Sort(List<PersonName> names)
        {
            return names.
                OrderBy(n => n.LastName).
                ThenBy(n => string.Join(" ", n.GivenNames)).
                ToList();
        }
    }
}