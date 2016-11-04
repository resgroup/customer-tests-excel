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
        public void RerouteFrom_of(string rerouteFrom)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, rerouteFrom);
            _rerouteFrom = rerouteFrom;
        }

        internal string _rerouteTo { get; private set; }
        public void RerouteTo_of(string rerouteTo)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, rerouteTo);
            _rerouteTo = rerouteTo;
        }

        internal SpecificationSpecificCargo _cargo { get; private set; }
        public void Cargo_of(SpecificationSpecificCargo cargo)
        {
            _classProperties.Add(new ReportSpecificationSetupClass(System.Reflection.MethodBase.GetCurrentMethod().Name, cargo));
            _cargo = cargo;
        }
    }

    public class SpecificationSpecificRoutingService : RoutingService//, IReportsSpecificationSetup
    {
        public SpecificationSpecificRoutingService(SpecificationSpecificRoutingServiceCreationalProperties creationalProperties)
            : base(rerouteFrom: creationalProperties._rerouteFrom, rerouteTo: creationalProperties._rerouteTo, cargo: creationalProperties._cargo.Cargo ) { }
    }
}
