using System;
using static System.Reflection.MethodBase;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificString : ReportsSpecificationSetup
    {
        public string String { get; private set; }

        public SpecificationSpecificString String_of(string value)
        {
            valueProperties.Add(GetCurrentMethod(), value);

            String = value;

            return this;
        }
    }
}