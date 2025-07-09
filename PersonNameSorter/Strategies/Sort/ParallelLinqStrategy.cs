using System.Collections.Generic;
using System.Linq;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

namespace PersonNameSorter.Strategies.Sort
{
    public class ParallelLinqStrategy : ISortStrategy
    {
        public List<PersonName> Sort(List<PersonName> names)
        {
            return names.
                AsParallel().
                OrderBy(n => n.LastName).
                ThenBy(n => string.Join(" ", n.GivenNames)).
                ToList();
        }
    }
}