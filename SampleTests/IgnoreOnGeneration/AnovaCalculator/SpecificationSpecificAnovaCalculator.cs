using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.AnovaCalculator;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleTests.GeneratedSpecificationSpecific;

namespace SampleTests.IgnoreOnGeneration.AnovaCalculator
{
    public class SpecificationSpecificAnovaCalculator : ReportsSpecificationSetup
    {
        internal string variableDescription { get; private set; }
        readonly List<SpecificationSpecificGroup> groups = new List<SpecificationSpecificGroup>();
        public IAnovaResult AnovaResult { get; private set; }

        internal SpecificationSpecificAnovaCalculator VariableDescription_of(string variableDescription)
        {
            valueProperties.Add(GetCurrentMethod(), variableDescription);

            this.variableDescription = variableDescription;

            return this;
        }

        internal void Groups_list_of(List<SpecificationSpecificGroup> groupsList, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, groupsList));

            groups.AddRange(groupsList);
        }

        internal void Calculate()
        {
            AnovaResult = new SampleSystemUnderTest.AnovaCalculator.AnovaCalculator(variableDescription, groups.Select(g => g.Group)).Calculate();
        }

    }
}