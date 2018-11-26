using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace SampleSystemUnderTest.AnovaCalculator
{
    // based on 
    // https://www.spss-tutorials.com/anova-what-is-it/
    // https://docs.google.com/spreadsheets/d/1YcmEfgdt7LZ-PSBckAQxJJqayBRAjWfGxdUTM0sw70o/edit#gid=0
    public class AnovaCalculator
    {
        readonly string variableDescription;
        readonly IEnumerable<IAnovaGroup> groups;

        public AnovaCalculator(string variableDescription, IEnumerable<IAnovaGroup> groups)
        {
            this.variableDescription = variableDescription;
            this.groups = groups;
        }

        public IAnovaResult Calculate()
        {
            var allValues = groups.SelectMany(g => g.Values);

            double meanOfAllObservations = allValues.Average();

            return CalculateSecondPass(
                groups.Count(), 
                allValues.Count(),
                groups.Select(g => new AnovaGroupCalculator(g).Calculate(meanOfAllObservations)));
        }

        IAnovaResult CalculateSecondPass(int numberOfGroups, int numberOfObservations, IEnumerable<IAnovaGroupResult> calculatedGroups)
        {
            var anovaResult = new AnovaResult();

            anovaResult.VariableDescription = variableDescription;

            anovaResult.SS_Between = calculatedGroups.Sum(g => g.squaresBetween);
            anovaResult.DF_Between = numberOfGroups - 1;
            anovaResult.MS_Between = anovaResult.SS_Between / anovaResult.DF_Between;

            anovaResult.SS_Within = calculatedGroups.Sum(g => SquaresWithin(g.group, g.mean));
            anovaResult.DF_Within = numberOfObservations - numberOfGroups;
            anovaResult.MS_Within = anovaResult.SS_Within / anovaResult.DF_Within;

            anovaResult.F = anovaResult.MS_Between / anovaResult.MS_Within;
            anovaResult.StatisticalSignificance = new FisherSnedecor(anovaResult.DF_Between, anovaResult.DF_Within).CumulativeDistribution(anovaResult.F);
            anovaResult.EffectSize = anovaResult.SS_Between / (anovaResult.SS_Between + anovaResult.SS_Within);

            return anovaResult;
        }

        double SquaresWithin(IAnovaGroup group, double groupMean) =>
            group.Values.Sum(value => Pow(value - groupMean, 2));
    }
}
