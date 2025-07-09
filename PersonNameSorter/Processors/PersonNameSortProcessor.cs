using System;
using System.Collections.Generic;
using System.IO;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;

namespace PersonNameSorter.Processors
{
    public class PersonNameSortProcessor : IPersonNameSortProcessor
    {
        private readonly IPersonNameValidator _validator;
        private readonly ISortStrategy _sortStrategy;
        private readonly List<IWriteStrategy> _writeStrategies;

        public PersonNameSortProcessor(IPersonNameValidator validator, ISortStrategy sortStrategy, List<IWriteStrategy> writeStrategies)
        {
            _validator = validator;
            _sortStrategy = sortStrategy;
            _writeStrategies = writeStrategies;
        }

        public void Process(string inputPath)
        {
            var names = ReadFile(inputPath);
            _validator.Validate(names);
            var sorted = _sortStrategy.Sort(names);

            foreach (var writer in _writeStrategies)
                writer.Write(sorted);
        }

        private List<PersonName> ReadFile(string inputPath)
        {
            var lines = File.ReadAllLines(inputPath);
            List<PersonName> result = new();
            foreach (var line in lines)
            {
                var parts = line.Trim().Split(' ');
                if (parts.Length < 2) continue;
                result.Add(new PersonName
                {
                    GivenNames = new List<string>(parts[..^1]),
                    LastName = parts[^1]
                });
            }
            return result;
        }
    }
}