using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;
using Microsoft.Extensions.Logging;

/// <summary>
/// Core processor class that orchestrates reading, validating, sorting, and writing of names.
/// Acts as the Template Method, coordinating multiple strategies.
/// Demonstrates Single Responsibility and Dependency Injection.
/// </summary>
namespace PersonNameSorter.Processors
{
    public class PersonNameSortProcessor : IPersonNameSortProcessor
    {
        private readonly IPersonNameValidator _validator;
        private readonly ISortStrategy _sortStrategy;
        private readonly List<IWriteStrategy> _writeStrategies;
        private readonly ILogger<PersonNameSortProcessor> _logger;


        /// <summary>
        /// Initializes a new instance of the class with dependencies injected.
        /// </summary>
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

        /// <summary>
        /// Processes the input file containing names.
        /// Reads names, validates them, sorts them using the specified strategy,
        /// and writes the sorted names using all provided write strategies (console and file).
        /// Implements the Template Method pattern to define the skeleton of the processing algorithm.
        /// </summary>
        public void Process(string inputPath)
        {
            _logger.LogInformation("Starting processing for file: {FilePath}", inputPath);

            try
            {
                // Step-1: Read names from file
                var names = ReadFile(inputPath);
                _logger.LogInformation("Successfully read {Count} names from file", names.Count);

                // Step-2: Validate names
                _validator.Validate(names);
                _logger.LogInformation("Validation completed successfully");

                // Step-3: Sort names using the provided strategy
                var sorted = _sortStrategy.Sort(names);
                _logger.LogInformation("Sorting completed successfully");

                // Step-4: Write sorted names using all provided strategies (console and file)
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

        /// <summary>
        /// Reads names from the specified input file and parses them into a list of PersonName objects.
        /// Each line in the file should contain a person's name with given names and a last name.
        /// </summary>
        private List<PersonName> ReadFile(string inputPath)
        {
            _logger.LogDebug("Reading names from file: {FilePath}", inputPath);
            var lines = File.ReadAllLines(inputPath);
            List<PersonName> result = new();

            foreach (var line in lines)
            {
                var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Skip empty lines or lines with invalid format
                if (parts.Length == 0)
                {
                    _logger.LogWarning("Skipping empty line");
                    continue;
                }

                // Ensure there are at least 2 parts (given names and last name) and at most 4 parts (3 given names and 1 last name)
                if (parts.Length < 2 || parts.Length > 4)
                {
                    _logger.LogWarning("Skipping invalid line: {Line}", line);
                    continue;
                }
                // Create PersonName object with given names and last name
                // Last part is considered the last name, all others are given names
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