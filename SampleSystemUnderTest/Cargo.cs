using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest
{
    public class Cargo : ICargo
    {

        public Cargo(string origin, string destination, IItinerary itinerary)
        {
            Contract.Requires(itinerary != null);

            Origin = origin;
            Destination = destination;
            Itinerary = itinerary;
        }

        public string Destination { get; }
        public string Origin { get; }
        public IItinerary Itinerary { get; }
    }
}
