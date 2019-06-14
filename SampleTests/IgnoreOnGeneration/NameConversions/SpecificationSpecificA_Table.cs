using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using System;

namespace SampleTests.IgnoreOnGeneration.NameConversions
{
    internal class SpecificationSpecificA_Table : ReportsSpecificationSetup
    {
        internal void A_Property_of(string a_Property)
        {
            _valueProperties.Add(GetCurrentMethod(), a_Property);
        }
    }
}