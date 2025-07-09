using System.Collections.Generic;
using System.IO;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;
using Serilog;

namespace PersonNameSorter.Strategies.Write
{
    public class WriteToFileStrategy : IWriteStrategy
    {
        private readonly string _filePath;
        public WriteToFileStrategy(string filePath = "sorted-names-list.txt")
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
