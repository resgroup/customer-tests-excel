using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.AnovaCalculator;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleTests.GeneratedSpecificationSpecific;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificAnovaCalculator : ReportsSpecificationSetup
    {
        public IAnovaResult AnovaResult { get; private set; }

        internal void Calculate()
        {
            AnovaResult = 
                new SampleSystemUnderTest.AnovaCalculator.AnovaCalculator(
                    variableDescription, 
                    groupss.Select(g => g.Group)
                ).Calculate();
        }

    }
}