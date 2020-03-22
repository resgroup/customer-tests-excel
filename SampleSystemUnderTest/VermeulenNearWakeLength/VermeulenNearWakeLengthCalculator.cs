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
            // I have copied the formulas from Excel, which helps a lot when writing the calculation.
            // A lot of the formulas used named ranges, but this only to works when the named 
            // ranges are on the same row.
            // The idea could be expanded by referencing the data in the setup in the assertion rows,
            // and applying named ranges there, at which point everything could become a named range,
            // using the domain language.
            // You could also define the Excel Functions (PI(), SQRT(), IF() etc in C#, and at that
            // point you could just copy and paste the excel formulas in to the code and they would
            // work.

            double radius = input.TurbineGeometry.Diameter / 2;

            // =F11* (2 * PI() / 60)
            double angularVelocity = input.RevolutionsPerMinute * (2 * PI / 60); 

            //=(AngularVelocity * Radius) / D11
            double tipSpeedRatio = (radius * angularVelocity) / input.Velocity;

            //=IF(G11>0.8888,3,1/SQRT(1-G11))
            double flowFieldRatio = (input.Thrust_Coefficient > 0.8888)
                ? 3
                : 1 / (Sqrt(1 - input.Thrust_Coefficient));

            //=(1-FlowFieldRatio)*SQRT(1.49+FlowFieldRatio)/(9.76*(1+FlowFieldRatio))
            double shearTurbulenceWakeErosionRate = (1 - flowFieldRatio) * Sqrt(1.49 + flowFieldRatio) / (9.76 * (1 + flowFieldRatio)); 

            //=IF(E11>0.02,2.5*E11+0.05,5*E11)
            double ambientTurbulenceWakeErosionRate = (input.AmbientTurbuluence > 0.02)
                ? 2.5 * input.AmbientTurbuluence + 0.05
                : 5 * input.AmbientTurbuluence; 

            //=0.012*H11*TipSpeedRatio
            double mechanicalWakeErosionRate = 0.012 * input.TurbineGeometry.NumberOfBlades * tipSpeedRatio;

            //=SQRT(POWER(ShearTurbulenceWakeErosionRate,2)+POWER(AmbientTurbulenceWakeErosionRate,2)+POWER(MechanicalWakeErosionRate,2))
            double totalErosionRate = Sqrt(Pow(shearTurbulenceWakeErosionRate, 2) + Pow(ambientTurbulenceWakeErosionRate, 2) + Pow(mechanicalWakeErosionRate, 2));

            //=Radius*SQRT((FlowFieldRatio+1)/2)
            double radiusOfInviscidExpandedRotorDisk = radius * Sqrt((flowFieldRatio + 1) / 2); 

            //=SQRT(0.214+0.144*FlowFieldRatio)* (1-SQRT(0.134+0.124*FlowFieldRatio))/((1-SQRT(0.214+0.144*FlowFieldRatio))*SQRT(0.134+0.124*FlowFieldRatio))
            double n =
                Sqrt(0.214 + 0.144 * flowFieldRatio) *
                (1 - Sqrt(0.134 + 0.124 * flowFieldRatio))
                /
                ((1 - Sqrt(0.214 + 0.144 * flowFieldRatio)) *
                Sqrt(0.134 + 0.124 * flowFieldRatio));
            
            //=N*RadiusOfInviscidExpandedRotorDisk/TotalErosionRate
            double vermeulenNearWakeLength_m = n * radiusOfInviscidExpandedRotorDisk / totalErosionRate;

            return new VermeulenNearWakeLength(
                vermeulenNearWakeLength_m: vermeulenNearWakeLength_m,
                angularVelocity: angularVelocity,
                tipSpeedRatio: tipSpeedRatio,
                flowFieldRatio: flowFieldRatio,
                shearTurbulenceWakeErosionRate: shearTurbulenceWakeErosionRate,
                ambientTurbulenceWakeErosionRate: ambientTurbulenceWakeErosionRate,
                mechanicalWakeErosionRate: mechanicalWakeErosionRate,
                totalErosionRate: totalErosionRate,
                radiusOfInviscidExpandedRotorDisk: radiusOfInviscidExpandedRotorDisk,
                n: n
            );
        }
    }
}