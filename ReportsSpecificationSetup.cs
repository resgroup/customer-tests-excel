using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class ReportsSpecificationSetup : IReportsSpecificationSetup
    {
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
    }
}
