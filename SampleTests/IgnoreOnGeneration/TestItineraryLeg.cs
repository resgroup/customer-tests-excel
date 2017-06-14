using Moq;
using RES.Specification;
using SampleSystemUnderTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTestsRerouting
{
    public class SpecificationSpecificItineraryLegCreationalProperties : ReportsSpecificationSetup  { }

    public class SpecificationSpecificItineraryLeg : ReportsSpecificationSetupIBlank//, IReportsSpecificationSetup//<IBlank>
    {
        readonly Mock<ILeg> _leg;

        public SpecificationSpecificItineraryLeg(IReportsSpecificationSetup creationalProperties)
            : base(creationalProperties)
        {
            _leg = new Mock<ILeg>();
        }

        public ILeg Leg => _leg.Object;

        public SpecificationSpecificItineraryLeg Origin_of(string origin)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, origin);
            _leg.Setup(m => m.Origin).Returns(origin);
            return this;
        }

        public SpecificationSpecificItineraryLeg Destination_of(string destination)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, destination);
            _leg.Setup(m => m.Destination).Returns(destination);
            return this;
        }
    }
}
