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
    // A class with the name A_Table was found in the Excel tests, but
    // no matching interface could be found in the assembliesUnderTest that were
    // specified on the command line parameters to GenerateCodeFromExcelTest. This file
    // is here so that the generated code can compile, to provide an example of
    // the custom file that you need to create, and point out anything that would stop
    // it matching an interface in your system under test.
    
    // The easiest thing to do is to make the names in the Excel match the names of your
    // Interfaces and their properties, and let the framework generate the Specification 
    // Specific setup classes for you. However you can write custom ones if you would
    // prefer, and they do allow you do some more esoteric things, such as instantiating
    // classes instead of mocking interfaces.
    // You can see examples of excel tests and matching interfaces in the SampleTests
    // and SampleSystemUnderTest folders of the framework repository
    // - https://github.com/resgroup/customer-tests-excel/tree/master/SampleTests
    // - https://github.com/resgroup/customer-tests-excel/tree/master/SampleSystemUnderTest

    // Custom classes should go under a directory called 'IgnoreOnGeneration'.
    // If the custom class filename is the same as this one (SpecificationSpecificA_Table),
    // then it will be used instead of this function. If it is called something else,
    // say SpecificationSpecificA_TablePartial, then this class will remain, and
    // the custom class can add to it.

    // Please see VermeulenNearWakeLengthInput.cs for an example of setting up simple and
    // complex properties
    // - https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/GeneratedSpecificationSpecific/VermeulenNearWakeLengthInput.cs
    // Please see Group.cs for an example of setting up list / table properties
    // - https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/GeneratedSpecificationSpecific/Cargo.cs

    public partial class SpecificationSpecificA_Table : ReportsSpecificationSetup
    {
        public String A_Property { get; private set; }





        public SpecificationSpecificA_Table()
        {

        }



        internal SpecificationSpecificA_Table A_Property_of(String a_Property)
        {
            AddValueProperty(GetCurrentMethod(), a_Property);

            this.A_Property = a_Property;

            return this;
        }





    }
}
