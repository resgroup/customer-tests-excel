using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.Calculator;
using System;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificCalculator : ReportsSpecificationSetup
    {
        public double Result { get; internal set; }

        internal void Perform_Operation()
        {
            Result = new SampleSystemUnderTest.Calculator.Calculator().Calculate(FirstValue, Operation, SecondValue);
        }
    }
}