using System.Collections.Generic;

namespace SampleSystemUnderTest.AnovaCalculator
{
    public interface IGroup
    {
        string Name { get; }
        IEnumerable<double> Floats { get; }
    }
}