using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CustomerTestsExcel
{
    public class ExcelTestOutputWriter : ExcelTestOutputWriterBase, ITestOutputWriter
    {
        readonly string _excelFolder;
        public ExcelTestOutputWriter(ITabularLibrary excel, ICodeNameToExcelNameConverter namer, string excelFolder) : base(excel, namer) { _excelFolder = excelFolder; }

        public void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription)
        {
            var fileName = GetFilename(specificationNamespace);
            if (File.Exists(fileName))
            {
                _workbook = _excel.NewBook(fileName);
            }
            else
            {
                _workbook = _excel.NewBook();
                //standlone todo _workbook.RemoveDefaultSheets();
            }

            string specificationFriendlyName = _namer.CodeSpecificationClassNameToExcelName(specificationName);

            if (_workbook.GetPageNames().Contains(specificationFriendlyName))
            {
                _worksheet = _workbook.GetPage(specificationFriendlyName);
            }
            else
            {
                _worksheet = _workbook.AddPageBefore(0);
                _worksheet.Name = specificationFriendlyName;
            }

            SetCell(1, 1, _namer.Specification);
            SetCell(1, 2, specificationDescription);
            ClearSkippedCellWarnings();
        }


        public void StartGiven()
        {
            SetPosition(3, 1);

            SetCell(_namer.Given);

            Indent();
        }

        public void StartClass(string className)
        {
            SetCell(_namer.CodeClassNameToExcelName(className));

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
            SetCell(_namer.GivenPropertyNameCodeNameToExcelName(propertyName, isChild, indexInParent));
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

            SetCell(_namer.CodeClassNameToExcelName(className));

            MoveToNextRow();
        }

        public void EndSubClass()
        {
            UnIndent();
        }
        public void GivenProperty(ReportSpecificationSetupProperty property)
        {
            SetCell(_namer.GivenPropertyNameCodeNameToExcelName(property.PropertyName, false, null));
            Indent();
            SetCell(_namer.PropertyValueCodeToExcel(property.PropertyNamespace, property.PropertyValue));
            MoveToNextRow();
            UnIndent();
        }

        public void StartClassTable(string propertyName, string className)
        {
            SetCell(_namer.GivenPropertyNameCodeNameToExcelName(propertyName, false, null));
            Indent();
            SetCell(_namer.CodeClassNameToExcelName(className));
            MoveToNextRow();
        }

        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames)
        {
            // write out the "withproperties" row
            SetCell(_namer.Properties);

            MoveToNextRow();

            // write out the property names
            using (SavePosition())
            {
                foreach (var property in propertyNames)
                {
                    SetCell(_namer.GivenPropertyNameCodeNameToExcelName(property, false, null));
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
                    SetCell(_namer.PropertyValueCodeToExcel(cell.PropertyNamespace, cell.PropertyValue));
                    Indent();
                }
            }

            MoveToNextRow();
        }

        public void EndClassTable()
        {
        }

        public void EndGiven()
        {
            MoveToNextRow();
        }

        public void When(string actionName)
        {
            SetColumn(1);

            SetCell(_namer.When);

            Indent();

            SetCell(_namer.ActionCodeNameToExcelName(actionName));

            MoveToNextRow();
        }

        public void StartAssertions()
        {
            MoveToNextRow();

            SetColumn(1);

            SetCell(_namer.Assert);

            MoveToNextRow();

            SetColumn(2);
        }

        public void Assert(string assertPropertyName, object assertPropertyExpectedValue, AssertionOperator assertionOperator, object assertPropertyActualValue, bool passed, IEnumerable<string> assertionSpecifics)
        {
            using (SavePosition())
            {
                SetCell(_namer.AssertPropertyCodeNameToExcelName(assertPropertyName));
                Indent();

                SetCell(_namer.AssertionOperatorCodeNameToExcelName(assertionOperator));
                Indent();

                SetCell(_namer.AssertValueCodeNameToExcelName(assertPropertyExpectedValue));
                Indent();

                foreach (var assertionSpecific in assertionSpecifics)
                {
                    SetCell(assertionSpecific);
                    Indent();
                }
            }

            MoveToNextRow();
        }

        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue)
        {

        }

        public void StartAssertionSubProperties(string cSharpMethodName, bool exists, string cSharpClassName, bool passed)
        {
            SetCell(_namer.AssertionSubPropertyCodeNameToExcelName(cSharpMethodName));

            Indent();

            SetCell(_namer.AssertionSubPropertyCodeNameToExcelName(cSharpClassName));

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
            _worksheet = null;

            _workbook.SaveAs(GetFilename(specificationNamespace));

            // standlone todo _workbook.Dispose();
            _workbook = null;
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

        private string GetFilename(string assemblyName)
        {
            // executing directory is expected to be svn\builtsdlls\debug, you need to make sure that the build path for your test projects are set to build here.
            return Path.Combine(_excelFolder, _namer.CodeNamespaceToExcelFileName(assemblyName) + "." + _excel.DefaultExtension);
        }

    }
}
