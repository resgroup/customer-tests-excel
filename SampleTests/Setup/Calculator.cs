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
using SampleSystemUnderTest.Misc;

namespace SampleTests.Setup
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
    // If the custom class filename is the same as this one (SpecificationSpecificCalculator),
    // then it will be used instead of this function. If it is called something else,
    // say SpecificationSpecificCalculatorPartial, then this class will remain, and
    // the custom class can add to it.

    public partial class SpecificationSpecificCalculator : ReportsSpecificationSetup
    {
        public Double FirstValue { get; private set; }
        public Double SecondValue { get; private set; }
        public Operation Operation { get; private set; }

        public SpecificationSpecificCalculator()
        {

        }

        internal SpecificationSpecificCalculator FirstValue_of(Double firstValue)
        {
            AddValueProperty(GetCurrentMethod(), firstValue);

            this.FirstValue = firstValue;

            return this;
        }

        internal SpecificationSpecificCalculator SecondValue_of(Double secondValue)
        {
            AddValueProperty(GetCurrentMethod(), secondValue);

            this.SecondValue = secondValue;

            return this;
        }

        internal SpecificationSpecificCalculator Operation_of(Operation operation)
        {
            AddValueProperty(GetCurrentMethod(), operation);

            this.Operation = operation;

            return this;
        }

    }
}
