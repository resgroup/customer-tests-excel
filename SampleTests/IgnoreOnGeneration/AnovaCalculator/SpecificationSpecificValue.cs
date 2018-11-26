using System;
using CustomerTestsExcel;

namespace SampleTests.IgnoreOnGeneration.AnovaCalculator
{
    internal class SpecificationSpecificValue : ReportsSpecificationSetup
    {
        internal float value { get; private set; }

        internal SpecificationSpecificValue Value_of(float value)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, value);

            this.value = value;

            return this;
        }
    }
}