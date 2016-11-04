using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest
{
    public interface ICargo
    {
        string Origin { get; }
        string Destination { get; }
        IEnumerable<ILeg> ItineraryLegs { get; }
    }
}
