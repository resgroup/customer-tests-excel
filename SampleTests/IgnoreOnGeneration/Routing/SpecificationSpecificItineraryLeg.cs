using Moq;
using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleSystemUnderTest.Routing;

namespace SampleTests.IgnoreOnGeneration.Routing
{
    public class SpecificationSpecificItineraryLeg : ReportsSpecificationSetup
    {
        readonly Mock<IItineraryLeg> _leg;

        public SpecificationSpecificItineraryLeg()
            : base()
        {
            _leg = new Mock<IItineraryLeg>();
        }

        public IItineraryLeg ItineraryLeg => _leg.Object;

        public SpecificationSpecificItineraryLeg Origin_of(string origin)
        {
            valueProperties.Add(GetCurrentMethod(), origin);
            _leg.Setup(m => m.Origin).Returns(origin);
            return this;
        }

        public SpecificationSpecificItineraryLeg Destination_of(string destination)
        {
            valueProperties.Add(GetCurrentMethod(), destination);
            _leg.Setup(m => m.Destination).Returns(destination);
            return this;
        }
    }
}
