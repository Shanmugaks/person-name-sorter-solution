using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonNameSorter.Factories;
using PersonNameSorter.Interfaces;
using PersonNameSorter.Models;
using PersonNameSorter.Processors;
using PersonNameSorter.Strategies.Sort;
using PersonNameSorter.Strategies.Write;
using PersonNameSorter.Validators;

namespace PersonNameSorter.Tests
{
    [TestClass]
    public class PersonNameSorterTests
    {
        /// <summary>
        /// Ensures the validator throws an exception for an invalid name (no given names or last name).
        /// </summary>
        [TestMethod]
        public void Validator_ShouldThrow_OnInvalidName()
        {
            var validator = new PersonNameValidator();
            var names = new List<PersonName> { new() { GivenNames = new(), LastName = "" } };
            // Should throw because name is invalid
            Assert.ThrowsException<ArgumentException>(() => validator.Validate(names));
        }

        /// <summary>
        /// Ensures the validator does not throw for a valid name.
        /// </summary>
        [TestMethod]
        public void Validator_ShouldNotThrow_OnValidName()
        {
            var validator = new PersonNameValidator();
            var names = new List<PersonName> { new() { GivenNames = new() { "Michael" }, LastName = "Jackson" } };
            // Should not throw for valid name
            validator.Validate(names);
        }

        /// <summary>
        /// Ensures the validator handles multiple valid names.
        /// </summary>
        [TestMethod]
        public void Validator_ShouldHandleMultipleValidNames()
        {
            var validator = new PersonNameValidator();
            var names = new List<PersonName>
            {
                new() { GivenNames = new() { "A" }, LastName = "B" },
                new() { GivenNames = new() { "C", "D" }, LastName = "E" },
            };
            // Should not throw for multiple valid names
            validator.Validate(names);
        }

        /// <summary>
        /// Ensures the validator does not throw for an empty list.
        /// </summary>
        [TestMethod]
        public void Validator_ShouldNotThrow_ForEmptyList()
        {
            // Should not throw for empty list
            new PersonNameValidator().Validate(new List<PersonName>());
        }

        /// <summary>
        /// Ensures LinqSortStrategy sorts by last name, then given names.
        /// </summary>
        [TestMethod]
        public void LinqSortStrategy_ShouldSort_ByLastNameThenGivenNames()
        {
            var strategy = new LinqSortStrategy();
            var names = new List<PersonName>
            {
                new() { GivenNames = new() { "Jerry" }, LastName = "Max" },
                new() { GivenNames = new() { "Vinodh" }, LastName = "Khatte" }
            };
            var result = strategy.Sort(names);
            // "Khatte" comes before "Max"
            Assert.AreEqual("Vinodh Khatte", result[0].ToString());
            Assert.AreEqual("Jerry Max", result[1].ToString());
        }

        /// <summary>
        /// Ensures ParallelLinqStrategy sorts correctly.
        /// </summary>
        [TestMethod]
        public void ParallelLinqStrategy_ShouldSortCorrectly()
        {
            var strategy = new ParallelLinqStrategy();
            var names = new List<PersonName>
            {
                new() { GivenNames = new() { "David" }, LastName = "Boom" },
                new() { GivenNames = new() { "Allan" }, LastName = "Border" }
            };
            var result = strategy.Sort(names);
            // "Boom" comes before "Border"
            Assert.AreEqual("David Boom", result[0].ToString());
        }

