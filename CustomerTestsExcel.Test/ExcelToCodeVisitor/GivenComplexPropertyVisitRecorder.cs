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

        public void VisitGivenRootClassDeclaration(string className) =>
            recordedComplexProperties.Add(className);

        public void VisitGivenRootClassFinalisation() =>
            recordedComplexProperties.Add("Finalisation");

        public void VisitGivenComplexPropertyDeclaration(IVisitedGivenComplexProperty givenComplexProperty) =>
            recordedComplexProperties.Add(givenComplexProperty.ToString());

        public void VisitGivenComplexPropertyFinalisation() =>
            recordedComplexProperties.Add("Finalisation");

        public void VisitGivenListPropertyDeclaration(IVisitedGivenListProperty givenListProperty)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenListPropertyFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenSimpleProperty(IVisitedGivenSimpleProperty givenSimpleProperty)
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

        public void VisitGivenTablePropertyDeclaration(IVisitedGivenTableProperty givenTableProperty, IEnumerable<ExcelToCode.TableHeader> tableHeaders)
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

        public void VisitGivenFunction(IVisitedGivenFunction givenFunction)
        {
            // ignore to keep test simple, just focus on the complex visits
        }
}
}
