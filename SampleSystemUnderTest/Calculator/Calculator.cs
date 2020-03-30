using System;

namespace SampleSystemUnderTest.Calculator
{
    public class Calculator
    {
        public double Calculate(double firstValue, Operation operation, double secondValue)
        {
            switch (operation)
            {
                case Operation.Add:
                    return firstValue + secondValue;
                case Operation.Subtract:
                    return firstValue - secondValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), $"calculation.Operation not supported {operation}");
            }
        }
    }
}