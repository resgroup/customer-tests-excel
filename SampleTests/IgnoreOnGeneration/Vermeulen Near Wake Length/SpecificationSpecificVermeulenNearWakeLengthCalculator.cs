using System;
using System.Collections.Generic;
using CustomerTestsExcel;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    public class SpecificationSpecificVermeulenNearWakeLengthCalculator : ReportsSpecificationSetup
    {
        internal IEnumerable<IVermeulenNearWakeLength> VermeulenNearWakeLengths { get; set; }

        internal void VermeulenNearWakeLengthInputs_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInputs> vermeulenNearWakeLengthInputs_table)
        {
            throw new NotImplementedException();
        }

        internal void Calculate()
        {
            throw new NotImplementedException();
        }
    }
}