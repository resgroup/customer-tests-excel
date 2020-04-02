using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleSystemUnderTest.Calculator;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificCalculator : ReportsSpecificationSetup
    {
        public Single FirstValue { get; private set; }
        public Single SecondValue { get; private set; }
        public Operation Operation { get; private set; }





        public SpecificationSpecificCalculator()
        {

        }



        internal SpecificationSpecificCalculator FirstValue_of(Single firstValue)
        {
            valueProperties.Add(GetCurrentMethod(), firstValue);

            this.FirstValue = firstValue;

            return this;
        }

        internal SpecificationSpecificCalculator SecondValue_of(Single secondValue)
        {
            valueProperties.Add(GetCurrentMethod(), secondValue);

            this.SecondValue = secondValue;

            return this;
        }

        internal SpecificationSpecificCalculator Operation_of(Operation operation)
        {
            valueProperties.Add(GetCurrentMethod(), operation);

            this.Operation = operation;

            return this;
        }





    }
}
