/*
CECS 342-05 Assignment 2
Deanna Solis
Hannah Ngyuen 
David Delatorre
Carlos Ponce 

Tools, Compilers, and Editors Used:
- editor: VScode
- compiler/runtime: .NET

Online references:
- https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby?view=net-9.0
- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/let-clause
- https://stackoverflow.com/questions/3008085/recursively-getting-files-in-a-directory-with-several-sub-directories
- chatgpt and stackoverflow for debugging
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FileTypeReport {
  internal static class Program {
    // 1. Enumerate all files in a folder recursively
    private static IEnumerable<string> EnumerateFilesRecursively(string path) {
      foreach (string filePath in Directory.EnumerateFiles(path))
      {
        yield return filePath; //  yields each file in the current directory one by one.
      }
      foreach (string subFilePath in Directory.EnumerateDirectories(path)) // files in the sub dir
      {
        foreach (string subFile in EnumerateFilesRecursively(subFilePath)) // recursive call the same function to get files from that subdir
        {
          yield return subFile; // yields files one by one
        }
        
      }
    }

    // Human readable byte size
    private static string FormatByteSize(long byteSize) {
      string[] units = { "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB" };
      double size = byteSize;
      int unitIndex = 0;

      while (size >= 1000 && unitIndex < units.Length - 1) {
        size /= 1000;
        unitIndex++;
        }

      return $"{size:0.##} {units[unitIndex]}";
    }

    // Create an HTML report file
    private static XDocument CreateReport(IEnumerable<string> files) {
      var excludedExtensions = new HashSet<string> { ".ds_store", ".tmp", ".log", ".ini" };
      // 2. Process data
      var query =
        from file in files
        // TODO: Fill in your code here.
        let info = new FileInfo(file)
        where !excludedExtensions.Contains(info.Extension.ToLower())
        group info by info.Extension.ToLower() into fileGroup
        orderby fileGroup.Sum(f => f.Length) descending
        select new {
          Type = string.IsNullOrWhiteSpace(fileGroup.Key) ? "(none)" : fileGroup.Key, // TODO: Fill in your code here.
          Count = fileGroup.Count(),    // TODO: Fill in your code here.
          TotalSize = fileGroup.Sum(f => f.Length) // TODO: Fill in your code here.
        };

      // 3. Functionally construct XML
      var alignment = new XAttribute("align", "right");
      var style = "table, th, td { border: 1px solid black; }";

      var tableRows = query.Select(entry =>
        new XElement("tr",
          new XElement("td", entry.Type),
          new XElement("td", alignment, entry.Count),
          new XElement("td", alignment, FormatByteSize(entry.TotalSize))
        )
      );

        
      var table = new XElement("table",
        new XElement("thead",
          new XElement("tr",
            new XElement("th", "Type"),
            new XElement("th", "Count"),
            new XElement("th", "Total Size"))),
        new XElement("tbody", tableRows));

      return new XDocument(
        new XDocumentType("html", null, null, null),
          new XElement("html",
            new XElement("head",
              new XElement("title", "File Report"),
              new XElement("style", style)),
            new XElement("body", table)));
    }

    // Console application with two arguments
    public static void Main(string[] args) {
      try {
        string inputFolder = args[0];
        string reportFile  = args[1];
        CreateReport(EnumerateFilesRecursively(inputFolder)).Save(reportFile);
      } catch {
        Console.WriteLine("Usage: FileTypeReport <folder> <report file>");
      }
    }
  }
}