using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace SampleSystemUnderTest.VermeulenNearWakeLength
{
    public class VermeulenNearWakeLengthCalculator
    {
        readonly IReadOnlyList<IVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputs;

        public VermeulenNearWakeLengthCalculator(IReadOnlyList<IVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputs)
        {
            this.vermeulenNearWakeLengthInputs = vermeulenNearWakeLengthInputs ?? throw new ArgumentNullException(nameof(vermeulenNearWakeLengthInputs));
        }

        public IEnumerable<IVermeulenNearWakeLength> Calculate() =>
            vermeulenNearWakeLengthInputs.Select(i => CalculateVermeulenNearWakeLength(i));

        IVermeulenNearWakeLength CalculateVermeulenNearWakeLength(IVermeulenNearWakeLengthInput input)
        {
            // used named ranges and then copy and paste the 
            double radius = input.TurbineGeometry.Diameter_m / 2;
            double angularVelocity = input.RevolutionsPerMinute * (2 * PI / 60); // =F11* (2 * PI() / 60)
            double tipSpeedRatio = (radius * angularVelocity) / input.Velocity_mps; // = (G28 * F28) / D11
            double flowFieldRatio = (input.ThrustCoefficient > 0.8888)
                ? 3
                : 1 / (Sqrt(1 - input.ThrustCoefficient)); //=IF(G11>0.8888,3,1/SQRT(1-G11))
            double shearTurbulenceWakeErosionRate = (1 - flowFieldRatio) * Sqrt(1.49 + flowFieldRatio) / (9.76 * (1 + flowFieldRatio)); //=(1-I28)*SQRT(1.49+I28)/(9.76*(1+I28))
            double ambientTurbulenceWakeErosionRate = (input.AmbientTurbuluence > 0.02)
                ? 2.5 * input.AmbientTurbuluence + 0.05
                : 5 * input.AmbientTurbuluence; //=IF(E11>0.02,2.5*E11+0.05,5*E11)
            double mechanicalWakeErosionRate = 0.012 * input.TurbineGeometry.NumberOfBlades * tipSpeedRatio;//=0.012*H11*H28
            double totalErosionRate = Sqrt(Pow(shearTurbulenceWakeErosionRate, 2) + Pow(ambientTurbulenceWakeErosionRate, 2) + Pow(mechanicalWakeErosionRate, 2));// =SQRT(POWER(J28,2)+POWER(K28,2)+POWER(L28,2))
            double radiusOfInviscidExpandedRotorDisk = radius * Sqrt((flowFieldRatio + 1) / 2); //=F28*SQRT((I28+1)/2)
            //=SQRT(0.214+0.144*I28) * (1-SQRT(0.134+0.124*I28))/((1-SQRT(0.214+0.144*I28))*SQRT(0.134+0.124*I28))
            double n =
                Sqrt(0.214 + 0.144 * flowFieldRatio) *
                (1 - Sqrt(0.134 + 0.124 * flowFieldRatio))
                /
                ((1 - Sqrt(0.214 + 0.144 * flowFieldRatio)) *
                Sqrt(0.134 + 0.124 * flowFieldRatio));
            double vermeulenNearWakeLength_m = n * radiusOfInviscidExpandedRotorDisk / totalErosionRate;//=O28 * N28 / M28

            return new VermeulenNearWakeLength(
                vermeulenNearWakeLength_m: vermeulenNearWakeLength_m
            );
        }
    }
}