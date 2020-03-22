using static System.Reflection.MethodBase;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificFloat : ReportsSpecificationSetup
    {
        public float Value { get; private set; }

        public SpecificationSpecificFloat Value_of(float value)
        {
            valueProperties.Add(GetCurrentMethod(), value);

            Value = value;

            return this;
        }
    }
}