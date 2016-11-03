using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RES.Specification;
using System.Linq.Expressions;

namespace SampleTestsRerouting
{
    [TestFixture]
    public class Reroute_to_SEA : SpecificationBase<SpecificationSpecificRoutingService>, ISpecification<SpecificationSpecificRoutingService>
    {
        public override string Description()
        {
            return "Reroute to SEA";
        }

        public override string TrunkRelativePath()
        {
            return "SampleTests";
        }

        // arrange
        public override SpecificationSpecificRoutingService Given()
        {
            var routingServiceCreationalProperties = new SpecificationSpecificRoutingServiceCreationalProperties();
            routingServiceCreationalProperties.Reroute_From_of("DAL");
            routingServiceCreationalProperties.Reroute_To_of("SEA");

            var routingService_CargoCreationalProperties = new SpecificationSpecificCargoCreationalProperties();
            routingService_CargoCreationalProperties.Origin_of("HKG");
            routingService_CargoCreationalProperties.Destination_of("DAL");
            var routingService_Cargo_ItineraryLeg_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg>();
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg0CreationalProperties = new SpecificationSpecificItineraryLegCreationalProperties();
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg0CreationalProperties.Origin_of("HKG");
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg0CreationalProperties.Destination_of("LGB");
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg0 = new SpecificationSpecificItineraryLeg(routingService_Cargo_ItineraryLeg_table_ItineraryLeg0CreationalProperties);
            routingService_Cargo_ItineraryLeg_table.Add(routingService_Cargo_ItineraryLeg_table_ItineraryLeg0);
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg1CreationalProperties = new SpecificationSpecificItineraryLegCreationalProperties();
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg1CreationalProperties.Origin_of("LGB");
            routingService_Cargo_ItineraryLeg_table_ItineraryLeg1CreationalProperties.Destination_of("DAL");
            var routingService_Cargo_ItineraryLeg_table_ItineraryLeg1 = new SpecificationSpecificItineraryLeg(routingService_Cargo_ItineraryLeg_table_ItineraryLeg1CreationalProperties);
            routingService_Cargo_ItineraryLeg_table.Add(routingService_Cargo_ItineraryLeg_table_ItineraryLeg1);
            routingService_CargoCreationalProperties.ItineraryLeg_table_of(routingService_Cargo_ItineraryLeg_table);
            var routingService_Cargo = new SpecificationSpecificCargo(routingService_CargoCreationalProperties);
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
                 new ParentAssertion<SpecificationSpecificRoutingService, Cargo>
                (
                    rerouted_Cargo => rerouted_Cargo.Rerouted_Cargo,
                    new List<IAssertion<Cargo>>
                    {
                         new ParentAssertion<Cargo, HKG>
                        (
                            origin_of => origin_of.Origin_of,
                            new List<IAssertion<HKG>>
                            {
                            }
                        )
                        ,new ParentAssertion<Cargo, SEA>
                        (
                            destination_of => destination_of.Destination_of,
                            new List<IAssertion<SEA>>
                            {
                            }
                        )
                    }
                )
            };
        }
    }
}
