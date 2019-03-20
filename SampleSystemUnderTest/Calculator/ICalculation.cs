using SampleSystemUnderTest.Calculator;

namespace SampleSystemUnderTest.Calculator
{
    public interface ICalculation
    {
        double FirstValue { get; }
        Operation Operation { get; }
        double SecondValue { get; }
    }
}