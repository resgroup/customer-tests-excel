namespace SampleSystemUnderTest.AnovaCalculator
{
    public interface IAnovaResult
    {
        string VariableDescription { get; }
        double SS_Between { get; }
        int DF_Between { get; }
        double MS_Between { get; }
        double SS_Within { get; }
        int DF_Within { get; }
        double MS_Within { get; }
        double F { get; }
        double StatisticalSignificance { get; }
        double EffectSize { get; }
    }
}