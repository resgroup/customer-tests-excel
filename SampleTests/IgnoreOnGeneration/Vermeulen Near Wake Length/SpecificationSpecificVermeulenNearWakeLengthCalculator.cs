using System;
using System.Collections.Generic;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.Vermeulen_Near_Wake_Length;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    public class SpecificationSpecificVermeulenNearWakeLengthCalculator : ReportsSpecificationSetup
    {
        internal IEnumerable<IVermeulenNearWakeLength> VermeulenNearWakeLengths { get; private set; }

        readonly List<IVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputs = new List<IVermeulenNearWakeLengthInput>();

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputs)
        {
            vermeulenNearWakeLengthInputs.PropertyName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            _classTableProperties.Add(vermeulenNearWakeLengthInputs);

            foreach (var row in vermeulenNearWakeLengthInputs.Rows)
                this.vermeulenNearWakeLengthInputs.Add(row.Properties);

            return this;
        }

        internal void Calculate()
        {
            VermeulenNearWakeLengths = new VermeulenNearWakeLengthCalculator(vermeulenNearWakeLengthInputs).Calculate();
        }
    }
}