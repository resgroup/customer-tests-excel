using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RES.Specification;
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

            var routingService_CargoCreationalProperties = new SpecificationSpecificCargoCreationalProperties();
            var routingService_Cargo = new SpecificationSpecificCargo(routingService_CargoCreationalProperties);
            routingService_Cargo.Origin_of("HKG");
            routingService_Cargo.Destination_of("DAL");
            var routingService_Cargo_ItineraryLeg_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg>();
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg0CreationalProperties = new SpecificationSpecificItineraryLegCreationalProperties();
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg0 = new SpecificationSpecificItineraryLeg(routingService_Cargo_ItineraryLeg_table_ItineraryLeg0CreationalProperties);
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg0.Origin_of("HKG");
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg0.Destination_of("LGB");
            routingService_Cargo_ItineraryLeg_table.Add(routingService_Cargo_ItineraryLeg_table_ItineraryLeg0);
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg1CreationalProperties = new SpecificationSpecificItineraryLegCreationalProperties();
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg1 = new SpecificationSpecificItineraryLeg(routingService_Cargo_ItineraryLeg_table_ItineraryLeg1CreationalProperties);
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg1.Origin_of("LGB");
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg1.Destination_of("DAL");
            routingService_Cargo_ItineraryLeg_table.Add(routingService_Cargo_ItineraryLeg_table_ItineraryLeg1);
            routingService_Cargo.ItineraryLeg_table_of(routingService_Cargo_ItineraryLeg_table);
            routingServiceCreationalProperties.Cargo_of(routingService_Cargo);
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
