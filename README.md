Hello, 

I'm Shan. This solution and documentation were created with care and a focus on continuous improvement. While not perfect, it's built to grow and improve over time.

👉 UML diagrams are available in the /docs folder at the project root.

# PersonNameSorter

**PersonNameSorter** is a robust and extensible .NET console application designed to efficiently sort lists of names. It reads unsorted names from a file, validates them, sorts them by last name (then by given names), and outputs the results to various formats (Console and file). Built with a modular architecture, PersonNameSorter emphasizes clean code, testability, and extensibility, making it easy to adapt to new requirements.

## 🛠️ Single Command to Build, Test & Run

The **`build-test-run.sh`** script is your all-in-one utility to:

- Restore dependencies  
- Build the project  
- Run unit tests  
- Execute the application with your selected dataset

```bash
sh ./build-test-run.sh [size]
```

### Optional Size Arguments

| Argument | Description               | Input File                             |
|----------|---------------------------|-----------------------------------------|
| (none)   | Default input file        | `./data/unsorted-names-list.txt`        |
| 2K       | 2,000 names               | `./data/unsorted-names-list-2K.txt`     |
| 5K       | 5,000 names               | `./data/unsorted-names-list-5K.txt`     |
| 10K      | 10,000 names              | `./data/unsorted-names-list-10K.txt`    |
| 100K     | 100,000 names             | `./data/unsorted-names-list-100K.txt`   |
| 1M       | 1 million names           | `./data/unsorted-names-list-1M.txt`     |

### Example Commands

```bash
sh ./build-test-run.sh         # Uses default input file
sh ./build-test-run.sh 5K      # Uses 5,000 names input
sh ./build-test-run.sh 1M      # Uses 1 million names input
```

Make sure the script is executable:
> ```bash
> chmod +x build-test-run.sh
> ```

## 📤 Output

- Console output of sorted names
- File output: `sorted-names-list.txt`
- Logs with Serilog: `PersonNameSorterYYYYMMDD.txt`


## ✅ Testing

Run tests independently:

```bash
dotnet test
```

### Test Coverage

- ✔️ Name validation (missing last names, blank inputs)
- ✔️ Sorting strategies (LINQ, Parallel LINQ)
- ✔️ Output strategies (Console, File)
- ✔️ Full pipeline processor test cases

<p align="center">
  <img src="/docs/test_result.png" alt="MS test result" width="100%">
</p>


## ✨ Features

PersonNameSorter offers a powerful set of features designed for flexibility and reliability:

* **Intelligent Sorting:** Sorts names primarily by last name, then by given names for precise ordering.
* **Flexible Input:** Currently supports plain text input files, with easy extensibility for other formats.
* **Efficient & Scalable Sorting:** Employs simple LINQ-based sorting for smaller datasets and parallel-enabled LINQ sorting for larger files.
* **Modular Architecture:** DLL-based design with Strategy and Factory patterns for extensibility.
* **Clean and Testable Code:** Adheres to best practices for maintainability and ease of testing.
* **Comprehensive Logging:** Integrates Serilog for robust logging.
* **Automated Testing**: Unit tested using MSTest and FluentAssertions.

## 🛠️ Design and Structure

### Architectural Overview
Below diagram illustrates the high-level components and their interactions within the application.
<p align="center">
  <img src="/docs/overview.png" alt="Architecture Diagram" width="600">
</p>

### Key Relationships
Below diagram provides a detailed view of how different classes interact, highlighting the dependencies and collaborations that form the application's core logic.
<p align="center">
  <img src="/docs/class_realtionship_diagram.png" alt="Class Relationship Diagram" width="100%">
</p>

### Execution Flow
Below **Sequence Diagram** visualizes the dynamic behavior of the application, showing the order of operations and interactions between objects when processing names.
<p align="center">
  <img src="/docs/sequence_diagram.png" alt="Sequence Diagram" width="100%">
</p>

### Project Structure

```
📁 PersonNameSorter.Solution/
├── 📁 PersonNameSorter/                    → Core Sorting Library (DLL)
│ ├── 📁 Interfaces/                        → Contracts for validator, sorter, and writer
│ ├── 📁 Models/                            → PersonName data model
│ ├── 📁 Processors/                        → Orchestrates the sorting pipeline
│ ├── 📁 Strategies/
│ │ ├── 📁 Sort/                            → Sorting strategies (Linq, MergeSort, PLINQ)
│ │ └── 📁 Write/                           → Output strategies (TXT, CSV, JSON)
│ ├── 📁 Factories/                         → Factory classes for dynamic strategy resolution
│ ├── 📁 Extensions/                        → DI container extensions
│ └── 🧿 PersonNameSorter.csproj            → Project file for core library
│
├── 📁 PersonNameSorter.Console/            → Console application (EXE)
│ ├── 📄 Program.cs                         → Main entry point for running the sorter
│ └── 🧿 PersonNameSorter.Console.csproj    → Project file for console app
│
├── 📁 PersonNameSorter.UnitTest/           → Unit test project using MSTest
│ ├── 📄 PersonNameSorterTests.cs           → Tests for validation, sorting, and output logic
│ └── 🧿 PersonNameSorter.UnitTest.csproj   → Project file for unit tests
│
├── ⚙️ .gitignore                           → Specifies ignored files for Git
├── 🔄 .travis.yml                          → Travis CI/CD pipeline
├── 🔧 build-test-run.sh                    → Single shell script to restore, build, test and run
├── 🐳 Dockerfile                           → Docker config for local container deployment
├── 🧿 PersonNameSorter.Solution.sln        → Visual Studio solution file linking all projects
└── 📘 README.md                            → Documentation for this whole solution
├── 📁 data/                                → all sample datasets - 11(default), 2K, 5K, 10K, 100K, 1M
├── 📁 docs/                                → all images + UML diagrams (generated using plantUML) for the readme.md
```


## 🔄 CI/CD Integration

Supports build automation on local and cloud environments:

- **.travis.yml**: Travis CI configuration for automated build/test
- **build-test-run.sh**: Runs entire flow with one command
- **Dockerfile**: Containerized execution on any platform

These tools ensure consistent quality, test automation, and seamless build validation across environments.



## 📋 Strategy Options

### Sorting Strategies

| Strategy Type | Description |
|---------------|-------------|
| LINQ          | Basic sequential sorting |
| Parallel      | Optimized for parallel sorting using PLINQ |
| MergeSort     | (Future) Custom merge sort for big data sets - TO DO|

### Writing Strategies

| Strategy Type | Output Format |
|---------------|---------------|
| Console       | Outputs to console |
| File          | Writes to `sorted-names-list.txt` (default) |



## 🧠 Future Improvements

- Support for CSV and JSON input/output
- CLI arguments for choosing strategy types
- Integration with a web front-end + Api
- Multi-language support
- Implementation of Merge sort for large data