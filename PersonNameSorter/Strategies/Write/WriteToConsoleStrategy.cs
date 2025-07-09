using System;
using System.Collections.Generic;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

/// <summary>
/// Strategy to output sorted names to the console.
/// </summary>
/// <remarks>
/// Part of Strategy Pattern for output behavior.
/// </remarks>
namespace PersonNameSorter.Strategies.Write
{
    public class WriteToConsoleStrategy : IWriteStrategy
    {
        public void Write(List<PersonName> names)
        {
            foreach (var name in names)
                Console.WriteLine(name.ToString());
        }
    }
}