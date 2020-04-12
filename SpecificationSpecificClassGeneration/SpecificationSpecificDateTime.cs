using System;
using static System.Reflection.MethodBase;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificDateTime : ReportsSpecificationSetup
    {
        public DateTime DateTime { get; private set; }

        public SpecificationSpecificDateTime DateTime_of(DateTime value)
        {
            valueProperties.Add(GetCurrentMethod(), value);

            DateTime = value;

            return this;
        }
    }
}