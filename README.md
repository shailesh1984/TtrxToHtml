# Test TRX to HTML Converter (TtrxToHtml)

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
