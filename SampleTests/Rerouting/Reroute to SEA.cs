using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using System.Linq.Expressions;
using SampleTests;

using SampleSystemUnderTest;
using SampleTests.IgnoreOnGeneration.AnovaCalculator;
using SampleSystemUnderTest.AnovaCalculator;
using SampleTests.IgnoreOnGeneration.Routing;
using SampleSystemUnderTest.Routing;
using SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length;
using SampleSystemUnderTest.VermeulenNearWakeLength;

namespace SampleTests.Rerouting
{
    [TestFixture]
    public class Reroute_To_Sea : SpecificationBase<SpecificationSpecificRoutingService>, ISpecification<SpecificationSpecificRoutingService>
    {
        public override string Description()
        {
            return "Reroute Cargo from HKG - DAL to HKG - SEA";
        }
        
        // arrange
        public override SpecificationSpecificRoutingService Given()
        {
            var routingService = new SpecificationSpecificRoutingService();
            routingService.RerouteFrom_of("DAL");
            routingService.RerouteTo_of("SEA");
            
            {
                var cargo = new SpecificationSpecificCargo();
                cargo.Origin_of("HKG");
                cargo.Destination_of("DAL");
                {
                    var itineraryLeg_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg>();
                    {
                        var itineraryLeg = new SpecificationSpecificItineraryLeg();
                        itineraryLeg.Origin_of("HKG");
                        itineraryLeg.Destination_of("LGB");
                        itineraryLeg_table.Add(itineraryLeg);
                    }
                    {
                        var itineraryLeg = new SpecificationSpecificItineraryLeg();
                        itineraryLeg.Origin_of("LGB");
                        itineraryLeg.Destination_of("DAL");
                        itineraryLeg_table.Add(itineraryLeg);
                    }
                    cargo.ItineraryLeg_table_of(itineraryLeg_table);
                }
                routingService.Cargo_of(cargo);
            }
            
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
                        , new EqualityAssertion<ICargo>(returns => returns.Destination, "SEA")
                    }
                )
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
