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
using SampleTests.IgnoreOnGeneration.Calculator;
using SampleSystemUnderTest.Calculator;
using SampleTests.IgnoreOnGeneration.NameConversions;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificRoutingService : ReportsSpecificationSetup
    {
        public string rerouteFrom { get; private set; }
        public string rerouteTo { get; private set; }

        public SpecificationSpecificCargo cargo { get; private set; }



        public SpecificationSpecificRoutingService()
        {

        }

        internal SpecificationSpecificRoutingService RerouteFrom_of(String rerouteFrom)
        {
            valueProperties.Add(GetCurrentMethod(), rerouteFrom);

            this.rerouteFrom = rerouteFrom;

            return this;
        }

        internal SpecificationSpecificRoutingService RerouteTo_of(String rerouteTo)
        {
            valueProperties.Add(GetCurrentMethod(), rerouteTo);

            this.rerouteTo = rerouteTo;

            return this;
        }


        internal SpecificationSpecificRoutingService Cargo_of(SpecificationSpecificCargo cargo)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), cargo));

            this.cargo = cargo;

            return this;
        }



    }
}
