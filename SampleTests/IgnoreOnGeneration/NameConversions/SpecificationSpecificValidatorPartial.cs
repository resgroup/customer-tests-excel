using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.Calculator;
using System;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificValidator : ReportsSpecificationSetup
    {
        public bool Valid { get; internal set; } = false;

        internal void WithValidProperties()
        {
            AddValueProperty(GetCurrentMethod());

            Valid = true;
        }

        internal void Validate()
        {
        }
    }
}