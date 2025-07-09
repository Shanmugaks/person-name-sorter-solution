using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;
using Microsoft.Extensions.Logging;

namespace PersonNameSorter.Processors
{
    public class PersonNameSortProcessor : IPersonNameSortProcessor
    {
        private readonly IPersonNameValidator _validator;
        private readonly ISortStrategy _sortStrategy;
        private readonly List<IWriteStrategy> _writeStrategies;
        private readonly ILogger<PersonNameSortProcessor> _logger;

        public PersonNameSortProcessor(
            IPersonNameValidator validator,
            ISortStrategy sortStrategy,
            List<IWriteStrategy> writeStrategies,
            ILogger<PersonNameSortProcessor> logger)
        {
            _validator = validator;
            _sortStrategy = sortStrategy;
            _writeStrategies = writeStrategies;
            _logger = logger;
        }

        public void Process(string inputPath)
        {
            _logger.LogInformation("Starting processing for file: {FilePath}", inputPath);

            try
            {
                var names = ReadFile(inputPath);
                _logger.LogInformation("Successfully read {Count} names from file", names.Count);

                _validator.Validate(names);
                _logger.LogInformation("Validation completed successfully");

                var sorted = _sortStrategy.Sort(names);
                _logger.LogInformation("Sorting completed successfully");

                foreach (var writer in _writeStrategies)
                {
                    writer.Write(sorted);
                    _logger.LogInformation("Output written using {Strategy}", writer.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during processing");
                throw;
            }
        }

        private List<PersonName> ReadFile(string inputPath)
        {
            _logger.LogDebug("Reading names from file: {FilePath}", inputPath);
            var lines = File.ReadAllLines(inputPath);
            List<PersonName> result = new();

            foreach (var line in lines)
            {
                var parts = line.Trim().Split(' ');
                if (parts.Length < 2 || parts.Length > 4)
                {
                    _logger.LogWarning("Skipping invalid line: {Line}", line);
                    continue;
                }

                result.Add(new PersonName
                {
                    GivenNames = new List<string>(parts[..^1]),
                    LastName = parts[^1]
                });
            }

            _logger.LogDebug("Finished parsing file. Valid names count: {Count}", result.Count);
            return result;
        }
    }
}
