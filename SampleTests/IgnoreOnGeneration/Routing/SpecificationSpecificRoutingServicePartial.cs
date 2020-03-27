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
        ICargo _reroutedCargo;

        //public SpecificationSpecificRoutingService()
        //    : base()
        //{
        //}

        //internal string _rerouteFrom { get; private set; }
        //public SpecificationSpecificRoutingService RerouteFrom_of(string rerouteFrom)
        //{
        //    valueProperties.Add(GetCurrentMethod(), rerouteFrom);
        //    _rerouteFrom = rerouteFrom;
        //    return this;
        //}

        //internal string _rerouteTo { get; private set; }
        //public SpecificationSpecificRoutingService RerouteTo_of(string rerouteTo)
        //{
        //    valueProperties.Add(GetCurrentMethod(), rerouteTo);
        //    _rerouteTo = rerouteTo;
        //    return this;
        //}

        //internal SpecificationSpecificCargo _cargo { get; private set; }
        //public SpecificationSpecificRoutingService Cargo_of(SpecificationSpecificCargo cargo)
        //{
        //    classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), cargo));
        //    _cargo = cargo;
        //    return this;
        //}

        public void Reroute()
        {
            _reroutedCargo = 
                new RoutingService(
                    rerouteFrom: rerouteFrom, 
                    rerouteTo: rerouteTo, 
                    cargo: this.cargo.Cargo
                ).Reroute();
        }

        public ICargo Returns => _reroutedCargo;
    }
}
