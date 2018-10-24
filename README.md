# Customer Tests Excel 

[![Build status](https://ci.appveyor.com/api/projects/status/wv0th0n4xknitplp?svg=true)](https://ci.appveyor.com/project/RESSoftwareTeam/customer-tests-excel)

A framework to round trip NUnit to / from Microsoft Excel customer tests.

Created as a replacment for Fitnesse. RES is primarily a science and engineering company, and due to the calculation heavy nature of the work, none of the existing customer test frameworks are a good fit.

Advantages
- Tests are expressed in Microsoft Excel Spreadsheets, which our customers are familiar with, and which support any complexity of calculation
- Tests are converted to C# / NUnit, and make use of existing tooling (which aids debugging, coverage, Continuous Integration etc)
- Automated refactoring can be done in C#, using existing tooling, and then the changes written back to Excel
- The NUnit tests are generated from Excel, so the two are guaranteed to be in sync

# Usage

- Install the Nuget Package `CustomerTestsExcel`
- Create an Excel Spreadsheet with a Test (see below for format)
- Run `GenerateCodeFromExcelTest.exe` to create / update the C# test project
 - For example `GenerateCodeFromExcelTest.exe /folder "SampleTests" /project SampleTests.csproj /namespace SampleTests /usings "SampleSystemUnderTest" /assertionClassPrefix "I"`
 - `GenerateCodeFromExcelTest.exe` will be in the `tools` folder of the nuget package (for example `CustomerTestsExcel.1.0.1\tools`)
 - `/folder` is the desired folder of the Test project relative to the current working directory
 - `/project` is the desired name of the visual studio project
 - `/namespace' is the desired namespace for the tests
 - `/usings' is space delimited list of namespaces that will be added as `using` statements to the tests
 - `/assertionClassPrefix` is added to the Excel assertion type names when creating the C# names.
- Open the test project in Visual Studio (For example `SampleTests\SampleTests.csproj`)
 - Put any custom code under a `IgnoreOnGeneration` folder, so that it will remain when the test project is regenerated.
- To create the Excel Spreadsheet from the C# code
  - Set a `CUSTOMER_TESTS_EXCEL_WRITE_TO_EXCEL` environment variable to `true`
  - Set a `CUSTOMER_TESTS_RELATIVE_PATH_TO_EXCELTESTS` environment variable from the Output Folder (for example `bin\debug`) to the `ExcelTests` folder
  - Run the tests
  
See the SampleTests and SampleSystemUnderTest projects for examples.

The Excel test for the Rerouting example (`SampleTests/ExcelTests/Rerouting.xlsx`) looks like this:

![Example Excel Test](example-excel-test.png "Example Excel Test")

# Building Locally

- CustomerTestsExcel.sln

# Build Server (private to RES)

- https://ci.appveyor.com/project/RESSoftwareTeam/customer-tests-excel

# End to End testing

The SampleTests project performs both documentation and End to End testing

During the AppVeyor build, the SampleTests Excel spreadsheets are converted in to C# tests, these tests are then run to re-create the spreadsheets, and then the spreadsheets are converted back in to C# again. This ensures that all the conepts contained within the spreadsheets are round trippable.

# Deploying

Click "Deploy" on the relevant AppVeyor build if you have access to it.

Otherwise, `nuget pack CustomerTestsExcel.csproj -build` in root directory, then push .nupkg to a feed



