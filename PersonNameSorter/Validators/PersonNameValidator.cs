using System;
using System.Collections.Generic;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

/// <summary>
/// Validates name format ensuring at least one given name and a last name.
/// </summary>
/// <remarks>
/// Encapsulates validation logic (SRP) and enables extensibility (OCP).
/// </remarks>
namespace PersonNameSorter.Validators
{
    public class PersonNameValidator : IPersonNameValidator
    {
        public void Validate(List<PersonName> names)
        {
            foreach (var name in names)
            {
                if (string.IsNullOrWhiteSpace(name.LastName) || name.GivenNames.Count == 0)
                    throw new ArgumentException("Invalid name format.");
            }
        }
    }
}