using System;
using System.Collections.Generic;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    public class GivenComplexPropertyVisitRecorder : IExcelToCodeVisitor
    {
        readonly List<string> recordedComplexProperties = new List<string>();
        public IReadOnlyList<string> RecordedComplexProperties =>
            recordedComplexProperties;

        public void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty) =>
            recordedComplexProperties.Add(givenComplexProperty.ToString());

        public void VisitGivenComplexPropertyFinalisation() =>
            recordedComplexProperties.Add("Finalisation");

        public void VisitGivenListPropertyDeclaration(IGivenListProperty givenListProperty)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenListPropertyFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyCellDeclaration(ExcelToCode.TableHeader tableHeader, uint row, uint column)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyCellFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyDeclaration(IGivenTableProperty givenTableProperty, IEnumerable<ExcelToCode.TableHeader> tableHeaders)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyRowDeclaration(uint row)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyRowFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

    }
}