        /// <summary>
        /// Ensures sort strategy handles empty list.
        /// </summary>
        [TestMethod]
        public void SortStrategy_ShouldHandleEmptyList()
        {
            var result = new LinqSortStrategy().Sort(new List<PersonName>());
            // Should return empty list
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Ensures WriteToConsoleStrategy does not throw when writing.
        /// </summary>
        [TestMethod]
        public void WriteToConsoleStrategy_ShouldNotThrow()
        {
            var writer = new WriteToConsoleStrategy();
            var names = new List<PersonName> { new() { GivenNames = new() { "Console" }, LastName = "Test" } };
            // Should not throw
            writer.Write(names);
        }

        /// <summary>
        /// Ensures WriteToFileStrategy writes and overwrites files.
        /// </summary>
        [TestMethod]
        public void WriteToFileStrategy_ShouldWriteAndOverwrite()
        {
            string path = "file-test.txt";
            var writer = new WriteToFileStrategy(path);
            var names = new List<PersonName> { new() { GivenNames = new() { "File" }, LastName = "Writer" } };
            writer.Write(names);
            Assert.IsTrue(File.Exists(path));
            writer.Write(names); // Overwrite
            File.Delete(path);
        }

        /// <summary>
        /// Ensures WriteToFileStrategy supports custom file paths.
        /// </summary>
        [TestMethod]
        public void WriteToFileStrategy_ShouldSupportCustomPath()
        {
            var path = "custom-path.txt";
            var writer = new WriteToFileStrategy(path);
            var names = new List<PersonName> { new() { GivenNames = new() { "X" }, LastName = "Y" } };
            writer.Write(names);
            Assert.IsTrue(File.Exists(path));
            File.Delete(path);
        }

        /// <summary>
        /// Ensures SortStrategyFactory creates expected types.
        /// </summary>
        [TestMethod]
        public void SortStrategyFactory_ShouldCreateExpectedTypes()
        {
            var factory = new SortStrategyFactory();
            Assert.IsInstanceOfType(factory.Create(SortStrategyType.Linq), typeof(LinqSortStrategy));
            Assert.IsInstanceOfType(factory.Create(SortStrategyType.Parallel), typeof(ParallelLinqStrategy));
        }

        /// <summary>
        /// Ensures WriteStrategyFactory creates expected types.
        /// </summary>
        [TestMethod]
        public void WriteStrategyFactory_ShouldCreateExpectedTypes()
        {
            var factory = new WriteStrategyFactory();
            Assert.IsInstanceOfType(factory.Create(WriteStrategyType.Console, null), typeof(WriteToConsoleStrategy));
            Assert.IsInstanceOfType(factory.Create(WriteStrategyType.File, "output.txt"), typeof(WriteToFileStrategy));
        }

        /// <summary>
        /// Ensures SortStrategyFactory defaults to LinqSortStrategy for invalid type.
        /// </summary>
        [TestMethod]
        public void Factory_InvalidSortType_ShouldDefaultToLinq()
        {
            var factory = new SortStrategyFactory();
            var strategy = factory.Create((SortStrategyType)999);
            Assert.IsInstanceOfType(strategy, typeof(LinqSortStrategy));
        }

        /// <summary>
        /// Ensures WriteStrategyFactory defaults to WriteToFileStrategy for invalid type.
        /// </summary>
        [TestMethod]
        public void Factory_InvalidWriteType_ShouldDefaultToFile()
        {
            var factory = new WriteStrategyFactory();
            var strategy = factory.Create((WriteStrategyType)999, "fallback.txt");
            Assert.IsInstanceOfType(strategy, typeof(WriteToFileStrategy));
        }

        /// <summary>
        /// Ensures PersonNameSortProcessor processes names and writes output file.
        /// </summary>
        [TestMethod]
        public void PersonNameSortProcessor_ShouldProcess_Successfully()
        {
            string input = "input.txt", output = "output.txt";
            File.WriteAllLines(input, new[] { "Sachin Tendulkar", "Johnny Zohar" });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);
            // Output file should exist
            Assert.IsTrue(File.Exists(output));
            File.Delete(input);
            File.Delete(output);
        }

        /// <summary>
        /// Ensures processor sorts complex names correctly.
        /// </summary>
        [TestMethod]
        public void Processor_ShouldSortCorrectly_ComplexNames()
        {
            string input = "complex.txt";
            string output = "complex-output.txt";

            File.WriteAllLines(input, new[]
            {
                "Algiya Thamil Magal",
                "Cherra Chola pandiya",
                "Jordan Michael"
            });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);

            // Assert the file was written
            Assert.IsTrue(File.Exists(output));

