using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace SampleSystemUnderTest.AnovaCalculator
{
    public class AnovaGroupCalculator
    {
        readonly IGroup group;

        public AnovaGroupCalculator(IGroup group)
        {
            this.group = group;
        }

        public IAnovaGroupResult Calculate(double meanOfAllObservations)
        {
            var result = new AnovaGroupResult();

            result.group = group;
            result.mean = group.Values.Average();
            result.varianceFromAnovaMean = result.mean - meanOfAllObservations;
            result.squaresBetween = Pow(result.varianceFromAnovaMean, 2);

            return result;
        }
    }
}