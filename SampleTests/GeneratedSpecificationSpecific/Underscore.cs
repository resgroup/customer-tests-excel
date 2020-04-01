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
    public partial class SpecificationSpecificUnderscore : ReportsSpecificationSetup
    {
        public String A_Property { get; private set; }



        readonly List<SpecificationSpecificA_Table> table_Propertys;
        readonly List<SpecificationSpecificA_Table> list_Propertys;

        public SpecificationSpecificUnderscore()
        {
            table_Propertys = new List<SpecificationSpecificA_Table>();
            list_Propertys = new List<SpecificationSpecificA_Table>();
        }



        internal SpecificationSpecificUnderscore A_Property_of(String a_Property)
        {
            valueProperties.Add(GetCurrentMethod(), a_Property);

            this.A_Property = a_Property;

            return this;
        }




        internal SpecificationSpecificUnderscore Table_Property_of(SpecificationSpecificA_Table table_Property)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), table_Property));

            this.table_Propertys.Add(table_Property);

            return this;
        }

        internal SpecificationSpecificUnderscore Table_Property_list_of(List<SpecificationSpecificA_Table> table_Propertys, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, table_Propertys));

            this.table_Propertys.AddRange(table_Propertys);

            return this;
        }

        internal SpecificationSpecificUnderscore Table_Property_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table> table_Propertys)
        {
            table_Propertys.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(table_Propertys);

            foreach (var row in table_Propertys.Rows)
                this.table_Propertys.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificUnderscore List_Property_of(SpecificationSpecificA_Table list_Property)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), list_Property));

            this.list_Propertys.Add(list_Property);

            return this;
        }

        internal SpecificationSpecificUnderscore List_Property_list_of(List<SpecificationSpecificA_Table> list_Propertys, string listType)
        {
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, list_Propertys));

            this.list_Propertys.AddRange(list_Propertys);

            return this;
        }

        internal SpecificationSpecificUnderscore List_Property_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table> list_Propertys)
        {
            list_Propertys.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(list_Propertys);

            foreach (var row in list_Propertys.Rows)
                this.list_Propertys.Add(row.Properties);

            return this;
        }
    }
}
