using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification
{
    // this class does most of the hard work in implementing IReportsSpecificationSetup. Our generated specificationspericicsetup classes can't use it, as they need to inherit the concrete business object, but custom setup classes can use it if they are implementing an interface rather than inheriting.
    public class ReportsSpecificationSetup : IReportsSpecificationSetup
    {
        // there probably won't be any creational properties if we are implementing an interface rather than inheriting a class, so this can probably be ignored by descending classes.
        public IReportsSpecificationSetup CreationalProperties { get; protected set; }

        protected readonly ReportSpecificationSetupPropertyList _valueProperties;
        public IEnumerable<ReportSpecificationSetupProperty> ValueProperties { get { return _valueProperties; } }

        protected readonly List<ReportSpecificationSetupClass> _classProperties;
        public IEnumerable<ReportSpecificationSetupClass> ClassProperties { get { return _classProperties; } }

        protected readonly List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> _classTableProperties;
        public IEnumerable<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> ClassTableProperties { get { return _classTableProperties; } }

        public ReportsSpecificationSetup()
        {
            _valueProperties = new ReportSpecificationSetupPropertyList();
            _classProperties = new List<ReportSpecificationSetupClass>();
            _classTableProperties = new List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>>();
        }

        public ReportsSpecificationSetup(IReportsSpecificationSetup creationalProperties)  : this()
		{
            CreationalProperties = creationalProperties;
		}
    }
}
