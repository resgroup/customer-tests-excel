using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest
{
    public class Leg : ILeg
    {
        public Leg(string origin, string destination)
        {
            Origin = origin;
            Destination = destination;
        }

        public string Origin { get; }
        public string Destination { get; }
    }
}
