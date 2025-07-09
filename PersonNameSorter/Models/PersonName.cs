using System.Collections.Generic;

namespace PersonNameSorter.Models
{
    public class PersonName
    {
        public List<string> GivenNames { get; set; } = new();
        public string LastName { get; set; } = string.Empty;
        public override string ToString() => string.Join(" ", GivenNames) + " " + LastName;
    }
}