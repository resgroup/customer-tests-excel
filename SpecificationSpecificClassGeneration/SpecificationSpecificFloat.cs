using static System.Reflection.MethodBase;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificFloat : ReportsSpecificationSetup
    {
        public float Float { get; private set; }

        public SpecificationSpecificFloat Float_of(float value)
        {
            valueProperties.Add(GetCurrentMethod(), value);

            Float = value;

            return this;
        }
    }
}