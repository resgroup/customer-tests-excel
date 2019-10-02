using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    public class GivenSimplePropertyVisitRecorder : IExcelToCodeVisitor
    {
        readonly List<IGivenSimpleProperty> recordedSimpleProperties = new List<IGivenSimpleProperty>();
        public IReadOnlyList<IGivenSimpleProperty> RecordedSimpleProperties =>
            recordedSimpleProperties;

        public void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty) =>
            recordedSimpleProperties.Add(givenSimpleProperty);

        public void VisitGivenListPropertyDeclaration(IGivenListProperty givenListProperty)
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenListPropertyFinalisation()
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty)
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }

        public void VisitGivenComplexPropertyFinalisation()
        {
            // ignore other properties to keep test simple, just focus on the simple property visits
        }
    }
}
