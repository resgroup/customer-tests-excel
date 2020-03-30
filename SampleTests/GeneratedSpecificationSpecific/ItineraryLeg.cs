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
using SampleTests.IgnoreOnGeneration.NameConversions;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificItineraryLeg : ReportsSpecificationSetup
    {
        readonly Mock<IItineraryLeg> itineraryLeg;

        public IItineraryLeg ItineraryLeg =>
            itineraryLeg.Object;



        public SpecificationSpecificItineraryLeg()
        {
            itineraryLeg = new Mock<IItineraryLeg>();


        }



        internal SpecificationSpecificItineraryLeg Origin_of(String origin)
        {
            valueProperties.Add(GetCurrentMethod(), origin);

            itineraryLeg.Setup(m => m.Origin).Returns(origin);

            return this;
        }

        internal SpecificationSpecificItineraryLeg Destination_of(String destination)
        {
            valueProperties.Add(GetCurrentMethod(), destination);

            itineraryLeg.Setup(m => m.Destination).Returns(destination);

            return this;
        }





    }
}
