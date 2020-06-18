using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystemUnderTest.Misc
{
    public interface IHasObjectProperty
    {
        IAnything Anything { get; }
    }
}
