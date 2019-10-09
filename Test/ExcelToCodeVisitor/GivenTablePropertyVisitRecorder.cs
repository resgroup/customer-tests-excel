using System;
using System.Collections.Generic;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    public class GivenTablePropertyVisitRecorder : IExcelToCodeVisitor
    {
        readonly List<string> recordedTableProperties = new List<string>();
        public IReadOnlyList<string> RecordedTableProperties =>
            recordedTableProperties;

        public void VisitGivenTablePropertyDeclaration(IEnumerable<TableHeader> tableHeaders) =>
            recordedTableProperties.Add($"Table {tableHeaders}");

        public void VisitGivenTablePropertyRowDeclaration(uint row) =>
            recordedTableProperties.Add($"RowDeclaration {row}");

        public void VisitGivenTablePropertyCellDeclaration(TableHeader tableHeader, uint row, uint column) =>
            recordedTableProperties.Add($"Cell({row}, {column}) {tableHeader}");

        public void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty) =>
            recordedTableProperties.Add(givenComplexProperty.ToString());

        public void VisitGivenComplexPropertyFinalisation() =>
            recordedTableProperties.Add("ComplexPropertyFinalisation");

        public void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty) =>
            recordedTableProperties.Add(givenSimpleProperty.ToString());

        public void VisitGivenTablePropertyCellFinalisation() =>
            recordedTableProperties.Add("CellFinalisation");

        public void VisitGivenTablePropertyRowFinalisation() =>
            recordedTableProperties.Add("RowFinalisation");

        public void VisitGivenTablePropertyFinalisation() =>
            recordedTableProperties.Add("TableFinalisation");

        public void VisitGivenListPropertyDeclaration(IGivenListProperty givenListProperty)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenListPropertyFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }
    }
}
