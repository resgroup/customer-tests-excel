using System;
using System.Collections.Generic;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    public class GivenListPropertyVisitRecorder : IExcelToCodeVisitor
    {
        readonly List<string> recordedListProperties = new List<string>();
        public IReadOnlyList<string> RecordedListProperties =>
            recordedListProperties;

        public void VisitGivenListPropertyDeclaration(IGivenListProperty givenListProperty) =>
            recordedListProperties.Add(givenListProperty.ToString());

        public void VisitGivenListPropertyFinalisation() =>
            recordedListProperties.Add("Finalisation");

        public void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty)
        {
            // ignore simple properties to keep test simple, just focus on the list visits
        }

        public void VisitGivenComplexPropertyFinalisation()
        {
            // ignore simple properties to keep test simple, just focus on the list visits
        }

        public void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty)
        {
            // ignore simple properties to keep test simple, just focus on the list visits
        }
    }
}
