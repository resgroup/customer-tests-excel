using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleSystemUnderTest.Calculator;
using SampleSystemUnderTest.CustomProperties;

namespace SampleTests.GeneratedSpecificationSpecific
{
    // This is a generated class that matches the root class of an excel test.

    // It should create all the things you need for the 'Given' section of the
    // test, but you will need to add a method for the 'When' section, and a 
    // property for the 'Assert' section.
    // You can see an example at the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/IgnoreOnGeneration/Vermeulen%20Near%20Wake%20Length/SpecificationSpecificVermeulenNearWakeLengthCalculatorPartial.cs
    // The 'Calculate()' method matches up with 'When', 'Calculate' from the 
    // test, and the 'VermeulenNearWakeLengths' property matches up with the 
    // 'Assert', 'VermeulenNearWakeLengths' from the test.
    // You can see the associated Excel test on the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/ExcelTests/Vermeulen%20Near%20Wake%20Length.xlsx

    // Custom classes should go under a directory called 'IgnoreOnGeneration'.
    // If the custom class filename is the same as this one (SpecificationSpecificUnmatchedClass),
    // then it will be used instead of this function. If it is called something else,
    // say SpecificationSpecificUnmatchedClassPartial, then this class will remain, and
    // the custom class can add to it.

    public partial class SpecificationSpecificUnmatchedClass : ReportsSpecificationSetup
    {

        public SpecificationSpecificClassWithCustomProperty ClassWithCustomProperty { get; private set; }

        public SpecificationSpecificUnmatchedClass()
        {

        }

        internal SpecificationSpecificUnmatchedClass ClassWithCustomProperty_of(SpecificationSpecificClassWithCustomProperty classWithCustomProperty)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), classWithCustomProperty));

            this.ClassWithCustomProperty = classWithCustomProperty;

            return this;
        }

    }
}
