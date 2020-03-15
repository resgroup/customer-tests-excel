using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest.Routing
{
    public interface ILeg
    {
        string Origin { get; }
        string Destination { get; }
    }
}
