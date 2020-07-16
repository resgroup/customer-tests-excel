using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    public class GivenSimplePropertyVisitRecorder : IExcelToCodeVisitor
    {
        readonly List<IVisitedGivenSimpleProperty> recordedSimpleProperties = new List<IVisitedGivenSimpleProperty>();
        public IReadOnlyList<IVisitedGivenSimpleProperty> RecordedSimpleProperties =>
            recordedSimpleProperties;

        public void VisitGivenRootClassDeclaration(string className)
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenRootClassFinalisation()
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenSimpleProperty(IVisitedGivenSimpleProperty givenSimpleProperty) =>
            recordedSimpleProperties.Add(givenSimpleProperty);

        public void VisitGivenListPropertyDeclaration(IVisitedGivenListProperty givenListProperty)
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenListPropertyFinalisation()
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenComplexPropertyDeclaration(IVisitedGivenComplexProperty givenComplexProperty)
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenComplexPropertyFinalisation()
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
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
