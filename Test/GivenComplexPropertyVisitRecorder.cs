using System;
using System.Collections.Generic;
using System.Text;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
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
    }
}
