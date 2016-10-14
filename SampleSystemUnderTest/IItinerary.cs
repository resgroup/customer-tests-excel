using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest
{
    public interface IItinerary
    {
        IEnumerable<ILeg> Legs { get; }
    }
}
