using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleSystemUnderTest.Calculator;
using SampleTests.IgnoreOnGeneration.NameConversions;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificVermeulenNearWakeLengthCalculator : ReportsSpecificationSetup
    {




        readonly List<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputss;

        public SpecificationSpecificVermeulenNearWakeLengthCalculator()
        {
            vermeulenNearWakeLengthInputss = new List<SpecificationSpecificVermeulenNearWakeLengthInput>();
        }





        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_of(SpecificationSpecificVermeulenNearWakeLengthInput vermeulenNearWakeLengthInputs)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), vermeulenNearWakeLengthInputs));

            this.vermeulenNearWakeLengthInputss.Add(vermeulenNearWakeLengthInputs);

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_list_of(List<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputss, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, vermeulenNearWakeLengthInputss));

            this.vermeulenNearWakeLengthInputss.AddRange(vermeulenNearWakeLengthInputss);

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputss)
        {
            vermeulenNearWakeLengthInputss.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(vermeulenNearWakeLengthInputss);

            foreach (var row in vermeulenNearWakeLengthInputss.Rows)
                this.vermeulenNearWakeLengthInputss.Add(row.Properties);

            return this;
        }
    }
}
