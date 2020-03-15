using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class GivenClassRecorder2 : TestBase
    {
        [Test]
        public void GivenClassRecorderIgnoresSimplePropertyDuplicates()
        {
            var visitRecorder = new GivenClassRecorder();

            var givenComplexProperty = new GivenComplexProperty(
                    "Leg",
                    "Leg"
                );

            var givenSimpleProperty = new GivenSimpleProperty(
                    "Origin of",
                    "Origin",
                    ExcelPropertyType.String
                );

            visitRecorder.VisitGivenComplexPropertyDeclaration(givenComplexProperty);
            visitRecorder.VisitGivenSimpleProperty(givenSimpleProperty);
            visitRecorder.VisitGivenSimpleProperty(givenSimpleProperty);
            visitRecorder.VisitGivenComplexPropertyFinalisation();

            Assert.AreEqual(1, visitRecorder.Classes.Single().Properties.Count());
        }

    }
}
