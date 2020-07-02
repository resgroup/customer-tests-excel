using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassRecorder : IExcelToCodeVisitor
    {
        readonly List<GivenClassMutable> classes = new List<GivenClassMutable>();
        public IReadOnlyList<GivenClass> Classes =>
            classes.Select(
                mutableClass =>
                    mutableClass.CreateGivenClass()
                    ).ToList();

        readonly Stack<GivenClassMutable> currentClasses = new Stack<GivenClassMutable>();

        public void VisitGivenRootClassDeclaration(string className) =>
            CreateOrActivateCurrentClass(className, true);

        public void VisitGivenRootClassFinalisation() =>
            FinishCurrentClass();

        public void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty)
        {
            if (currentClasses.Any())
                currentClasses.Peek().AddComplexProperty(givenComplexProperty);

            CreateOrActivateCurrentClass(givenComplexProperty.ClassName);
        }

        public void VisitGivenComplexPropertyFinalisation() =>
            FinishCurrentClass();

        public void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty)
        {
            if (currentClasses.Any())
                currentClasses.Peek().AddSimpleProperty(givenSimpleProperty);
        }

        public void VisitGivenFunction(IGivenFunction givenFunction)
        {
            if (currentClasses.Any())
                currentClasses.Peek().AddFunction(givenFunction);
        }

        public void VisitGivenListPropertyDeclaration(IGivenListProperty givenListProperty)
        {
            if (currentClasses.Any())
                currentClasses.Peek().AddListProperty(givenListProperty);

            CreateOrActivateCurrentClass(givenListProperty.ClassName);
        }

        public void VisitGivenListPropertyFinalisation() =>
            FinishCurrentClass();

        public void VisitGivenTablePropertyDeclaration(IGivenTableProperty givenTableProperty, IEnumerable<TableHeader> tableHeaders)
        {
            if (currentClasses.Any())
                currentClasses.Peek().AddTableProperty(givenTableProperty);

            CreateOrActivateCurrentClass(givenTableProperty.ClassName);
        }

        public void VisitGivenTablePropertyFinalisation() =>
            FinishCurrentClass();

        public void VisitGivenTablePropertyRowDeclaration(uint row)
        {
            // don't need to do anything special with table rows
        }

        public void VisitGivenTablePropertyRowFinalisation()
        {
            // don't need to do anything special with table rows
        }

        public void VisitGivenTablePropertyCellDeclaration(TableHeader tableHeader, uint row, uint column)
        {
            // don't need to do anything special with table cells
        }

        public void VisitGivenTablePropertyCellFinalisation()
        {
            // don't need to do anything special with table cells
        }

        void CreateOrActivateCurrentClass(string className, bool isRootClass = false)
        {
            // The same class might be set up twice (2 items in a list for example). 
            // So want to put it back on the stack if so.
            // This doesn't consider classes that have a property of their own class
            // but we could handle this is we wanted to by looking at the stack for
            // the class and adding it again to the top of the stack. I think.
            if (classes.Any(c => c.Name == className))
                currentClasses.Push(
                    classes.First(
                        c => 
                            c.Name == className 
                            && c.IsRootClass == isRootClass)
                    );
            else
                currentClasses.Push(new GivenClassMutable(className, isRootClass));
        }

        void FinishCurrentClass()
        {
            // The class might already exist in our list, in which case don't add it twice.
            // See CreateOrActivateCurrentClass
            // There is probably a neater way of doing this.
            var currentClass = currentClasses.Pop();

            if (!classes.Any(c => c.Name == currentClass.Name))
                classes.Add(currentClass);
        }
    }
}
