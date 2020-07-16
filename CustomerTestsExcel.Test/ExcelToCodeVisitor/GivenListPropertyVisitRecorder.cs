using System;
using System.Collections.Generic;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    public class GivenListPropertyVisitRecorder : IExcelToCodeVisitor
    {
        readonly List<string> recordedListProperties = new List<string>();
        public IReadOnlyList<string> RecordedListProperties =>
            recordedListProperties;

        public void VisitGivenRootClassDeclaration(string className)
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenRootClassFinalisation()
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenListPropertyDeclaration(IVisitedGivenListProperty givenListProperty) =>
            recordedListProperties.Add(givenListProperty.ToString());

        public void VisitGivenListPropertyFinalisation() =>
            recordedListProperties.Add("Finalisation");

        public void VisitGivenComplexPropertyDeclaration(IVisitedGivenComplexProperty givenComplexProperty)
        {
            // ignore simple properties to keep test simple, just focus on the list visits
        }

        public void VisitGivenComplexPropertyFinalisation()
        {
            // ignore simple properties to keep test simple, just focus on the list visits
        }

        public void VisitGivenSimpleProperty(IVisitedGivenSimpleProperty givenSimpleProperty)
        {
            // ignore simple properties to keep test simple, just focus on the list visits
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
