# File-Type-Report

A C# Console Application that generates an HTML report of all files in a given directory (and its subdirectories), grouped by file type and sorted by total size using LINQ and XML.

---

## Features

- Lazy file enumeration with `yield return`
- File grouping and sorting via LINQ
- Human-readable byte size formatting (B, kB, MB, ...)
- Functional HTML report generated with `System.Xml.Linq`
- Excludes system files like `.DS_Store`, `.tmp`, and `.log`

---

## Folder Structure

```
FileTypeReport/
│
├── FileTypeReport.cs         // Main source code
├── FileTypeReport.csproj     // Project file
├── bin/                      // Build output
├── obj/                      // Temporary build files
└── README.md                 
```

---

## How to Run

### 1. **Clone the Repository**

```bash
git clone https://github.com/<your-username>/FileTypeReport.git
cd FileTypeReport
```

### 2. **Create a Test Folder **

```bash
mkdir ~/Desktop/TestFolder
echo "hello world" > ~/Desktop/TestFolder/sample.txt
```
- note: add random files like pdf to the TestFolder, to test

### 3. **Build the Project**

```bash
dotnet build
```

> This ensures the project compiles without errors.

### 4. **Run the Project**

```bash
dotnet run -- "<input-folder>" "<output-report.html>"
```

Example:

```bash
dotnet run -- "/Users/yourname/Desktop/TestFolder" "/Users/yourname/Desktop/report.html"
```

Then open the output:
```bash
open "/Users/yourname/Desktop/report.html"
```
---

## Example Output

The output HTML file contains a table like this:

| Type   | Count | Total Size |
|--------|-------|------------|
| .pdf   | 2     | 300.00 kB  |
| .txt   | 3     | 2.15 kB    |

---

## Tools & Technologies Used

- C# (.NET 9.0)
- Visual Studio Code (Editor)
- Terminal (macOS ZSH)
- System.IO, System.Linq, System.Xml.Linq

---

## Excluded File Types

The following extensions are filtered out from the report:

- `.ds_store`
- `.tmp`
- `.log`
- `.ini`
