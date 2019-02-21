using CustomerTestsExcel;
using System;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    internal class SpecificationSpecificTurbineGeometry : ReportsSpecificationSetup
    {
        internal int numberOfBlades { get; private set; }
        internal double diameter_m { get; private set; }

        internal SpecificationSpecificTurbineGeometry NumberOfBlades_of(int numberOfBlades)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, numberOfBlades);

            this.numberOfBlades = numberOfBlades;

            return this;
        }

        internal SpecificationSpecificTurbineGeometry Diameter_of(double diameter_m)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, diameter_m);

            this.diameter_m = diameter_m;

            return this;
        }
    }
}