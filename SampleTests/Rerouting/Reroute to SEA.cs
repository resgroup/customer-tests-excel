using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using CustomerTestsExcel.Assertions;
using System.Linq.Expressions;
using SampleTests;

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
                    var ItineraryLeg = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg>();
                    {
                        var itineraryLeg_Row = new SpecificationSpecificItineraryLeg();
                        itineraryLeg_Row.Origin_of("HKG");
                        itineraryLeg_Row.Destination_of("LGB");
                        ItineraryLeg.Add(itineraryLeg_Row);
                    }
                    {
                        var itineraryLeg_Row = new SpecificationSpecificItineraryLeg();
                        itineraryLeg_Row.Origin_of("LGB");
                        itineraryLeg_Row.Destination_of("DAL");
                        ItineraryLeg.Add(itineraryLeg_Row);
                    }
                    cargo.ItineraryLeg_table_of(ItineraryLeg);
                }
                routingService.Cargo_of(cargo);
            }
            
            return routingService;
        }
        
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
