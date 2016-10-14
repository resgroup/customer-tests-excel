using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest
{
    public class RoutingService
    {
        public string RerouteFrom { get; }
        public string RerouteTo { get; }
        public ICargo Cargo { get; }

        public RoutingService(string rerouteFrom , string rerouteTo, ICargo cargo)
        {
            Contract.Requires(cargo != null);

            Cargo = cargo;
            RerouteFrom = rerouteFrom;
            RerouteTo = rerouteTo;
        }

        public ICargo Reroute()
        {
            return Cargo;
        }

    }
}
