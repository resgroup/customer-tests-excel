using System;

namespace SampleSystemUnderTest.Calculator
{
    public class Calculator
    {
        public double Calculate(ICalculation calculation)
        {
            switch (calculation.Operation)
            {
                case Operation.Add:
                    return calculation.FirstValue + calculation.SecondValue;
                case Operation.Subtract:
                    return calculation.FirstValue - calculation.SecondValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculation), $"calculation.Operation not supported {calculation.Operation}");
            }
        }
    }
}