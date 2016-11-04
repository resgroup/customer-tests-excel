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
    public class SpecificationSpecificCargoCreationalProperties : ReportsSpecificationSetup { }

    public class SpecificationSpecificCargo : ReportsSpecificationSetup//, IReportsSpecificationSetup//<ICargo>
    {
        readonly Mock<ICargo> _cargo;

        public SpecificationSpecificCargo(IReportsSpecificationSetup creationalProperties)
            : base(creationalProperties)
        {
            _cargo = new Mock<ICargo>();
            _cargo.Setup(m => m.ItineraryLegs).Returns(_legs.Select(l => l.Leg));
        }

        public ICargo Cargo => _cargo.Object;

        public void Origin_of(string origin)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, origin);
            _cargo.Setup(m => m.Origin).Returns(origin);
        }

        public void Destination_of(string destination)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, destination);
            _cargo.Setup(m => m.Destination).Returns(destination);
        }

        readonly List<SpecificationSpecificItineraryLeg> _legs = new List<SpecificationSpecificItineraryLeg>();
        public void ItineraryLeg_of(SpecificationSpecificItineraryLeg leg)
        {
            _classProperties.Add(new ReportSpecificationSetupClass(System.Reflection.MethodBase.GetCurrentMethod().Name, leg, true, _legs.Count));
            _legs.Add(leg);
        }

        public void ItineraryLeg_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg> legs)
        {
            legs.PropertyName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            _classTableProperties.Add(legs);
            foreach (var row in legs.Rows) _legs.Add(row.Properties);
        }

    }
}
