using CustomerTestsExcel;
using static System.Reflection.MethodBase;
using SampleSystemUnderTest;
using SampleSystemUnderTest.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleTests.GeneratedSpecificationSpecific;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificRoutingService : ReportsSpecificationSetup
    {
        ICargo reroutedCargo;

        public void Reroute()
        {
            reroutedCargo = 
                new RoutingService(
                    rerouteFrom: RerouteFrom, 
                    rerouteTo: RerouteTo, 
                    cargo: this.Cargo.Cargo
                ).Reroute();
        }

        public ICargo Returns => 
            reroutedCargo;
    }
}
