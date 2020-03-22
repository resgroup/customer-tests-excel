namespace SampleSystemUnderTest.VermeulenNearWakeLength
{
    public interface IVermeulenNearWakeLengthInput
    {
        double Velocity { get; }
        double AmbientTurbuluence { get; }
        double RevolutionsPerMinute { get; }
        double Thrust_Coefficient { get; }
        ITurbineGeometry TurbineGeometry { get; }
    }
}