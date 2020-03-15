using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest.Routing
{
    public interface IItineraryLeg
    {
        string Origin { get; }
        string Destination { get; }
    }
}
