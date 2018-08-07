# Customer Tests Excel 

[![Build status](https://ci.appveyor.com/api/projects/status/wv0th0n4xknitplp?svg=true)](https://ci.appveyor.com/project/RESSoftwareTeam/customer-tests-excel)

A framework to round trip NUnit to / from Microsoft Excel customer tests.

Born out of frustration using Fitnesse.

We are primarily a science and engineering company, and due to the calculation heavy nature of our work, none of the existing customer test frameworks are a good fit for us.

Advantages
- Tests are expressed in Microsoft Excel Spreadsheets, which our customers are familiar with, and which support any complexity of calculation
- Tests are converted to C# via NUnit, and make use of existing tooling (which aids debugging, coverage, Continuous Integration etc)
- Automated refactoring can be done in C#, using existing tooling, and then the changes written back to Excel
- The NUnit tests are generated from Excel, so the two are guaranteed to be in sync

# Usage

There are SampleTests and SampleSystemUnderTest projects in the main solution (CustomerTestsExcel.sln), and in the SampleTests and SampleSystemUnderTest directories.

There is an example Excel test in SampleTests/ExcelTests/Rerouting.xlsx, which looks like this:

![Example Excel Test](example-excel-test.png "Example Excel Test")

# Building

- CustomerTestsExcel.sln

# End to End testing

The SampleTests project performs both documentation and End to End testing

During the AppVeyor build, the SampleTests Excel spreadsheets are converted in to C# tests, these tests are then run to re-create the spreadsheets, and then the spreadsheets are converted back in to C# again. This ensures that all the conepts contained within the spreadsheets are round trippable.

# Deploying

`nuget pack` in root directory, then push .nupkg to a feed



