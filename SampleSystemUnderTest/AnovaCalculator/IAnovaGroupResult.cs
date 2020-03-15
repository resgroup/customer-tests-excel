namespace SampleSystemUnderTest.AnovaCalculator
{
    public interface IAnovaGroupResult
    {
        IGroup group { get; set; }
        double mean { get; set; }
        double squaresBetween { get; set; }
        double varianceFromAnovaMean { get; set; }
    }
}