using System.Collections.Generic;
using PersonNameSorter.Models;

/// <summary>
/// Validates name format ensuring at least one given name and a last name.
/// </summary>
/// <remarks>
/// Encapsulates validation logic (SRP) and enables extensibility (OCP).
/// </remarks>
namespace PersonNameSorter.Interfaces
{
    public interface IPersonNameValidator
    {
        void Validate(List<PersonName> names);
    }
}