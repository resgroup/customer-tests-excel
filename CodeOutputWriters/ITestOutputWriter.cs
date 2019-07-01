using CustomerTestsExcel.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public interface ITestOutputWriter
    {
        void Exception(string exception);

        void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription);

        void StartGiven();

        // should probably add "Given" to these names so that they all follow the same convention
        void StartClass(string className);
        void StartSubClass(string className);
        void EndSubClass();
        void EndClass();

        void StartClassTable(string propertyName, string className);
        void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames);
        void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells);
        void EndClassTable();

        void StartGivenProperties();
        void EndGivenProperties();

        void GivenProperty(ReportSpecificationSetupProperty property);

        void GivenClassProperty(string propertyName, bool isChild, int? indexInParent, bool isNull);

        void EndGiven();

        void When(string actionName);

        void Assert(
            string assertPropertyName,
            object assertPropertyExpectedValue,
            AssertionOperator assertionOperator,
            object assertPropertyActualValue,
            bool passed,
            IEnumerable<string> assertionSpecifics);

        void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue);

        void EndSpecification(string specificationNamespace, bool passed);

        void StartAssertions();

        void EndAssertions();

        void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed);

        void EndAssertionSubProperties();

    }
}
