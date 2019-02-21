using System;
using System.Collections.Generic;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.Vermeulen_Near_Wake_Length;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    public class SpecificationSpecificVermeulenNearWakeLengthCalculator : ReportsSpecificationSetup
    {
        internal IEnumerable<IVermeulenNearWakeLength> VermeulenNearWakeLengths { get; }

        readonly List<SpecificationSpecificVermeulenNearWakeLengthInputs> vermeulenNearWakeLengthInputs = new List<SpecificationSpecificVermeulenNearWakeLengthInputs>();

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInputs> vermeulenNearWakeLengthInputs)
        {
            vermeulenNearWakeLengthInputs.PropertyName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            _classTableProperties.Add(vermeulenNearWakeLengthInputs);

            foreach (var row in vermeulenNearWakeLengthInputs.Rows)
                this.vermeulenNearWakeLengthInputs.Add(row.Properties);

            return this;
        }

        internal void Calculate()
        {
            throw new NotImplementedException();
        }
    }
}