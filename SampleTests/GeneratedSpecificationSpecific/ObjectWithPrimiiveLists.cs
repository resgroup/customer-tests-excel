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




        readonly List<SpecificationSpecificInteger> integerListSyntaxs;
        readonly List<SpecificationSpecificInteger> integerTableSyntaxs;
        readonly List<SpecificationSpecificFloat> floatTableSyntaxs;
        readonly List<SpecificationSpecificString> stringTableSyntaxs;
        readonly List<SpecificationSpecificDateTime> dateTimeTableSyntaxs;

        public SpecificationSpecificObjectWithPrimiiveLists()
        {
            integerListSyntaxs = new List<SpecificationSpecificInteger>();
            integerTableSyntaxs = new List<SpecificationSpecificInteger>();
            floatTableSyntaxs = new List<SpecificationSpecificFloat>();
            stringTableSyntaxs = new List<SpecificationSpecificString>();
            dateTimeTableSyntaxs = new List<SpecificationSpecificDateTime>();
        }







        internal SpecificationSpecificObjectWithPrimiiveLists IntegerListSyntax_of(SpecificationSpecificInteger integerListSyntax)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), integerListSyntax));

            this.integerListSyntaxs.Add(integerListSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists IntegerListSyntax_list_of(List<SpecificationSpecificInteger> integerListSyntaxs, string listType)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, integerListSyntaxs));

            this.integerListSyntaxs.AddRange(integerListSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists IntegerListSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificInteger> integerListSyntaxs)
        {
            integerListSyntaxs.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(integerListSyntaxs);

            foreach (var row in integerListSyntaxs.Rows)
                this.integerListSyntaxs.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificObjectWithPrimiiveLists IntegerTableSyntax_of(SpecificationSpecificInteger integerTableSyntax)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), integerTableSyntax));

            this.integerTableSyntaxs.Add(integerTableSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists IntegerTableSyntax_list_of(List<SpecificationSpecificInteger> integerTableSyntaxs, string listType)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, integerTableSyntaxs));

            this.integerTableSyntaxs.AddRange(integerTableSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists IntegerTableSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificInteger> integerTableSyntaxs)
        {
            integerTableSyntaxs.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(integerTableSyntaxs);

            foreach (var row in integerTableSyntaxs.Rows)
                this.integerTableSyntaxs.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificObjectWithPrimiiveLists FloatTableSyntax_of(SpecificationSpecificFloat floatTableSyntax)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), floatTableSyntax));

            this.floatTableSyntaxs.Add(floatTableSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists FloatTableSyntax_list_of(List<SpecificationSpecificFloat> floatTableSyntaxs, string listType)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, floatTableSyntaxs));

            this.floatTableSyntaxs.AddRange(floatTableSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists FloatTableSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat> floatTableSyntaxs)
        {
            floatTableSyntaxs.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(floatTableSyntaxs);

            foreach (var row in floatTableSyntaxs.Rows)
                this.floatTableSyntaxs.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificObjectWithPrimiiveLists StringTableSyntax_of(SpecificationSpecificString stringTableSyntax)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), stringTableSyntax));

            this.stringTableSyntaxs.Add(stringTableSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists StringTableSyntax_list_of(List<SpecificationSpecificString> stringTableSyntaxs, string listType)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, stringTableSyntaxs));

            this.stringTableSyntaxs.AddRange(stringTableSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists StringTableSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificString> stringTableSyntaxs)
        {
            stringTableSyntaxs.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(stringTableSyntaxs);

            foreach (var row in stringTableSyntaxs.Rows)
                this.stringTableSyntaxs.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificObjectWithPrimiiveLists DateTimeTableSyntax_of(SpecificationSpecificDateTime dateTimeTableSyntax)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), dateTimeTableSyntax));

            this.dateTimeTableSyntaxs.Add(dateTimeTableSyntax);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists DateTimeTableSyntax_list_of(List<SpecificationSpecificDateTime> dateTimeTableSyntaxs, string listType)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, dateTimeTableSyntaxs));

            this.dateTimeTableSyntaxs.AddRange(dateTimeTableSyntaxs);

            return this;
        }

        internal SpecificationSpecificObjectWithPrimiiveLists DateTimeTableSyntax_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificDateTime> dateTimeTableSyntaxs)
        {
            dateTimeTableSyntaxs.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(dateTimeTableSyntaxs);

            foreach (var row in dateTimeTableSyntaxs.Rows)
                this.dateTimeTableSyntaxs.Add(row.Properties);

            return this;
        }
    }
}
