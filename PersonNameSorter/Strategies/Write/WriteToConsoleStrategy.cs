using System;
using System.Collections.Generic;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

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