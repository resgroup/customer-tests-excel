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
    public partial class SpecificationSpecificAnovaCalculator : ReportsSpecificationSetup
    {
        public String VariableDescription { get; private set; }



        readonly List<SpecificationSpecificGroup> groupss;

        public SpecificationSpecificAnovaCalculator()
        {
            groupss = new List<SpecificationSpecificGroup>();
        }



        internal SpecificationSpecificAnovaCalculator VariableDescription_of(String variableDescription)
        {
            valueProperties.Add(GetCurrentMethod(), variableDescription);

            this.VariableDescription = variableDescription;

            return this;
        }




        internal SpecificationSpecificAnovaCalculator Groups_of(SpecificationSpecificGroup groups)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), groups));

            this.groupss.Add(groups);

            return this;
        }

        internal SpecificationSpecificAnovaCalculator Groups_list_of(List<SpecificationSpecificGroup> groupss, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, groupss));

            this.groupss.AddRange(groupss);

            return this;
        }

        internal SpecificationSpecificAnovaCalculator Groups_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificGroup> groupss)
        {
            groupss.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(groupss);

            foreach (var row in groupss.Rows)
                this.groupss.Add(row.Properties);

            return this;
        }
    }
}
