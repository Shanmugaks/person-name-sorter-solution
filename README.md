## 🧩 Architecture Diagram

<p align="center">
  <img src="/docs/overview.png" alt="Architecture Diagram" width="600">
</p>


## 🧩 Class Relationship Diagram

<p align="center">
  <img src="/docs/sequence_diagram.png" alt="Architecture Diagram" width="600">
</p>

## 🧩 Architecture Diagram

<p align="center">
  <img src="/docs/class_realtionship_diagram.png" alt="Architecture Diagram" width="600">
</p>


# PersonNameSorter

**PersonNameSorter** is a modular, extensible .NET console application that reads a list of unsorted names from a file, validates and sorts them by last name (and then given names), and writes the results to various output formats (console and file).

## ✨ Features

- Sorts names by **last name**, then **given names**
- Supports multiple input formats (plain text currently)
- Modular architecture with:
  - Strategy Pattern for sorting and writing
  - Validator for name structure
  - Factory for strategy instantiation
- Clean and testable code
- Serilog-based logging (file output)
- Unit tested using MSTest and FluentAssertions

---

## 🛠️ Setup

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- IDE like Visual Studio 2022  or VS code (or just a terminal)

---

## 🚀 How to Run

### Build the project

```bash
dotnet build
```

### Run the console application

```bash
dotnet run --project PersonNameSorter.ConsoleApp <input-file-path>
```

> Example:

```bash
dotnet run --project PersonNameSorter.ConsoleApp .\data\unsorted-names-list.txt
```

### Output

- Sorted names will be written to:
  - Console output
  - A file named `sorted-names-list.txt` in the same directory

---

## ✅ How to Test

```bash
dotnet test
```

### Test Coverage Includes:

- Name validation rules (e.g., required last name, allowed name count)
- Sorting strategies (LINQ, Parallel LINQ)
- Output strategies (console, file)
- End-to-end processor scenarios (e.g., trimming, blank lines, malformed inputs)

---

## 📁 Project Structure

```
PersonNameSorter/
├── Interfaces/           # Core interfaces for validator, sorter, writer
├── Models/               # PersonName model
├── Processors/           # Core logic for reading, validating, sorting, writing
├── Strategies/           # Sorting and output strategy implementations
├── Factories/            # Factory pattern for dynamic strategy selection
├── Extensions/           # DI setup
├── Program.cs            # Entry point
├── PersonNameSorter.csproj
└── README.md
```

---

## 📋 Strategy Options

### Sorting Strategies

| Strategy Type | Description               |
|---------------|---------------------------|
| LINQ          | Basic sequential sorting  |
| Parallel      | Optimized for parallel sorting using PLINQ |
| MergeSort     | (Future) Custom merge sort for big data sets - TODO|

### Writing Strategies

| Strategy Type | Output Format |
|---------------|---------------|
| Console       | Outputs to console |
| File          | Writes to `sorted-names-list.txt` (default) |

---

## 📌 Notes

- Logging is configured using **Serilog**. Logs are written to `PersonNameSorter.txt` by default.
- Invalid lines (e.g., empty lines, too many or too few name parts) are gracefully skipped.
- You can extend this to support CSV or JSON input/output with minimal changes due to its pluggable design.



---

## 🧠 Future Enhancements

- Support for CSV and JSON input/output
- CLI arguments for choosing strategy types
- Integration with a web front-end + Api
- Multi-language support
- Implementation of Merge sort for large data

---

## 👨‍💻 Author

**Shan KS**  
