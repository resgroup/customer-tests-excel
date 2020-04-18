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

namespace SampleTests.GeneratedSpecificationSpecific
{
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
