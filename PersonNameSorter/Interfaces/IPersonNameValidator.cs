using System.Collections.Generic;
using PersonNameSorter.Models;

namespace PersonNameSorter.Interfaces
{
    public interface IPersonNameValidator
    {
        void Validate(List<PersonName> names);
    }
}