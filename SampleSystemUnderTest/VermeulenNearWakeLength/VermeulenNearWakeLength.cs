namespace SampleSystemUnderTest.VermeulenNearWakeLength
{
    public class VermeulenNearWakeLength : IVermeulenNearWakeLength
    {
        public double VermeulenNearWakeLength_m { get; }
        public double AngularVelocity_rps { get; }
        public double TipSpeedRatio { get; }
        public double FlowFieldRatio { get; }
        public double ShearTurbulenceWakeErosionRate { get; }
        public double AmbientTurbulenceWakeErosionRate { get; }
        public double MechanicalWakeErosionRate { get; }
        public double TotalErosionRate { get; }
        public double RadiusOfInviscidExpandedRotorDisk_m { get; }
        public double N { get; }

        public VermeulenNearWakeLength(
            double vermeulenNearWakeLength_m,
            double angularVelocity,
            double tipSpeedRatio,
            double flowFieldRatio,
            double shearTurbulenceWakeErosionRate,
            double ambientTurbulenceWakeErosionRate,
            double mechanicalWakeErosionRate,
            double totalErosionRate,
            double radiusOfInviscidExpandedRotorDisk,
            double n
            )
        {
            VermeulenNearWakeLength_m = vermeulenNearWakeLength_m;
            AngularVelocity_rps = angularVelocity;
            TipSpeedRatio = tipSpeedRatio;
            FlowFieldRatio = flowFieldRatio;
            ShearTurbulenceWakeErosionRate = shearTurbulenceWakeErosionRate;
            AmbientTurbulenceWakeErosionRate = ambientTurbulenceWakeErosionRate;
            MechanicalWakeErosionRate = mechanicalWakeErosionRate;
            TotalErosionRate = totalErosionRate;
            RadiusOfInviscidExpandedRotorDisk_m = radiusOfInviscidExpandedRotorDisk;
            N = n;
        }
    }
}