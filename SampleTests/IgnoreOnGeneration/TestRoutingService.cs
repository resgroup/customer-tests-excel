using RES.Specification;
using SampleSystemUnderTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTestsRerouting
{
    public class SpecificationSpecificRoutingServiceCreationalProperties : ReportsSpecificationSetup
    {
        internal string _rerouteFrom { get; private set;  }
        public SpecificationSpecificRoutingServiceCreationalProperties RerouteFrom_of(string rerouteFrom)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, rerouteFrom);
            _rerouteFrom = rerouteFrom;
            return this;
        }

        internal string _rerouteTo { get; private set; }
        public SpecificationSpecificRoutingServiceCreationalProperties RerouteTo_of(string rerouteTo)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, rerouteTo);
            _rerouteTo = rerouteTo;
            return this;
        }

        internal SpecificationSpecificCargo _cargo { get; private set; }
        public SpecificationSpecificRoutingServiceCreationalProperties Cargo_of(SpecificationSpecificCargo cargo)
        {
            _classProperties.Add(new ReportSpecificationSetupClass(System.Reflection.MethodBase.GetCurrentMethod().Name, cargo));
            _cargo = cargo;
            return this;
        }
    }

    public class SpecificationSpecificRoutingService : ReportsSpecificationSetup
    {
        private readonly RoutingService _routingService;
        private ICargo _reroutedCargo;

        public SpecificationSpecificRoutingService(SpecificationSpecificRoutingServiceCreationalProperties creationalProperties)
            : base(creationalProperties)
        {
            _routingService = new RoutingService(rerouteFrom: creationalProperties._rerouteFrom, rerouteTo: creationalProperties._rerouteTo, cargo: creationalProperties._cargo.Cargo); 
        }

        public ICargo Returns => _reroutedCargo;

        public ICargo Reroute()
        {
            _reroutedCargo = _routingService.Reroute();
            return _reroutedCargo;
        }

    }
}
