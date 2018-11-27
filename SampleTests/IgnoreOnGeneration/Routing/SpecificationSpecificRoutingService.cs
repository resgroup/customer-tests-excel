using CustomerTestsExcel;
using SampleSystemUnderTest;
using SampleSystemUnderTest.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTests.IgnoreOnGeneration.Routing
{
    public class SpecificationSpecificRoutingService : ReportsSpecificationSetup
    {
        ICargo _reroutedCargo;

        public SpecificationSpecificRoutingService()
            : base()
        {
        }

        internal string _rerouteFrom { get; private set; }
        public SpecificationSpecificRoutingService RerouteFrom_of(string rerouteFrom)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, rerouteFrom);
            _rerouteFrom = rerouteFrom;
            return this;
        }

        internal string _rerouteTo { get; private set; }
        public SpecificationSpecificRoutingService RerouteTo_of(string rerouteTo)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, rerouteTo);
            _rerouteTo = rerouteTo;
            return this;
        }

        internal SpecificationSpecificCargo _cargo { get; private set; }
        public SpecificationSpecificRoutingService Cargo_of(SpecificationSpecificCargo cargo)
        {
            _classProperties.Add(new ReportSpecificationSetupClass(System.Reflection.MethodBase.GetCurrentMethod().Name, cargo));
            _cargo = cargo;
            return this;
        }

        public void Reroute()
        {
            _reroutedCargo = new RoutingService(rerouteFrom: _rerouteFrom, rerouteTo: _rerouteTo, cargo: _cargo.Cargo).Reroute();
        }

        public ICargo Returns => _reroutedCargo;
    }
}
