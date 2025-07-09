using System.Collections.Generic;

/// <summary>
/// Model class representing a person's name with given names and last name.
/// </summary>
/// <remarks>
/// Follows basic data encapsulation (SRP).
/// </remarks>
namespace PersonNameSorter.Models
{
    public class PersonName
    {
        public List<string> GivenNames { get; set; } = new();
        public string LastName { get; set; } = string.Empty;
        public override string ToString() => string.Join(" ", GivenNames) + " " + LastName;
    }
}