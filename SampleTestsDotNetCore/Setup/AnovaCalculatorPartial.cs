using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.AnovaCalculator;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleTests.Setup;

namespace SampleTests.Setup
{
    public partial class SpecificationSpecificAnovaCalculator : ReportsSpecificationSetup
    {
        public IAnovaResult AnovaResult { get; private set; }

        internal void Calculate()
        {
            AnovaResult = 
                new SampleSystemUnderTest.AnovaCalculator.AnovaCalculator(
                    VariableDescription, 
                    groupss.Select(g => g.Group)
                ).Calculate();
        }

    }
}