using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using CustomerTestsExcel.Assertions;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using System.Linq.Expressions;
using SampleTests;
using SampleTests.Setup;

using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleSystemUnderTest.Calculator;
using SampleSystemUnderTest.CustomProperties;
using SampleSystemUnderTest.Misc;

namespace SampleTests.Rerouting
{
    [TestFixture]
    public class Reroute_To_Sea : SpecificationBase<SpecificationSpecificRoutingService>, ISpecification<SpecificationSpecificRoutingService>
    {
        public override string Description()
        {
            return "Reroute Cargo from HKG - DAL to HKG - SEA";
        }
        
        public override SpecificationSpecificRoutingService Given()
        {
            return
                new SpecificationSpecificRoutingService()
                .RerouteFrom_of("DAL")
                .RerouteTo_of("SEA")
                
                .Cargo_of(
                    new SpecificationSpecificCargo()
                    .Origin_of("HKG")
                    .Destination_of("DAL")
                    .ItineraryLeg_table_of(
                        new ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg>()
                        .Add(
                            new SpecificationSpecificItineraryLeg()
                            .Origin_of("HKG")
                            .Destination_of("LGB")
                        )
                        .Add(
                            new SpecificationSpecificItineraryLeg()
                            .Origin_of("LGB")
                            .Destination_of("DAL")
                        )
                    )
                )
            ;
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
