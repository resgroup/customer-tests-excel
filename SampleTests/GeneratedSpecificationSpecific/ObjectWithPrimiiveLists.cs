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

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificObjectWithPrimiiveLists : ReportsSpecificationSetup
    {




        readonly List<SpecificationSpecificInteger> integerTableSyntaxs;
        readonly List<SpecificationSpecificFloat> floatTableSyntaxs;
        readonly List<SpecificationSpecificString> stringTableSyntaxs;

        public SpecificationSpecificObjectWithPrimiiveLists()
        {
            integerTableSyntaxs = new List<SpecificationSpecificInteger>();
            floatTableSyntaxs = new List<SpecificationSpecificFloat>();
            stringTableSyntaxs = new List<SpecificationSpecificString>();
        }







        internal SpecificationSpecificObjectWithPrimiiveLists IntegerTableSyntax_of(SpecificationSpecificInteger integerTableSyntax)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), integerTableSyntax));

            this.integerTableSyntaxs.Add(integerTableSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists IntegerTableSyntax_list_of(List<SpecificationSpecificInteger> integerTableSyntaxs, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, integerTableSyntaxs));

            this.integerTableSyntaxs.AddRange(integerTableSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists IntegerTableSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificInteger> integerTableSyntaxs)
        {
            integerTableSyntaxs.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(integerTableSyntaxs);

            foreach (var row in integerTableSyntaxs.Rows)
                this.integerTableSyntaxs.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificObjectWithPrimiiveLists FloatTableSyntax_of(SpecificationSpecificFloat floatTableSyntax)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), floatTableSyntax));

            this.floatTableSyntaxs.Add(floatTableSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists FloatTableSyntax_list_of(List<SpecificationSpecificFloat> floatTableSyntaxs, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, floatTableSyntaxs));

            this.floatTableSyntaxs.AddRange(floatTableSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists FloatTableSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat> floatTableSyntaxs)
        {
            floatTableSyntaxs.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(floatTableSyntaxs);

            foreach (var row in floatTableSyntaxs.Rows)
                this.floatTableSyntaxs.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificObjectWithPrimiiveLists StringTableSyntax_of(SpecificationSpecificString stringTableSyntax)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), stringTableSyntax));

            this.stringTableSyntaxs.Add(stringTableSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists StringTableSyntax_list_of(List<SpecificationSpecificString> stringTableSyntaxs, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, stringTableSyntaxs));

            this.stringTableSyntaxs.AddRange(stringTableSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists StringTableSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificString> stringTableSyntaxs)
        {
            stringTableSyntaxs.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(stringTableSyntaxs);

            foreach (var row in stringTableSyntaxs.Rows)
                this.stringTableSyntaxs.Add(row.Properties);

            return this;
        }
    }
}
