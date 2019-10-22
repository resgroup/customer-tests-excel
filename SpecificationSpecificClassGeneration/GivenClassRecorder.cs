using System;
using System.Collections.Generic;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassRecorder : IExcelToCodeVisitor
    {
        readonly List<GivenClass> classes = new List<GivenClass>();
        public IReadOnlyList<GivenClass> Classes =>
            classes;

        List<GivenClassProperty> currentClassProperties;

        public void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty)
        {
            currentClassProperties = new List<GivenClassProperty>();
        }

        public void VisitGivenComplexPropertyFinalisation()
        {
            classes.Add(new GivenClass(currentClassProperties));
        }

        public void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty)
        {
            currentClassProperties.Add(new GivenClassProperty(givenSimpleProperty.PropertyOrFunctionName, givenSimpleProperty.ExcelPropertyType));
        }

        public void VisitGivenListPropertyDeclaration(IGivenListProperty givenListProperty)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenListPropertyFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyCellDeclaration(TableHeader tableHeader, uint row, uint column)
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyCellFinalisation()
        {
            // ignore to keep test simple, just focus on the complex visits
        }

        public void VisitGivenTablePropertyDeclaration(IEnumerable<TableHeader> tableHeaders)
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
