using System.Collections.Generic;
using System.IO;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;
using Serilog;

/// <summary>
/// Strategy to output sorted names to a file.
/// </summary>
/// <remarks>
/// Applies Strategy Pattern with file-based output.
/// </remarks>
namespace PersonNameSorter.Strategies.Write
{
    public class WriteToFileStrategy : IWriteStrategy
    {
        private readonly string _filePath;        
        public WriteToFileStrategy(string filePath = IWriteStrategyFactory.DEFAULT_OUTPUT_FILE)
        {   
            _filePath = filePath;
        }

        public void Write(List<PersonName> names)
        {
            File.WriteAllLines(_filePath, names.ConvertAll(n => n.ToString()));
            Log.Information("Names written to file: {FilePath}", _filePath);
        }
    }
}
