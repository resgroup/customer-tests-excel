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
using SampleSystemUnderTest.CustomProperties;
using SampleSystemUnderTest.Misc;

namespace SampleTests.Setup
{
    // This is a generated class that matches the root class of an excel test.

    // It should create all the things you need for the 'Given' section of the
    // test, but you will need to add a method for the 'When' section, and a 
    // property for the 'Assert' section.
    // You can see an example at the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/Setup/Vermeulen%20Near%20Wake%20Length/SpecificationSpecificVermeulenNearWakeLengthCalculatorPartial.cs
    // The 'Calculate()' method matches up with 'When', 'Calculate' from the 
    // test, and the 'VermeulenNearWakeLengths' property matches up with the 
    // 'Assert', 'VermeulenNearWakeLengths' from the test.
    // You can see the associated Excel test on the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/ExcelTests/Vermeulen%20Near%20Wake%20Length.xlsx

    // Custom classes should go in the 'Setup' folder.
    // If the custom class filename is 'SpecificationSpecificVermeulenNearWakeLengthCalculatorOverride.cs',
    // then it will be used instead of this function. If it is called something else,
    // say 'SpecificationSpecificVermeulenNearWakeLengthCalculatorPartial.cs', then this class will remain, and
    // the custom class can add to it.

    public partial class SpecificationSpecificVermeulenNearWakeLengthCalculator : ReportsSpecificationSetup
    {

        readonly List<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputss;

        public SpecificationSpecificVermeulenNearWakeLengthCalculator()
        {
            vermeulenNearWakeLengthInputss = new List<SpecificationSpecificVermeulenNearWakeLengthInput>();
        }

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_of(SpecificationSpecificVermeulenNearWakeLengthInput vermeulenNearWakeLengthInputs)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), vermeulenNearWakeLengthInputs));

            this.vermeulenNearWakeLengthInputss.Add(vermeulenNearWakeLengthInputs);

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_list_of(string listType, List<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputss)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, vermeulenNearWakeLengthInputss));

            this.vermeulenNearWakeLengthInputss.AddRange(vermeulenNearWakeLengthInputss);

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthCalculator VermeulenNearWakeLengthInputs_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInputss)
        {
            vermeulenNearWakeLengthInputss.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(vermeulenNearWakeLengthInputss);

            foreach (var row in vermeulenNearWakeLengthInputss.Rows)
                this.vermeulenNearWakeLengthInputss.Add(row.Properties);

            return this;
        }
    }
}
