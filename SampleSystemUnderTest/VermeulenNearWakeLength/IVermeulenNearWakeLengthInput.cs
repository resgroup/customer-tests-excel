namespace SampleSystemUnderTest.VermeulenNearWakeLength
{
    public interface IVermeulenNearWakeLengthInput
    {
        double Velocity_mps { get; }
        double AmbientTurbuluence { get; }
        double RevolutionsPerMinute { get; }
        double ThrustCoefficient { get; }
        ITurbineGeometry TurbineGeometry { get; }
    }
}