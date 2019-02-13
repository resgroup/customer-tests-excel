using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class ExcelTestOutputWriterBase
    {
        readonly string excelFolder;
        protected readonly ITabularLibrary excel;
        protected readonly ICodeNameToExcelNameConverter namer;
        protected ITabularBook workbook;
        protected ITabularPage worksheet;
        protected uint row;
        protected uint column;

        public ExcelTestOutputWriterBase(
            ITabularLibrary excel, 
            ICodeNameToExcelNameConverter namer,
            string excelFolder)
        {
            this.excel = excel ?? throw new ArgumentNullException(nameof(excel));
            this.namer = namer ?? throw new ArgumentNullException(nameof(namer));
            this.excelFolder = excelFolder ?? throw new ArgumentNullException(nameof(excelFolder));
        }

        protected void Initialise(
            string specificationNamespace,
            string specificationName)
        {
            workbook = Workbook(specificationNamespace);

            worksheet = Worksheet(specificationName);
        }

        ITabularBook Workbook(string specificationNamespace)
        {
            var fileName = GetFilename(specificationNamespace);

            if (File.Exists(fileName))
                return excel.NewBook(fileName);
            else
                return excel.NewBook();
        }

        ITabularPage Worksheet(string specificationName)
        {
            string specificationFriendlyName = namer.CodeSpecificationClassNameToExcelName(specificationName);

            if (workbook.GetPageNames().Contains(specificationFriendlyName))
            {
                return workbook.GetPage(specificationFriendlyName);
            }
            else
            {
                var worksheet = workbook.AddPageBefore(0);
                worksheet.Name = specificationFriendlyName;
                return worksheet;
            }
        }

        protected void SetCell(uint row, uint column, string cSharpValue, object excelValue)
        {
            var cell = worksheet.GetCell(row, column);
            if (cell.Value == null || (cell.Value.Equals(excelValue) == false && cell.Value.ToString() != cSharpValue))
            {
                if (!cell.IsFormula)
                {
                    worksheet.SetCell(row, column, excelValue);
                }
                else
                {
                    AddSkippedCellWarning(row, column, excelValue);
                }
            }
        }

        protected void SetCell(uint row, uint column, object value)
        {
            SetCell(row, column, "", value);
        }

        protected void ClearSkippedCellWarnings()
        {
            worksheet.SetCell(1, 3, "");
        }

        void AddSkippedCellWarning(uint row, uint column, object value)
        {
            AddWarning($"{worksheet.GetCell(1, 3).Value} Skipped updating Cell R{row}C{column} to '{value}' as it has a formula in it. Please fix this value by hand, or remove the formula and re run the test.\r\n");
        }

        protected void AddWarning(string warning)
        {
            // put all warnings in cell 1, 3, which is reserved for this purpose
            worksheet.SetCell(1, 3, warning);
        }

        protected void SetCell(object value)
        {
            SetCell(row, column, "", value);
        }

        protected void SetCell(string cSharpValue, object value)
        {
            SetCell(row, column, cSharpValue, value);
        }

        protected void SetPosition(uint row, uint column)
        {
            this.row = row;
            this.column = column;
        }

        protected void SetColumn(uint column)
        {
            SetPosition(row, column);
        }

        protected void MoveToNextRow()
        {
            row++;
        }

        protected void Indent(uint by = 1)
        {
            column += by;
        }

        protected void UnIndent(uint by = 1)
        {
            column -= by;
        }

        protected TidyUp SavePosition()
        {
            uint savedColumn = column;
            uint savedRow = row;

            return new TidyUp(() => SetPosition(savedRow, savedColumn));
        }

        protected string GetFilename(string assemblyName)
        {
            return Path.Combine(excelFolder, namer.CodeNamespaceToExcelFileName(assemblyName) + "." + excel.DefaultExtension);
        }

    }
}
