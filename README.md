# TtrxToHtml

Transforms Visual Studio test results (.trx) files into a more readable HTML report.

## Purpose
TtrxToHtml converts one or more .trx test result files (Visual Studio test results) into a single, human-friendly HTML report. It:
- Combines multiple .trx files when a directory is provided.
- Renders test results using an embedded Razor template to produce a self-contained HTML report.
- Opens the generated report automatically after creation.

## Requirements
- .NET SDK (6.0 or later recommended)
- Windows / macOS / Linux (dotnet is cross-platform; Process.Start is used to open the output file)

## Quick start — build and run
1. Clone the repository:
   - git clone https://github.com/shailesh1984/TtrxToHtml.git

2. Build:
   - dotnet build

3. Run (examples):
   - Convert a single .trx file:
     - dotnet run --project src/TtrxToHtml.Console -- -f /path/to/results.trx
   - Convert all .trx files in a directory:
     - dotnet run --project src/TtrxToHtml.Console -- -d /path/to/trx-directory
   - Specify output HTML file name:
     - dotnet run --project src/TtrxToHtml.Console -- -f /path/to/results.trx -hfn MyTestReport
   - Show help / info:
     - dotnet run --project src/TtrxToHtml.Console -- --help
     - dotnet run --project src/TtrxToHtml.Console -- --info

Notes:
- The `--` after `dotnet run --project ...` separates dotnet CLI args from the application args.
- If you prefer, you can publish and run the produced executable:
  - dotnet publish -c Release -r <RID> --self-contained false
  - ./bin/Release/net*/<RID>/publish/TtrxToHtml.Console <app-args>

## appsettings.json (defaults)
appsettings.json controls file extensions, template name and output folder. Example:
```json
{
  "ProjectName": "TtrxToHtml",
  "AppSettings": {
    "TrxFileExt": ".trx",
    "OutputFileExt": ".html",
    "HtmlReportDirectoryFolder": "Test_Reports",
    "DateTimeFormat": "dd-M-yyyy-HH-mm-ss",
    "CshtmlFileName": "Index",
    "TestReportFileName": "TestReportFile_"
  }
}
```
- TrxFileExt: extension expected for input files
- OutputFileExt: extension for the generated HTML
- HtmlReportDirectoryFolder: output directory created under the input path
- DateTimeFormat and TestReportFileName are used to produce unique output file names
- CshtmlFileName: name of the embedded Razor template used to render HTML

## Command-line options
- -f, --tfp     : Trx file path (single .trx file)
- -d, --tdp     : Trx directory path (all .trx files in directory will be processed)
- -hfn, --html-file-name : Optional HTML file base name (overrides default TestReportFileName)
- --info        : Print project info
- -h, --help    : Print help

(See src/TtrxToHtml.Console/Helpers/CommandLineInterfaceHelper.cs for argument parsing and validation.)

## Output
By default, a Test_Reports folder (configurable via appsettings.json) will be created in the same directory as the input .trx (or trx directory), and a timestamped HTML file will be created inside it. The application attempts to open the generated report after creation.

## Project structure (important files)
- README.md
- src/
  - TtrxToHtml.Console/
    - Program.cs
    - appsettings.json
    - Helpers/
      - CommandLineInterfaceHelper.cs
    - Service/
      - GenerateTrxReportService.cs
    - Interfaces/
      - IGenerateTrxReportService.cs
    - Templates/
      - (embedded Razor templates, e.g. Index.cshtml)
    - .editorconfig
- tests/
  - TtrxToHtml.Test/
    - GenerateTrxReportServiceTest.cs

## Testing
A unit test for report generation exists:
- tests/TtrxToHtml.Test/GenerateTrxReportServiceTest.cs
Run tests with:
- dotnet test

## Implementation notes
- The project uses RazorLight to render an embedded Razor template into HTML.
- Multiple .trx files will be combined into a single JSON representation before rendering.
- Logging is provided via Microsoft.Extensions.Logging (console logger configured).

## Contribution
Contributions and improvements are welcome. Please open issues or pull requests.

## License
(If you have a license, add it here. If not, add one before public distribution.)