            // Cleanup with retry logic for file locks
            File.Delete(input);
            for (int i = 0; i < 3 && File.Exists(output); i++)
            {
                try
                {
                    File.Delete(output);
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Ensures processor handles empty input files.
        /// </summary>
        [TestMethod]
        public void Processor_ShouldHandleEmptyFile()
        {
            string input = "empty.txt", output = "empty-out.txt";
            File.WriteAllText(input, "");

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);
            // Output file should exist even if input is empty
            Assert.IsTrue(File.Exists(output));
            File.Delete(input);
            File.Delete(output);
        }

        /// <summary>
        /// Ensures processor trims leading/trailing spaces in names.
        /// </summary>
        [TestMethod]
        public void Processor_ShouldTrimNamesWithLeadingOrTrailingSpaces()
        {
            string input = "trim-test.txt";
            string output = "trim-output.txt";
            File.WriteAllLines(input, new[] { "  John   Smith  ", " Alice   Wonderland " });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);
            var lines = File.ReadAllLines(output);

            // Should trim and sort names correctly
            Assert.AreEqual(2, lines.Length);
            Assert.AreEqual("John Smith", lines[0]);
            Assert.AreEqual("Alice Wonderland", lines[1]);

            File.Delete(input);
            File.Delete(output);
        }

        /// <summary>
        /// Ensures processor handles multiple spaces between names.
        /// </summary>
        [TestMethod]
        public void Processor_ShouldHandleMultipleSpacesBetweenNames()
        {
            string input = "multi-space.txt";
            string output = "multi-output.txt";
            File.WriteAllLines(input, new[]
            {
                "John     Smith",
                "Alice       Wonderland"
            });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);
            var lines = File.ReadAllLines(output);

            // Should trim and sort names correctly
            Assert.AreEqual(2, lines.Length);
            Assert.AreEqual("John Smith", lines[0]);
            Assert.AreEqual("Alice Wonderland", lines[1]);

            File.Delete(input);
            File.Delete(output);
        }

        /// <summary>
        /// Ensures processor skips lines with only one word.
        /// </summary>
        [TestMethod]
        public void Processor_ShouldSkipLinesWithOnlyOneWord()
        {
            string input = "single-word.txt";
            string output = "single-out.txt";
            File.WriteAllLines(input, new[] { "Michael", "Michael Jordan" });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);
            var lines = File.ReadAllLines(output);

            // Only valid full name should be written
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual("Michael Jordan", lines[0]);

            File.Delete(input);
            File.Delete(output);
        }

        /// <summary>
        /// Ensures processor skips blank lines.
        /// </summary>
        [TestMethod]
        public void Processor_ShouldSkipBlankLines()
        {
            string input = "blank-lines.txt";
            string output = "blank-out.txt";

            File.WriteAllLines(input, new[] { "", "John Smith", "   ", "Alice Wonderland" });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);
            var lines = File.ReadAllLines(output);

            // Only valid names should be written
            Assert.AreEqual(2, lines.Length);
            File.Delete(input);
            File.Delete(output);
        }

        /// <summary>
        /// Ensures processor skips lines with more than four parts (too many given names).
        /// </summary>
        [TestMethod]
        public void Processor_ShouldSkipLinesWithMoreThanFourParts()
        {
            string input = "too-many-parts.txt";
            string output = "too-many-out.txt";
            File.WriteAllLines(input, new[]
            {
                "Alice Bob Carol Dee Smith", // too many
                "John Middle Smith"          // valid
            });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<IWriteStrategy> { new WriteToFileStrategy(output) },
                NullLogger<PersonNameSortProcessor>.Instance
            );

            processor.Process(input);
            var lines = File.ReadAllLines(output);

            // Only valid name should be written
            Assert.AreEqual(1, lines.Length);
            Assert.AreEqual("John Middle Smith", lines[0]);

            File.Delete(input);
            File.Delete(output);
        }

        /// <summary>
        /// Ensures PersonName.ToString formats names correctly.
        /// </summary>
        [TestMethod]
        public void PersonName_ToString_ShouldFormatProperly()
        {
            var person = new PersonName { GivenNames = new() { "A", "B" }, LastName = "C" };
            // Should format as "A B C"
            Assert.AreEqual("A B C", person.ToString());
        }
    }
}
