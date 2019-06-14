using static System.Reflection.MethodBase;
using CustomerTestsExcel;
using SampleSystemUnderTest.Calculator;
using System;

namespace SampleTests.IgnoreOnGeneration.NameConversions
{
    public class SpecificationSpecificValidator : ReportsSpecificationSetup
    {
        public bool Valid { get; internal set; }

        public SpecificationSpecificValidator()
        {
            Valid = false;
        }


        internal void WithValidProperties()
        {
            _valueProperties.Add(GetCurrentMethod());

            Valid = true;
        }

        internal void Validate()
        {
        }
    }
}