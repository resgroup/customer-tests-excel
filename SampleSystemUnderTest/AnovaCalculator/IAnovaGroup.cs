using System.Collections.Generic;

namespace SampleSystemUnderTest.AnovaCalculator
{
    public interface IAnovaGroup
    {
        string Name { get; }
        IEnumerable<float> Values { get; }
    }
}