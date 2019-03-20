using System;

namespace SampleSystemUnderTest.Calculator
{
    public class Calculator
    {
        public double Calculate(ICalculation calculation)
        {
            if (calculation.Operation == Operation.Add)
                return calculation.FirstValue + calculation.SecondValue;

            throw new ArgumentOutOfRangeException(nameof(calculation), $"calculation.Operation not supported {calculation.Operation}");
        }
    }
}