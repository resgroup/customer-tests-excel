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
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/Setup/Vermeulen%20Near%20Wake%20Length/SpecificationSpecificVermeulenNearWakeLengthCalculatorPartial.cs
    // The 'Calculate()' method matches up with 'When', 'Calculate' from the 
    // test, and the 'VermeulenNearWakeLengths' property matches up with the 
    // 'Assert', 'VermeulenNearWakeLengths' from the test.
    // You can see the associated Excel test on the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/ExcelTests/Vermeulen%20Near%20Wake%20Length.xlsx

    // Custom classes should go in the 'Setup' folder.
    // If the custom class filename is 'SpecificationSpecificRoutingServiceOverride.cs',
    // then it will be used instead of this function. If it is called something else,
    // say 'SpecificationSpecificRoutingServicePartial.cs', then this class will remain, and
    // the custom class can add to it.

    public partial class SpecificationSpecificRoutingService : ReportsSpecificationSetup
    {
        public String RerouteFrom { get; private set; }
        public String RerouteTo { get; private set; }

        public SpecificationSpecificCargo Cargo { get; private set; }

        public SpecificationSpecificRoutingService()
        {

        }

        internal SpecificationSpecificRoutingService RerouteFrom_of(String rerouteFrom)
        {
            AddValueProperty(GetCurrentMethod(), rerouteFrom);

            this.RerouteFrom = rerouteFrom;

            return this;
        }

        internal SpecificationSpecificRoutingService RerouteTo_of(String rerouteTo)
        {
            AddValueProperty(GetCurrentMethod(), rerouteTo);

            this.RerouteTo = rerouteTo;

            return this;
        }

        internal SpecificationSpecificRoutingService Cargo_of(SpecificationSpecificCargo cargo)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), cargo));

            this.Cargo = cargo;

            return this;
        }

    }
}
