using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassMutable
    {
        public string Name { get; set; }
        public List<GivenClassProperty> Properties { get; set; } = new List<GivenClassProperty>();
    }

    public class GivenClassRecorder : IExcelToCodeVisitor
    {
        readonly List<GivenClass> classes = new List<GivenClass>();
        public IReadOnlyList<GivenClass> Classes =>
            classes;

        readonly Stack<GivenClassMutable> currentClasses = new Stack<GivenClassMutable>();

        public void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty)
        {
            if (currentClasses.Any())
            {
                currentClasses.Peek().Properties.Add(
                    new GivenClassProperty(
                        givenComplexProperty.PropertyName,
                        ExcelPropertyType.Object,
                        givenComplexProperty.ClassName));
            }

            var nextClass = new GivenClassMutable();
            nextClass.Name = givenComplexProperty.ClassName;

            currentClasses.Push(nextClass);
        }

        public void VisitGivenComplexPropertyFinalisation()
        {
            var finishedClass = currentClasses.Pop();
            classes.Add(new GivenClass(finishedClass.Name, finishedClass.Properties));
        }

        public void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty)
        {
            currentClasses.Peek().Properties.Add(new GivenClassProperty(givenSimpleProperty.PropertyOrFunctionName, givenSimpleProperty.ExcelPropertyType));
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
