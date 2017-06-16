using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using System.Linq.Expressions;

using SampleSystemUnderTest;

namespace SampleTestsRerouting
{
    [TestFixture]
    public class Reroute_To_Sea : SpecificationBase<SpecificationSpecificRoutingService>, ISpecification<SpecificationSpecificRoutingService>
    {
        public override string Description()
        {
            return "Reroute Cargo from HKG - DAL to HKG - SEA";
        }

        public override string TrunkRelativePath()
        {
            return "SampleTests";
        }

        // arrange
        public override SpecificationSpecificRoutingService Given()
        {
            var routingServiceCreationalProperties = new SpecificationSpecificRoutingServiceCreationalProperties();
            routingServiceCreationalProperties.RerouteFrom_of("DAL");
            routingServiceCreationalProperties.RerouteTo_of("SEA");

            {
                var cargoCreationalProperties = new SpecificationSpecificCargoCreationalProperties();
                var cargo = new SpecificationSpecificCargo(cargoCreationalProperties);
                cargo.Origin_of("HKG");
                cargo.Destination_of("DAL");
                {
                    var itineraryLeg_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg>();
                    {
                        var itineraryLegCreationalProperties = new SpecificationSpecificItineraryLegCreationalProperties();
                        var itineraryLeg = new SpecificationSpecificItineraryLeg(itineraryLegCreationalProperties);
                        itineraryLeg.Origin_of("HKG");
                        itineraryLeg.Destination_of("LGB");
                        itineraryLeg_table.Add(itineraryLeg);
                    }
                    {
                        var itineraryLegCreationalProperties = new SpecificationSpecificItineraryLegCreationalProperties();
                        var itineraryLeg = new SpecificationSpecificItineraryLeg(itineraryLegCreationalProperties);
                        itineraryLeg.Origin_of("LGB");
                        itineraryLeg.Destination_of("DAL");
                        itineraryLeg_table.Add(itineraryLeg);
                    }
                    cargo.ItineraryLeg_table_of(itineraryLeg_table);
                }
                routingServiceCreationalProperties.Cargo_of(cargo);
            }
            var routingService = new SpecificationSpecificRoutingService(routingServiceCreationalProperties);

            return routingService;
        }

        // act
        public override string When(SpecificationSpecificRoutingService routingService)
        {
            routingService.Reroute();
            return "Reroute";
        }

        public override IEnumerable<IAssertion<SpecificationSpecificRoutingService>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificRoutingService>>
            {
                 new ParentAssertion<SpecificationSpecificRoutingService, ICargo>
                (
                    returns => returns.Returns,
                    new List<IAssertion<ICargo>>
                    {
                         new EqualityAssertion<ICargo>(returns => returns.Origin, "HKG")
                        ,new EqualityAssertion<ICargo>(returns => returns.Destination, "SEA")
                    }
                )
            };
        }
    }
}
