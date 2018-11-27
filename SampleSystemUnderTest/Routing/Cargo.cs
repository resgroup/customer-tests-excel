using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest.Routing
{
    public class Cargo : ICargo
    {

        public Cargo(string origin, string destination, IEnumerable<ILeg> itineraryLegs)
        {
            Contract.Requires(itineraryLegs != null);

            Origin = origin;
            Destination = destination;
            ItineraryLegs = itineraryLegs;
        }

        public string Destination { get; }
        public string Origin { get; }
        public IEnumerable<ILeg> ItineraryLegs { get; }
    }
}
