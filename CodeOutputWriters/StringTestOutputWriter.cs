using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CustomerTestsExcel.Assertions;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public class StringTestOutputWriter : StringTestOutputWriterBase, ITestOutputWriter
    {
        public StringTestOutputWriter(IHumanFriendlyFormatter formatter, ITextLineWriter writer) : base(formatter, writer)
        {
        }


        public void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription)
        {
            WriteLine(specificationNamespace);
            WriteLine("--------------------------------------------------");
            WriteLine("Specification: " + specificationDescription);
        }

        public void StartGiven()
        {
            WriteLine("");
            StartLine("Given a ");
        }

        public void StartClass(string className)
        {
            EndLine(className.Replace("SpecificationSpecific", ""));
            Indent();
        }

        public void StartGivenProperties()
        {
            WriteLine("With Properties");
            Indent();
        }

        public void EndGivenProperties()
        {
            Outdent();
        }

        public void GivenClassProperty(string propertyName, bool isNull)
        {
            WriteLine(propertyName + " " + (isNull ? "null" : ""));
        }

        public void GivenProperty(ReportSpecificationSetupProperty property)
        {
            WriteLine(property.PropertyName + " " + _formatter.FormatValue(property.PropertyValue));
        }

        public void StartSubClass(string className)
        {
            Indent();
            WriteLine(_formatter.FormatSpecificationSpecificClassName(className));
            Indent();
        }

        public void EndSubClass()
        {
            Outdent();
            Outdent();
        }

        public void EndClass()
        {
            Outdent();
        }

        public void StartClassTable(string propertyName, string className)
        {
            WriteLine(propertyName + " " + _formatter.FormatValue(className));
            Indent();
        }

        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames)
        {
            WriteLine(string.Join(", ", propertyNames));
        }

        public void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells)
        {
            WriteLine(string.Join(", ", cells.Select(c => _formatter.FormatValue(c.PropertyValue))));
        }

        public void EndClassTable()
        {
            Outdent();
        }

        public void StartGivenListProperty(ReportSpecificationSetupList list)
        {
            WriteLine($"{_formatter.FormatMethodName(list.PropertyName)} {_formatter.FormatSpecificationSpecificClassName(list.PropertyType)}");
            Indent();

        }

        public void StartGivenListPropertyItem(IReportsSpecificationSetup listItem)
        {
            WriteLine("With Item");
            Indent();
        }

        public void EndGivenListPropertyItem(IReportsSpecificationSetup listItem)
        {
            Outdent();
        }

        public void EndGivenListProperty(ReportSpecificationSetupList list)
        {
            Outdent();
        }

        public void EndGiven()
        {

        }

        public void When(string actionName)
        {
            WriteLine("");
            WriteLine("When " + actionName);
            WriteLine("");
        }

        public void StartAssertions()
        {
            WriteLine("Assert");
            Indent();
        }

        public void Assert(
            string assertPropertyName,
            object assertPropertyExpectedValue,
            AssertionOperator assertionOperator,
            object assertPropertyActualValue,
            bool passed,
            IEnumerable<string> assertionSpecifics)
        {
            StartLine(assertPropertyName + " " + assertionOperator.ToDescription() + " " + _formatter.FormatValue(assertPropertyExpectedValue) + '\t' + (passed ? "(Passed)" : "(Failed, Actual value: " + _formatter.FormatValue(assertPropertyActualValue) + ")"));

            foreach (var assertionSpecific in assertionSpecifics)
            {
                ContinueLine('\t' + assertionSpecific);
            }

            EndLine("");
        }

        public void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed)
        {
            if (exists)
                WriteLine(_formatter.FormatSpecificationSpecificClassName(assertPropertyName));
            else
                WriteLine("Failed: " + _formatter.FormatSpecificationSpecificClassName(assertPropertyName) + " does not exist");

            Indent();
        }

        public void EndAssertionSubProperties()
        {
            Outdent();
        }

        public void EndAssertions()
        {
            Outdent();
        }

        public void EndSpecification(string specificationNamespace, bool passed)
        {
            WriteLine("--------------------------------------------------");
        }

        public void Exception(string exception)
        {
            WriteLine("Exception: " + exception);
        }

        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue)
        {
            WriteLine(string.Format("The value in the c# code ({0}) does not match the value in the excel sheet ({1} for {2}", csharpValue, excelValue, assertPropertyName));
        }

    }
}
