using System;
using System.Collections.Generic;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

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