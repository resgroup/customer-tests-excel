using RES.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTestsRerouting
{
    public class SpecificationSpecificCargoCreationalProperties : ReportsSpecificationSetup
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

        readonly List<SpecificationSpecificItineraryLeg> _legs = new List<SpecificationSpecificItineraryLeg>();
        public void ItineraryLeg_of(SpecificationSpecificItineraryLeg leg)
        {
            _classProperties.Add(new ReportSpecificationSetupClass(System.Reflection.MethodBase.GetCurrentMethod().Name, leg, true, _legs.Count));
            _legs.Add(leg);
        }

        public void ItineraryLeg_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificItineraryLeg> legs)
        {
            legs.PropertyName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            _classTableProperties.Add(legs);
            foreach (var row in legs.Rows) _legs.Add(row.Properties);
        }
    }

    public class SpecificationSpecificCargo : ReportsSpecificationSetupIBlank, IBlank, IReportsSpecificationSetup<IBlank>
    {
        public SpecificationSpecificCargo(IReportsSpecificationSetup creationalProperties)
            : base(creationalProperties) { }
    }
}
