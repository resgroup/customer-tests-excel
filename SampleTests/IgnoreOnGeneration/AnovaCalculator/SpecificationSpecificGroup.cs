using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using Moq;
using SampleSystemUnderTest.AnovaCalculator;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;

namespace SampleTests.IgnoreOnGeneration.AnovaCalculator
{
    internal class SpecificationSpecificGroup : ReportsSpecificationSetup
    {
        readonly Mock<IGroup> anovaGroup;
        readonly List<SpecificationSpecificFloat> values = new List<SpecificationSpecificFloat>();

        public IGroup AnovaGroup =>
                anovaGroup.Object;

        public SpecificationSpecificGroup()
        {
            anovaGroup = new Mock<IGroup>();
            anovaGroup.Setup(m => m.Values).Returns(values.Select(l => l.Value));
        }

        internal SpecificationSpecificGroup Name_of(string name)
        {
            valueProperties.Add(GetCurrentMethod(), name);

            anovaGroup.Setup(m => m.Name).Returns(name);

            return this;
        }

        internal SpecificationSpecificGroup Values_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat> values)
        {
            // the generated test code should do this
            values.PropertyName = GetCurrentMethod().Name;

            // the generated test code should do this
            classTableProperties.Add(values);

            foreach (var row in values.Rows)
                this.values.Add(row.Properties);

            return this;
        }
    }
}