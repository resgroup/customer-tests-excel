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
    // This is a generated class that matches the root class of an excel test.

    // It should create all the things you need for the 'Given' section of the
    // test, but you will need to add a method for the 'When' section, and a 
    // property for the 'Assert' section.
    // You can see an example at the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/IgnoreOnGeneration/Vermeulen%20Near%20Wake%20Length/SpecificationSpecificVermeulenNearWakeLengthCalculatorPartial.cs
    // The 'Calculate()' method matches up with 'When', 'Calculate' from the 
    // test, and the 'VermeulenNearWakeLengths' property matches up with the 
    // 'Assert', 'VermeulenNearWakeLengths' from the test.
    // You can see the associated Excel test on the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/ExcelTests/Vermeulen%20Near%20Wake%20Length.xlsx

    // Custom classes should go under a directory called 'IgnoreOnGeneration'.
    // If the custom class filename is the same as this one (SpecificationSpecificUnderscore),
    // then it will be used instead of this function. If it is called something else,
    // say SpecificationSpecificUnderscorePartial, then this class will remain, and
    // the custom class can add to it.

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
            AddValueProperty(GetCurrentMethod(), a_Property);

            this.A_Property = a_Property;

            return this;
        }




        internal SpecificationSpecificUnderscore Table_Property_of(SpecificationSpecificA_Table table_Property)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), table_Property));

            this.table_Propertys.Add(table_Property);

            return this;
        }

        internal SpecificationSpecificUnderscore Table_Property_list_of(string listType, List<SpecificationSpecificA_Table> table_Propertys)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, table_Propertys));

            this.table_Propertys.AddRange(table_Propertys);

            return this;
        }

        internal SpecificationSpecificUnderscore Table_Property_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table> table_Propertys)
        {
            table_Propertys.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(table_Propertys);

            foreach (var row in table_Propertys.Rows)
                this.table_Propertys.Add(row.Properties);

            return this;
        }
        internal SpecificationSpecificUnderscore List_Property_of(SpecificationSpecificA_Table list_Property)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), list_Property));

            this.list_Propertys.Add(list_Property);

            return this;
        }

        internal SpecificationSpecificUnderscore List_Property_list_of(string listType, List<SpecificationSpecificA_Table> list_Propertys)
        {
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, list_Propertys));

            this.list_Propertys.AddRange(list_Propertys);

            return this;
        }

        internal SpecificationSpecificUnderscore List_Property_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table> list_Propertys)
        {
            list_Propertys.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(list_Propertys);

            foreach (var row in list_Propertys.Rows)
                this.list_Propertys.Add(row.Properties);

            return this;
        }
    }
}
