using System.Collections.Generic;
using PersonNameSorter.Models;

/// <summary>
/// Interface or utility involved in name sorting system.
/// </summary>
/// <remarks>
/// Defines contracts supporting DIP and Strategy pattern.
/// </remarks>
namespace PersonNameSorter.Interfaces
{
    public interface IWriteStrategy
    {
        void Write(List<PersonName> names);
    }
}