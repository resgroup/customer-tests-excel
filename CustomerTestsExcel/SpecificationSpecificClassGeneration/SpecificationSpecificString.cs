using System;
using static System.Reflection.MethodBase;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificString : ReportsSpecificationSetup
    {
        public string String { get; private set; }

        public SpecificationSpecificString String_of(string value)
        {
            AddValueProperty(GetCurrentMethod(), value);

            String = value;

            return this;
        }
    }
}