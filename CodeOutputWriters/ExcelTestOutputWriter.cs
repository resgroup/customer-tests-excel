using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CustomerTestsExcel.Assertions;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public class ExcelTestOutputWriter : ExcelTestOutputWriterBase, ITestOutputWriter
    {
        public ExcelTestOutputWriter(
            ITabularLibrary excel,
            ICodeNameToExcelNameConverter namer,
            string excelFolder)
            : base(
                  excel,
                  namer,
                  excelFolder)
        {
        }

        public void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription)
        {
            Initialise(
                specificationNamespace,
                specificationName);

            SetCell(1, 1, namer.Specification);
            SetCell(1, 2, specificationDescription);
            ClearSkippedCellWarnings();
        }


        public void StartGiven()
        {
            SetPosition(3, 1);

            SetCell(namer.Given);

            Indent();
        }

        public void StartClass(string className)
        {
            SetCell(namer.CodeClassNameToExcelName(className));

            MoveToNextRow();
        }

        public void EndClass()
        {
            UnIndent();
        }

        public void StartGivenProperties()
        {
            SetCell("With Properties");

            MoveToNextRow();

            Indent();
        }

        public void EndGivenProperties()
        {
            UnIndent();
        }

        public void GivenClassProperty(string propertyName, bool isChild, int? indexInParent, bool isNull)
        {
            SetCell(namer.GivenPropertyNameCodeNameToExcelName(propertyName, indexInParent));
            if (isNull)
            {
                Indent();
                SetCell("null");
                UnIndent();
                MoveToNextRow();
            }
        }

        public void StartSubClass(string className)
        {
            Indent();

            SetCell(namer.CodeClassNameToExcelName(className));

            MoveToNextRow();
        }

        public void EndSubClass()
        {
            UnIndent();
        }

        public void GivenProperty(ReportSpecificationSetupProperty property)
        {
            SetCell(namer.GivenPropertyNameCodeNameToExcelName(property.PropertyName, null));
            Indent();
            SetCell(namer.PropertyValueCodeToExcel(property.PropertyNamespace, property.PropertyValue));
            MoveToNextRow();
            UnIndent();
        }

        public void StartClassTable(string propertyName, string className)
        {
            SetCell(namer.GivenPropertyNameCodeNameToExcelName(propertyName, null));
            Indent();
            SetCell(namer.CodeClassNameToExcelName(className));
            MoveToNextRow();
        }

        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames)
        {
            // write out the "withproperties" row
            SetCell(namer.WithProperties);

            MoveToNextRow();

            // write out the property names
            using (SavePosition())
            {
                foreach (var property in propertyNames)
                {
                    SetCell(namer.GivenPropertyNameCodeNameToExcelName(property, null));
                    Indent();
                }
            }

            MoveToNextRow();
        }

        public void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells)
        {
            using (SavePosition())
            {
                foreach (var cell in cells)
                {
                    SetCell(namer.PropertyValueExcelToCode(namer.AssertPropertyCodeNameToExcelName(cell.PropertyName), cell.PropertyValue), namer.PropertyValueCodeToExcel(cell.PropertyNamespace, cell.PropertyValue));
                    Indent();
                }
            }

            MoveToNextRow();
        }

        public void EndClassTable()
        {
            UnIndent();
        }

        public void EndGiven()
        {
            MoveToNextRow();
        }

        public void When(string actionName)
        {
            SetColumn(1);

            SetCell(namer.When);

            Indent();

            SetCell(namer.ActionCodeNameToExcelName(actionName));

            MoveToNextRow();
        }

        public void StartAssertions()
        {
            MoveToNextRow();

            SetColumn(1);

            SetCell(namer.Assert);

            MoveToNextRow();

            SetColumn(2);
        }

        public void Assert(
            string assertPropertyName,
            object assertPropertyExpectedValue,
            AssertionOperator assertionOperator,
            object assertPropertyActualValue,
            bool passed,
            IEnumerable<string> assertionSpecifics)
        {
            using (SavePosition())
            {
                SetCell(namer.AssertPropertyCodeNameToExcelName(assertPropertyName), assertPropertyName);
                Indent();

                SetCell(namer.AssertionOperatorCodeNameToExcelName(assertionOperator));
                Indent();

                SetCell(namer.PropertyValueExcelToCode(namer.AssertPropertyCodeNameToExcelName(assertPropertyName), assertPropertyExpectedValue), namer.AssertValueCodeNameToExcelName(assertPropertyExpectedValue));
                Indent();

                foreach (var assertionSpecific in assertionSpecifics)
                {
                    SetCell(assertionSpecific, assertionSpecific);
                    Indent();
                }
            }

            MoveToNextRow();
        }

        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue)
        {

        }

        public void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed)
        {
            SetCell(namer.AssertionSubPropertyCodePropertyNameToExcelName(assertPropertyName));

            Indent();

            SetCell(namer.AssertionSubPropertyCodeClassNameToExcelName(cSharpClassName));

            MoveToNextRow();

        }

        public void EndAssertionSubProperties()
        {
            UnIndent();
        }

        public void EndAssertions()
        {
        }

        public void EndSpecification(string specificationNamespace, bool passed)
        {
            worksheet = null;

            workbook.SaveAs(GetFilename(specificationNamespace));

            workbook = null;
        }

        public void Exception(string exception)
        {
            //IExcelWorksheet exceptionWorksheet;

            //if (_workbook.GetSheetNames().Contains("Exceptions"))
            //{
            //    exceptionWorksheet = _workbook.GetWorkSheet("Exceptions");
            //}
            //else
            //{
            //    exceptionWorksheet = _workbook.AddWorkSheet();
            //    exceptionWorksheet.Name = "Exceptions";
            //}

            //exceptionWorksheet.GetCell(_exceptionRow, 1).Value = "Exception: " + exception;
        }

        public void StartGivenListProperty(ReportSpecificationSetupList list) => throw new NotImplementedException();
        public void StartGivenListPropertyItem(IReportsSpecificationSetup listItem) => throw new NotImplementedException();
        public void EndGivenListPropertyItem(IReportsSpecificationSetup listItem) => throw new NotImplementedException();
        public void EndGivenListProperty(ReportSpecificationSetupList list) => throw new NotImplementedException();
    }
}
