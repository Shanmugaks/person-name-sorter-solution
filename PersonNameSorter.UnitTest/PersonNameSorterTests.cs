using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using PersonNameSorter.Models;
using PersonNameSorter.Validators;
using PersonNameSorter.Strategies.Sort;
using PersonNameSorter.Strategies.Write;
using PersonNameSorter.Factories;
using PersonNameSorter.Processors;

namespace PersonNameSorter.Tests
{
    [TestClass]
    public class PersonNameSorterTests
    {
        [TestMethod]
        public void PersonNameValidator_ShouldThrow_OnInvalidName()
        {
            var validator = new PersonNameValidator();
            var names = new List<PersonName> { new PersonName { GivenNames = new(), LastName = "" } };
            Assert.ThrowsException<System.ArgumentException>(() => validator.Validate(names));
        }

        [TestMethod]
        public void PersonNameValidator_ShouldNotThrow_OnValidName()
        {
            var validator = new PersonNameValidator();
            var names = new List<PersonName> { new PersonName { GivenNames = new() { "Alice" }, LastName = "Smith" } };
            validator.Validate(names);
        }

        [TestMethod]
        public void LinqSortStrategy_ShouldSort_ByLastNameThenGivenNames()
        {
            var strategy = new LinqSortStrategy();
            var names = new List<PersonName>
            {
                new() { GivenNames = new() { "John" }, LastName = "Zebra" },
                new() { GivenNames = new() { "Alice" }, LastName = "Apple" }
            };
            var result = strategy.Sort(names);
            Assert.AreEqual("Alice Apple", result[0].ToString());
            Assert.AreEqual("John Zebra", result[1].ToString());
        }

        [TestMethod]
        public void MergeSortStrategy_ShouldSortCorrectly()
        {
            var strategy = new MergeSortStrategy();
            var names = new List<PersonName>
            {
                new() { GivenNames = new() { "Bob" }, LastName = "Taylor" },
                new() { GivenNames = new() { "Anna" }, LastName = "Brown" }
            };
            var result = strategy.Sort(names);
            Assert.AreEqual("Anna Brown", result[0].ToString());
        }

        [TestMethod]
        public void ParallelLinqStrategy_ShouldSortCorrectly()
        {
            var strategy = new ParallelLinqStrategy();
            var names = new List<PersonName>
            {
                new() { GivenNames = new() { "David" }, LastName = "Young" },
                new() { GivenNames = new() { "Carol" }, LastName = "Anderson" }
            };
            var result = strategy.Sort(names);
            Assert.AreEqual("Carol Anderson", result[0].ToString());
        }

        [TestMethod]
        public void WriteToConsoleStrategy_ShouldNotThrow()
        {
            var writer = new WriteToConsoleStrategy();
            var names = new List<PersonName> { new() { GivenNames = new() { "Console" }, LastName = "Test" } };
            writer.Write(names);
        }

        [TestMethod]
        public void WriteToFileStrategy_ShouldWriteAndOverwrite()
        {
            string path = "file-test.txt";
            var writer = new WriteToFileStrategy(path);
            var names = new List<PersonName> { new() { GivenNames = new() { "File" }, LastName = "Writer" } };
            writer.Write(names);
            Assert.IsTrue(File.Exists(path));
            writer.Write(names);
            File.Delete(path);
        }

        [TestMethod]
        public void SortStrategyFactory_ShouldCreateExpectedTypes()
        {
            var factory = new SortStrategyFactory();
            Assert.IsInstanceOfType(factory.Create("linq"), typeof(LinqSortStrategy));
            Assert.IsInstanceOfType(factory.Create("merge"), typeof(MergeSortStrategy));
            Assert.IsInstanceOfType(factory.Create("parallel"), typeof(ParallelLinqStrategy));
        }

        [TestMethod]
        public void WriteStrategyFactory_ShouldCreateExpectedTypes()
        {
            var factory = new WriteStrategyFactory();
            Assert.IsInstanceOfType(factory.Create("console"), typeof(WriteToConsoleStrategy));
            Assert.IsInstanceOfType(factory.Create("file", "output.txt"), typeof(WriteToFileStrategy));
        }

        [TestMethod]
        public void PersonNameSortProcessor_ShouldProcess_Successfully()
        {
            string input = "input.txt", output = "output.txt";
            File.WriteAllLines(input, new[] { "Anna Smith", "John Zebra" });

            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<PersonNameSorter.Interfaces.IWriteStrategy>
                {
                    new WriteToFileStrategy(output)
                }
            );

            processor.Process(input);
            Assert.IsTrue(File.Exists(output));
            File.Delete(input);
            File.Delete(output);
        }

        [TestMethod]
        public void Processor_ShouldSortCorrectly_ComplexNames()
        {
            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new MergeSortStrategy(),
                new List<PersonNameSorter.Interfaces.IWriteStrategy>
                {
                    new WriteToConsoleStrategy()
                }
            );
            string path = "complex.txt";
            File.WriteAllLines(path, new[] {
                "Anna Marie Brown",
                "John David Smith",
                "Anna Brown"
            });

            processor.Process(path);
            File.Delete(path);
        }

        [TestMethod]
        public void Validator_ShouldHandleMultipleValidNames()
        {
            var validator = new PersonNameValidator();
            var names = new List<PersonName>
            {
                new() { GivenNames = new() { "A" }, LastName = "B" },
                new() { GivenNames = new() { "C", "D" }, LastName = "E" },
            };
            validator.Validate(names);
        }

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

        [TestMethod]
        public void Factory_InvalidSortType_ShouldDefaultToLinq()
        {
            var factory = new SortStrategyFactory();
            var strategy = factory.Create("invalid-type");
            Assert.IsInstanceOfType(strategy, typeof(LinqSortStrategy));
        }

        [TestMethod]
        public void Factory_InvalidWriteType_ShouldDefaultToFile()
        {
            var factory = new WriteStrategyFactory();
            var strategy = factory.Create("invalid-type", "fallback.txt");
            Assert.IsInstanceOfType(strategy, typeof(WriteToFileStrategy));
        }

        [TestMethod]
        public void PersonName_ToString_ShouldFormatProperly()
        {
            var person = new PersonName { GivenNames = new() { "A", "B" }, LastName = "C" };
            Assert.AreEqual("A B C", person.ToString());
        }

        [TestMethod]
        public void SortStrategy_ShouldHandleEmptyList()
        {
            var strategy = new LinqSortStrategy();
            var result = strategy.Sort(new List<PersonName>());
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Validator_ShouldNotThrow_ForEmptyList()
        {
            var validator = new PersonNameValidator();
            validator.Validate(new List<PersonName>());
        }

        [TestMethod]
        public void Processor_ShouldHandleEmptyFile()
        {
            string path = "empty.txt";
            File.WriteAllText(path, "");
            var processor = new PersonNameSortProcessor(
                new PersonNameValidator(),
                new LinqSortStrategy(),
                new List<PersonNameSorter.Interfaces.IWriteStrategy>
                {
                    new WriteToFileStrategy("empty-out.txt")
                });
            processor.Process(path);
            Assert.IsTrue(File.Exists("empty-out.txt"));
            File.Delete(path);
            File.Delete("empty-out.txt");
        }
    }
}