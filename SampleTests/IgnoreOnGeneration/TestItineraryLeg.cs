using RES.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTestsRerouting
{
    public class SpecificationSpecificItineraryLegCreationalProperties : ReportsSpecificationSetup
    {
        protected string _origin;
        public void Origin_of(string origin)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, origin);
            _origin = origin;
        }

        protected string _destination;
        public void Destination_of(string destination)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, destination);
            _destination = destination;
        }
    }

    public class SpecificationSpecificItineraryLeg : ReportsSpecificationSetupIBlank, IBlank, IReportsSpecificationSetup<IBlank>
    {
        public SpecificationSpecificItineraryLeg(IReportsSpecificationSetup creationalProperties)
            : base(creationalProperties) { }
    }
}
