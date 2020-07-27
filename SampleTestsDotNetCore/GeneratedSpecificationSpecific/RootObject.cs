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
    // If the custom class filename is the same as this one (SpecificationSpecificRootObject),
    // then it will be used instead of this function. If it is called something else,
    // say SpecificationSpecificRootObjectPartial, then this class will remain, and
    // the custom class can add to it.

    public partial class SpecificationSpecificRootObject : ReportsSpecificationSetup
    {
        public Double? NullableInt { get; private set; }
        public Double Number { get; private set; }
        public String NullableString { get; private set; }

        public SpecificationSpecificRootObject()
        {

        }

        internal SpecificationSpecificRootObject NullableInt_of(Double? nullableInt)
        {
            AddValueProperty(GetCurrentMethod(), nullableInt);

            this.NullableInt = nullableInt;

            return this;
        }

        internal SpecificationSpecificRootObject Number_of(Double number)
        {
            AddValueProperty(GetCurrentMethod(), number);

            this.Number = number;

            return this;
        }

        internal SpecificationSpecificRootObject NullableString_of(String nullableString)
        {
            AddValueProperty(GetCurrentMethod(), nullableString);

            this.NullableString = nullableString;

            return this;
        }

    }
}
