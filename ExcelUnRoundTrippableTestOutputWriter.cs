using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CustomerTestsExcel
{
    public class ExcelUnRoundTrippableTestOutputWriter : ExcelTestOutputWriterBase, ITestOutputWriter
    {
        readonly IEnumerable<string> issuesPreventingRoundTrip;

        public ExcelUnRoundTrippableTestOutputWriter(
            ITabularLibrary excel, 
            ICodeNameToExcelNameConverter namer, 
            string excelFolder,
            IEnumerable<string> issuesPreventingRoundTrip) 
            : base(
                  excel, 
                  namer,
                  excelFolder)
        {
            this.issuesPreventingRoundTrip = issuesPreventingRoundTrip;
        }

        public void StartSpecification(
            string specificationNamespace, 
            string specificationName, 
            string specificationDescription)
        {
            Initialise(
                specificationNamespace,
                specificationName);

            SetCell(1, 1, namer.Specification);
            SetCell(1, 2, specificationDescription);

            ClearSkippedCellWarnings();
            AddWarning("This test cannot be converted from C# to excel. The following issues are preventing this:\r\n");
            issuesPreventingRoundTrip.ToList().ForEach(issue => AddWarning(issue + "\r\n"));
        }

#pragma warning disable S1186 // Methods should not be empty
        // This is a violation of the interface segragation principle, as we have to implement a lot
        // of functions that we don't want or need, but this is basically a null object implementation,
        // so I think its ok.
        public void StartGiven() { }
        public void StartClass(string className) { }
        public void EndClass() { }
        public void StartGivenProperties() { }
        public void EndGivenProperties() { }
        public void GivenClassProperty(string propertyName, bool isChild, int? indexInParent, bool isNull) { }
        public void StartSubClass(string className) { }
        public void EndSubClass() { }
        public void GivenProperty(ReportSpecificationSetupProperty property) { }
        public void StartClassTable(string propertyName, string className) { }
        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames) { }
        public void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells) { }
        public void EndClassTable() { }
        public void EndGiven() { }
        public void When(string actionName) { }
        public void StartAssertions() { }
        public void Assert(string assertPropertyName, object assertPropertyExpectedValue, AssertionOperator assertionOperator, object assertPropertyActualValue, bool passed, IEnumerable<string> assertionSpecifics) { }
        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue) { }
        public void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed) { }
        public void EndAssertionSubProperties() { }
        public void EndAssertions() { }
        public void EndSpecification(string specificationNamespace, bool passed) { }
        public void Exception(string exception) { }
#pragma warning restore S1186 // Methods should not be empty
    }
}
