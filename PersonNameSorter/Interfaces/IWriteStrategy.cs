using System.Collections.Generic;
using PersonNameSorter.Models;

namespace PersonNameSorter.Interfaces
{
    public interface IWriteStrategy
    {
        void Write(List<PersonName> names);
    }
}