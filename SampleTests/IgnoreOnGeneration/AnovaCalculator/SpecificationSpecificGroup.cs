using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel;
using Moq;
using SampleSystemUnderTest.AnovaCalculator;

namespace SampleTests.IgnoreOnGeneration.AnovaCalculator
{
    internal class SpecificationSpecificGroup : ReportsSpecificationSetup
    {
        readonly Mock<IAnovaGroup> anovaGroup;
        readonly List<SpecificationSpecificValue> values = new List<SpecificationSpecificValue>();

        public IAnovaGroup AnovaGroup =>
                anovaGroup.Object;

        public SpecificationSpecificGroup()
        {
            anovaGroup = new Mock<IAnovaGroup>();
            anovaGroup.Setup(m => m.Values).Returns(values.Select(l => l.value));
        }

        internal SpecificationSpecificGroup Name_of(string name)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, name);

            anovaGroup.Setup(m => m.Name).Returns(name);

            return this;
        }

        internal SpecificationSpecificGroup Values_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificValue> values)
        {
            values.PropertyName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            _classTableProperties.Add(values);

            foreach (var row in values.Rows)
                this.values.Add(row.Properties);

            return this;
        }
    }
}