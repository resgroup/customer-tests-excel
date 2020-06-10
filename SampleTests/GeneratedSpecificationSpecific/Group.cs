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

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificGroup : ReportsSpecificationSetup
    {
        readonly Mock<IGroup> group;

        public IGroup Group =>
            group.Object;

        readonly List<SpecificationSpecificFloat> floatss = new List<SpecificationSpecificFloat>();

        public SpecificationSpecificGroup()
        {
            group = new Mock<IGroup>();

            group.Setup(m => m.Floats).Returns(floatss.Select(l => l.Float));
        }

        internal SpecificationSpecificGroup Name_of(String name)
        {
            AddValueProperty(GetCurrentMethod(), name);

            group.Setup(m => m.Name).Returns(name);

            return this;
        }

        internal SpecificationSpecificGroup Floats_of(SpecificationSpecificFloat floats)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), floats));

            this.floatss.Add(floats);

            return this;
        }

        internal SpecificationSpecificGroup Floats_list_of(string listType, List<SpecificationSpecificFloat> floatss)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, floatss));

            this.floatss.AddRange(floatss);

            return this;
        }

        internal SpecificationSpecificGroup Floats_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat> floatss)
        {
            floatss.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(floatss);

            foreach (var row in floatss.Rows)
                this.floatss.Add(row.Properties);

            return this;
        }
    }
}
