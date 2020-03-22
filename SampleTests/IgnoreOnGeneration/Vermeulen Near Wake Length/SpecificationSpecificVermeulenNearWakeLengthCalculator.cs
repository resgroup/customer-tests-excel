using static System.Reflection.MethodBase;
using System;
using System.Collections.Generic;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.Vermeulen_Near_Wake_Length;
using SampleTests.GeneratedSpecificationSpecific;
using System.Linq;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    public class SpecificationSpecificVermeulenNearWakeLengthCalculator : ReportsSpecificationSetup
    {
        internal IEnumerable<IVermeulenNearWakeLength> VermeulenNearWakeLengths { get; private set; }

        readonly List<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputs = new List<SpecificationSpecificVermeulenNearWakeLengthInput>();

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputs)
        {
            vermeulenNearWakeLengthInputs.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(vermeulenNearWakeLengthInputs);

            foreach (var row in vermeulenNearWakeLengthInputs.Rows)
                this.vermeulenNearWakeLengthInputs.Add(row.Properties);

            return this;
        }

        internal void Calculate()
        {
            VermeulenNearWakeLengths = 
                new VermeulenNearWakeLengthCalculator(
                    vermeulenNearWakeLengthInputs.Select(i => i.VermeulenNearWakeLengthInput).ToList()
                ).Calculate();
        }
    }
}