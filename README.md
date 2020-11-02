# Customer Tests Excel 

[![Build status](https://buildstats.info/appveyor/chart/RESSoftwareTeam/customer-tests-excel)](https://ci.appveyor.com/project/RESSoftwareTeam/customer-tests-excel)

[![codecov](https://codecov.io/gh/resgroup/customer-tests-excel/branch/master/graph/badge.svg)](https://codecov.io/gh/resgroup/customer-tests-excel)

[![Maintainability](https://api.codeclimate.com/v1/badges/1bc743c6d68c2f245cd8/maintainability)](https://codeclimate.com/github/resgroup/customer-tests-excel/maintainability)


A framework to round trip NUnit to / from Microsoft Excel customer tests.

Created as a replacment for Fitnesse. RES is primarily a science and engineering company, and due to the calculation heavy nature of the work, none of the existing customer test frameworks are a good fit.

Advantages
- Tests are expressed in Microsoft Excel Spreadsheets, which our customers are familiar with, and which support any complexity of calculation
- Tests are converted to C# / NUnit, and make use of existing tooling (which aids debugging, coverage, Continuous Integration etc)
- Automated refactoring can be done in C#, using existing tooling, and then the changes written back to Excel
- The NUnit tests are generated from Excel, so the two are guaranteed to be in sync
- Most scaffolding code can be generated, you usually only need to supply the code to call your system under test

# Usage

The easiest way to get started is to use the [scaffolding project](https://github.com/resgroup/customer-tests-excel-scaffolding).

However you can do it manually

- Create a new .net Core project (.csproj) for the Customer Tests
- Install the Nuget Package `CustomerTestsExcel`
- Create an `ExcelTests` subfolder
- Create an Excel Spreadsheet in the `ExcelTests` subfolder (like the ones in [SampleTests\ExcelTests\](SampleTests\ExcelTests\)).
- To generate the C# tests run `GenerateCodeFromExcelTest.exe` (as is done in [SampleTests\ExcelTests\GenerateTests.bat](SampleTests\ExcelTests\GenerateTests.bat))
- Enhance the generated code with any custom code required, and run the tests
- To recreate the Excel Spreadsheets from the C# code (should you want to)
  - Set a `CUSTOMER_TESTS_EXCEL_WRITE_TO_EXCEL` environment variable to `true`
  - Set a `CUSTOMER_TESTS_RELATIVE_PATH_TO_EXCELTESTS` environment variable from the Output Folder to the `ExcelTests` folder (usually `..\..\..\..\SampleTests\ExcelTests`)
  - Run the tests
  
The Excel test for the classic Rerouting example (`SampleTests/ExcelTests/Rerouting.xlsx`) looks like this:

![Example Excel Test](media/example-excel-test.png "Example Excel Test")

# Documentation

The [scaffolding project](https://github.com/resgroup/customer-tests-excel-scaffolding) has sample tests that showcase all of the functionality supported by the framework, and it contains instructions and examples for augmenting the generated code with custom code.

[doc/excel-syntax.md](doc/excel-syntax.md) shows the Excel syntax used for describing tests.

# Building Locally

- CustomerTestsExcel.sln

# End to End testing

The SampleTests project performs both documentation and End to End testing

During the AppVeyor build, the SampleTests Excel spreadsheets are converted in to C# tests, these tests are then run to re-create the spreadsheets, and then the spreadsheets are converted back in to C# again. This ensures that all the conepts contained within the spreadsheets are round trippable.

# Deploying the Nuget Package

Click "Deploy" on the relevant AppVeyor build if you have access to it.

Otherwise
- `dotnet build.exe CustomerTestsExcel.sln`
- push .nupkg to a feed



