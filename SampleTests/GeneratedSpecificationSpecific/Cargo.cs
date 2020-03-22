using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleSystemUnderTest;
using SampleTests.IgnoreOnGeneration.AnovaCalculator;
using SampleSystemUnderTest.AnovaCalculator;
using SampleTests.IgnoreOnGeneration.Routing;
using SampleSystemUnderTest.Routing;
using SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.IgnoreOnGeneration.Calculator;
using SampleSystemUnderTest.Calculator;
using SampleTests.IgnoreOnGeneration.NameConversions;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificCargo : ReportsSpecificationSetup
    {
        readonly Mock<ICargo> cargo;

        public ICargo Cargo =>
            cargo.Object;

        readonly List<SpecificationSpecificItineraryLeg> itineraryLegs = new List<SpecificationSpecificItineraryLeg>();

        public SpecificationSpecificCargo()
        {
            cargo = new Mock<ICargo>();

            cargo.Setup(m => m.ItineraryLeg).Returns(itineraryLegs.Select(l => l.ItineraryLeg));
        }



        internal SpecificationSpecificCargo Origin_of(String origin)
        {
            valueProperties.Add(GetCurrentMethod(), origin);

            cargo.Setup(m => m.Origin).Returns(origin);

            return this;
        }

        internal SpecificationSpecificCargo Destination_of(String destination)
        {
            valueProperties.Add(GetCurrentMethod(), destination);

            cargo.Setup(m => m.Destination).Returns(destination);

            return this;
        }




        internal SpecificationSpecificCargo ItineraryLeg_of(SpecificationSpecificItineraryLeg itineraryLeg)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), itineraryLeg));

            this.itineraryLegs.Add(itineraryLeg);

            return this;
        }

        internal SpecificationSpecificCargo ItineraryLeg_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg> itineraryLegs)
        {
            itineraryLegs.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(itineraryLegs);

            foreach (var row in itineraryLegs.Rows)
                this.itineraryLegs.Add(row.Properties);

            return this;
        }
    }
}
