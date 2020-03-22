namespace SampleSystemUnderTest.VermeulenNearWakeLength
{
    public interface IVermeulenNearWakeLengthInput
    {
        double Velocity { get; }
        double Ambient_Turbulence { get; }
        double RevolutionsPerMinute { get; }
        double Thrust_Coefficient { get; }
        ITurbineGeometry TurbineGeometry { get; }
    }
}