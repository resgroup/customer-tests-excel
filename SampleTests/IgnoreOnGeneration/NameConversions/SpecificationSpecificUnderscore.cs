using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.Calculator;
using System;
using System.Collections.Generic;

namespace SampleTests.IgnoreOnGeneration.NameConversions
{
    public class SpecificationSpecificUnderscore : ReportsSpecificationSetup
    {
        public readonly int A_Result = 1;

        internal void A_Property_of(string anything)
        {
            valueProperties.Add(GetCurrentMethod(), anything);
        }

        internal void Table_Property_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table> table_Property)
        {
            table_Property.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(table_Property);
        }

        internal void List_Property_list_of(List<SpecificationSpecificA_Table> items, string listType)
        {
            // the generated test code should do this
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, items));
        }

        internal void Do_Nothing()
        {
            // this test it so checks that everything compiles / round trips, it is not trying to test the running code, so no need to do anything here.
        }

    }
}