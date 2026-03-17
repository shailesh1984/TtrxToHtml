# TRX to HTML Converter (TtrxToHtml)

Convert Visual Studio TRX test results into a cleaner, more readable HTML report.

## Keywords
- trx to html converter
- convert trx file to html
- visual studio test report html
- trx report viewer

## Features
- Fast conversion
- Easy CLI usage
- Lightweight

## Purpose
trx-to-html converts one or more .trx test result files (Visual Studio test results) into a single, human-friendly HTML report. It:
- Combines multiple .trx files when a directory is provided.
- Renders test results using an embedded Razor template to produce a self-contained HTML report.
- Opens the generated report automatically after creation.

## Command-line options
- -f, --tfp     : Trx file path (single .trx file)
- -d, --tdp     : Trx directory path (all .trx files in directory will be processed)
- -hfn, --html-file-name : Optional HTML file base name (overrides default TestReportFileName)
- --info        : Print project info
- -h, --help    : Print help

(See src/TtrxToHtml.Console/Helpers/CommandLineInterfaceHelper.cs for argument parsing and validation.)

## Output
By default, a Test_Reports folder (configurable via appsettings.json) will be created in the same directory as the input .trx (or trx directory), and a timestamped HTML file will be created inside it. The application attempts to open the generated report after creation.

## Testing
A unit test for report generation exists:
- tests/TtrxToHtml.Test/GenerateTrxReportServiceTest.cs
Run tests with:
- dotnet test
