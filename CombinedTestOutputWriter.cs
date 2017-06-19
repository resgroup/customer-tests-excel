using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class CombinedTestOutputWriter : ITestOutputWriter
    {
        protected readonly List<ITestOutputWriter> _writers;

        public CombinedTestOutputWriter(List<ITestOutputWriter> writers)
        {
            if (writers == null) throw new ArgumentNullException("writers");

            _writers = writers;
        }

        public void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription)
        {
            foreach (var writer in _writers)
            {
                writer.StartSpecification(specificationNamespace, specificationName, specificationDescription);
            }
        }

        public void StartGiven()
        {
            foreach (var writer in _writers)
            {
                writer.StartGiven();
            }
        }

        public void StartClass(string className)
        {
            foreach (var writer in _writers)
            {
                writer.StartClass(className);
            }
        }

        public void StartGivenProperties()
        {
            foreach (var writer in _writers)
            {
                writer.StartGivenProperties();
            }
        }

        public void GivenProperty(ReportSpecificationSetupProperty property)
        {
            foreach (var writer in _writers)
            {
                writer.GivenProperty(property);
            }
        }

        public void GivenClassProperty(string propertyName, bool isChild, int? indexInParent, bool isNull)
        {
            foreach (var writer in _writers)
            {
                writer.GivenClassProperty(propertyName, isChild, indexInParent, isNull);
            }
        }

        public void EndGivenProperties()
        {
            foreach (var writer in _writers)
            {
                writer.EndGivenProperties();
            }
        }

        public void StartSubClass(string className)
        {
            foreach (var writer in _writers)
            {
                writer.StartSubClass(className);
            }
        }

        public void EndSubClass()
        {
            foreach (var writer in _writers)
            {
                writer.EndSubClass();
            }
        }


        public void EndGiven()
        {
            foreach (var writer in _writers)
            {
                writer.EndGiven();
            }
        }

        public void When(string actionName)
        {
            foreach (var writer in _writers)
            {
                writer.When(actionName);
            }
        }

        public void StartAssertions()
        {
            foreach (var writer in _writers)
            {
                writer.StartAssertions();
            }
        }

        public void Assert(string assertPropertyName, object assertPropertyExpectedValue, AssertionOperator assertionOperator, object assertPropertyActualValue, bool passed, IEnumerable<string> assertionSpecifics)
        {
            foreach (var writer in _writers)
            {
                writer.Assert(assertPropertyName, assertPropertyExpectedValue, assertionOperator, assertPropertyActualValue, passed, assertionSpecifics);
            }
        }

        public void EndAssertions()
        {
            foreach (var writer in _writers)
            {
                writer.EndAssertions();
            }
        }

        public void EndSpecification(string specificationNamespace, bool passed)
        {
            foreach (var writer in _writers)
            {
                writer.EndSpecification(specificationNamespace, passed);
            }
        }

        public void Exception(string exception)
        {
            foreach (var writer in _writers)
            {
                writer.Exception(exception);
            }
        }

        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue)
        {
            foreach (var writer in _writers)
            {
                writer.CodeValueDoesNotMatchExcelFormula(assertPropertyName, excelValue, csharpValue);
            }
        }

        public void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed)
        {
            foreach (var writer in _writers)
            {
                writer.StartAssertionSubProperties(assertPropertyName, exists, cSharpClassName, passed);
            }
        }

        public void EndAssertionSubProperties()
        {
            foreach (var writer in _writers)
            {
                writer.EndAssertionSubProperties();
            }
        }

        public void EndClass()
        {
            foreach (var writer in _writers)
            {
                writer.EndClass();
            }
        }

        public void StartClassTable(string propertyName, string className)
        {
            foreach (var writer in _writers)
            {
                writer.StartClassTable(propertyName, className);
            }
        }

        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames)
        {
            foreach (var writer in _writers)
            {
                writer.ClassTablePropertyNamesHeaderRow(propertyNames);
            }
        }

        public void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells)
        {
            foreach (var writer in _writers)
            {
                writer.ClassTablePropertyRow(cells);
            }
        }

        public void EndClassTable()
        {
            foreach (var writer in _writers)
            {
                writer.EndClassTable();
            }
        }

    }
}
