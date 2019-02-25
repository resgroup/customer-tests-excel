namespace SampleSystemUnderTest.VermeulenNearWakeLength
{
    public class VermeulenNearWakeLength : IVermeulenNearWakeLength
    {
        public double VermeulenNearWakeLength_m { get; }

        public VermeulenNearWakeLength(double vermeulenNearWakeLength_m)
        {
            VermeulenNearWakeLength_m = vermeulenNearWakeLength_m;
        }
    }
}