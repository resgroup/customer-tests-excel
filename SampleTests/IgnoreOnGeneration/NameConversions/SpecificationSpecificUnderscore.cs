using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.Calculator;
using System;

namespace SampleTests.IgnoreOnGeneration.NameConversions
{
    public class SpecificationSpecificUnderscore : ReportsSpecificationSetup
    {
        public readonly int A_Result = 1;

        internal void A_Property_of(string anything)
        {
            _valueProperties.Add(GetCurrentMethod(), anything);
        }

        internal void Table_Property_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table> table_Property)
        {
            table_Property.PropertyName = GetCurrentMethod().Name;

            _classTableProperties.Add(table_Property);

            //return this;
        }

        internal void List_Property_of(SpecificationSpecificA_Table list_Property)
        {
            // always passing in 0 as indexInParent, as we aren't setting up anything
            _classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), list_Property, true, 0));

            //return this;

        }

        internal void Do_Nothing()
        {
        }

    }
}