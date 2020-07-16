using System;
using System.Collections.Generic;
using System.Linq;
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

        public void VisitGivenTablePropertyDeclaration(IVisitedGivenTableProperty givenTableProperty, IEnumerable<TableHeader> tableHeaders) =>
            recordedTableProperties.Add($"Table [{string.Join(",", tableHeaders.Select(h => h.ToString()))}]");

        public void VisitGivenTablePropertyRowDeclaration(uint row) =>
            recordedTableProperties.Add($"RowDeclaration {row}");

        public void VisitGivenTablePropertyCellDeclaration(TableHeader tableHeader, uint row, uint column) =>
            recordedTableProperties.Add($"Cell({row}, {column})");

        public void VisitGivenComplexPropertyDeclaration(IVisitedGivenComplexProperty givenComplexProperty) =>
            recordedTableProperties.Add(givenComplexProperty.ToString());

        public void VisitGivenComplexPropertyFinalisation() =>
            recordedTableProperties.Add("ComplexPropertyFinalisation");

        public void VisitGivenSimpleProperty(IVisitedGivenSimpleProperty givenSimpleProperty) =>
            recordedTableProperties.Add(givenSimpleProperty.ToString());

        public void VisitGivenTablePropertyCellFinalisation() =>
            recordedTableProperties.Add("CellFinalisation");

        public void VisitGivenTablePropertyRowFinalisation() =>
            recordedTableProperties.Add("RowFinalisation");

        public void VisitGivenTablePropertyFinalisation() =>
            recordedTableProperties.Add("TableFinalisation");

        public void VisitGivenListPropertyDeclaration(IVisitedGivenListProperty givenListProperty)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenListPropertyFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenRootClassDeclaration(string className)
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenRootClassFinalisation()
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenFunction(IVisitedGivenFunction givenFunction)
        {
            // ignore to keep test simple, just focus on the complex visits
        }
    }
}
