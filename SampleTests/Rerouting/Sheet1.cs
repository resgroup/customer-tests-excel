using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RES.Specification;
using System.Linq.Expressions;
using .Specification.Setup;
using .Calculation.Base;
using .Base;

namespace .Specification.Rerouting
{
    [TestFixture]
    public class Sheet1 : SpecificationBase<SpecificationSpecificRoutingService>, ISpecification<SpecificationSpecificRoutingService>
    {
        public override string Description()
        {
            return "Reroute to SEA";
        }

        public override string TrunkRelativePath()
        {
            return @"";
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
            routingService_CargoCreationalProperties.Itinerary_of("Itinerary");
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
