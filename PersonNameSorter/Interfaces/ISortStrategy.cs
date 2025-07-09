using System.Collections.Generic;
using PersonNameSorter.Models;

namespace PersonNameSorter.Interfaces
{
    public interface ISortStrategy
    {
        List<PersonName> Sort(List<PersonName> names);
    }
}