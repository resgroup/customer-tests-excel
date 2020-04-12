using static System.Reflection.MethodBase;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificInteger : ReportsSpecificationSetup
    {
        public int Integer { get; private set; }

        public SpecificationSpecificInteger Integer_of(int value)
        {
            valueProperties.Add(GetCurrentMethod(), value);
            
            Integer = value;

            return this;
        }
    }
}