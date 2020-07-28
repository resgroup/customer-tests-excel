using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.Calculator;
using System;

namespace SampleTests.Setup
{
    public partial class SpecificationSpecificValidator : ReportsSpecificationSetup
    {
        public bool Valid { get; internal set; } = false;

        internal SpecificationSpecificValidator WithValidProperties()
        {
            AddValueProperty(GetCurrentMethod());

            Valid = true;

            return this;
        }

        internal void Validate()
        {
        }
    }
}