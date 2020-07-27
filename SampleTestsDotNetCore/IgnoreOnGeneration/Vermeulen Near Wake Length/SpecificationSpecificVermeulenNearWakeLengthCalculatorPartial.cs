using static System.Reflection.MethodBase;
using System;
using System.Collections.Generic;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.Vermeulen_Near_Wake_Length;
using SampleTests.GeneratedSpecificationSpecific;
using System.Linq;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificVermeulenNearWakeLengthCalculator : ReportsSpecificationSetup
    {
        internal IEnumerable<IVermeulenNearWakeLength> VermeulenNearWakeLengths { get; private set; }

        internal void Calculate()
        {
            VermeulenNearWakeLengths = 
                new VermeulenNearWakeLengthCalculator(
                    vermeulenNearWakeLengthInputss.Select(i => i.VermeulenNearWakeLengthInput).ToList()
                ).Calculate();
        }
    }
}