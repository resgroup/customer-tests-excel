using System.Collections.Generic;

namespace SampleSystemUnderTest.AnovaCalculator
{
    public class AnovaGroupResult : IAnovaGroupResult
    {
        public IAnovaGroup group { get; set; }
        public double mean { get; set; }
        public double varianceFromAnovaMean { get; set; }
        public double squaresBetween { get; set; }
    }
}