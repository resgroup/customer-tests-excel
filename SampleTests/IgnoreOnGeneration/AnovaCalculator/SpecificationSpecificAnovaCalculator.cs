using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel;
using SampleSystemUnderTest.AnovaCalculator;

namespace SampleTests.IgnoreOnGeneration.AnovaCalculator
{
    public class SpecificationSpecificAnovaCalculator : ReportsSpecificationSetup
    {
        internal string variableDescription { get; private set; }
        readonly List<SpecificationSpecificGroup> groups = new List<SpecificationSpecificGroup>();
        public IAnovaResult AnovaResult { get; private set; }

        internal SpecificationSpecificAnovaCalculator VariableDescription_of(string variableDescription)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, variableDescription);

            this.variableDescription = variableDescription;

            return this;
        }

        internal SpecificationSpecificAnovaCalculator Groups_of(SpecificationSpecificGroup group)
        {
            _classProperties.Add(new ReportSpecificationSetupClass(System.Reflection.MethodBase.GetCurrentMethod().Name, group, true, groups.Count));

            groups.Add(group);

            return this;
        }

        internal void Calculate()
        {
            AnovaResult = new SampleSystemUnderTest.AnovaCalculator.AnovaCalculator(variableDescription, groups.Select(g => g.AnovaGroup)).Calculate();
        }
    }
}