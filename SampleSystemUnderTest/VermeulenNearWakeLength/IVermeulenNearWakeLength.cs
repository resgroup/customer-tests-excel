namespace SampleSystemUnderTest.VermeulenNearWakeLength
{
    public interface IVermeulenNearWakeLength
    {
        double VermeulenNearWakeLength_m { get; }
        double AngularVelocity_rps { get; }
        double TipSpeedRatio { get; }
        double FlowFieldRatio { get; }
        double ShearTurbulenceWakeErosionRate { get; }
        double AmbientTurbulenceWakeErosionRate { get; }
        double MechanicalWakeErosionRate { get; }
        double TotalErosionRate { get; }
        double RadiusOfInviscidExpandedRotorDisk_m { get; }
        double N { get; }
    }
}