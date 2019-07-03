using CustomerTestsExcel.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public class CombinedTestOutputWriter : ITestOutputWriter
    {
        protected readonly List<ITestOutputWriter> writers;

        public CombinedTestOutputWriter(List<ITestOutputWriter> writers)
        {
            this.writers = writers ?? throw new ArgumentNullException("writers");
        }

        public void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription) =>
            writers.ForEach(writer => writer.StartSpecification(specificationNamespace, specificationName, specificationDescription));

        public void StartGiven() =>
            writers.ForEach(writer => writer.StartGiven());

        public void StartClass(string className) =>
            writers.ForEach(writer => writer.StartClass(className));

        public void StartGivenProperties() =>
            writers.ForEach(writer => writer.StartGivenProperties());

        public void GivenProperty(ReportSpecificationSetupProperty property) =>
            writers.ForEach(writer => writer.GivenProperty(property));

        public void GivenClassProperty(string propertyName, bool isChild, int? indexInParent, bool isNull) =>
            writers.ForEach(writer => writer.GivenClassProperty(propertyName, isChild, indexInParent, isNull));

        public void EndGivenProperties() =>
            writers.ForEach(writer => writer.EndGivenProperties());

        public void StartSubClass(string className) =>
            writers.ForEach(writer => writer.StartSubClass(className));

        public void EndSubClass() =>
            writers.ForEach(writer => writer.EndSubClass());

        public void EndGiven() =>
            writers.ForEach(writer => writer.EndGiven());

        public void When(string actionName) =>
            writers.ForEach(writer => writer.When(actionName));

        public void StartAssertions() =>
            writers.ForEach(writer => writer.StartAssertions());

        public void Assert(
            string assertPropertyName,
            object assertPropertyExpectedValue,
            AssertionOperator assertionOperator,
            object assertPropertyActualValue,
            bool passed,
            IEnumerable<string> assertionSpecifics) =>
            writers.ForEach(
                writer =>
                    writer.Assert(
                        assertPropertyName,
                        assertPropertyExpectedValue,
                        assertionOperator,
                        assertPropertyActualValue,
                        passed,
                        assertionSpecifics));

        public void EndAssertions() =>
            writers.ForEach(writer => writer.EndAssertions());

        public void EndSpecification(string specificationNamespace, bool passed) =>
            writers.ForEach(writer => writer.EndSpecification(specificationNamespace, passed));

        public void Exception(string exception) =>
            writers.ForEach(writer => writer.Exception(exception));

        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue) =>
            writers.ForEach(writer => writer.CodeValueDoesNotMatchExcelFormula(assertPropertyName, excelValue, csharpValue));

        public void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed) =>
            writers.ForEach(writer => writer.StartAssertionSubProperties(assertPropertyName, exists, cSharpClassName, passed));

        public void EndAssertionSubProperties() =>
            writers.ForEach(writer => writer.EndAssertionSubProperties());

        public void EndClass() =>
            writers.ForEach(writer => writer.EndClass());

        public void StartClassTable(string propertyName, string className) =>
            writers.ForEach(writer => writer.StartClassTable(propertyName, className));

        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames) =>
            writers.ForEach(writer => writer.ClassTablePropertyNamesHeaderRow(propertyNames));

        public void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells) =>
            writers.ForEach(writer => writer.ClassTablePropertyRow(cells));

        public void EndClassTable() =>
            writers.ForEach(writer => writer.EndClassTable());

        public void StartGivenListProperty(ReportSpecificationSetupList list) =>
            writers.ForEach(writer => writer.StartGivenListProperty(list));

        public void StartGivenListPropertyItem(IReportsSpecificationSetup listItem) =>
            writers.ForEach(writer => writer.StartGivenListPropertyItem(listItem));

        public void EndGivenListPropertyItem(IReportsSpecificationSetup listItem) =>
            writers.ForEach(writer => writer.EndGivenListPropertyItem(listItem));

        public void EndGivenListProperty(ReportSpecificationSetupList list) =>
            writers.ForEach(writer => writer.EndGivenListProperty(list));
    }
}
