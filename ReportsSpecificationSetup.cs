using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class ReportsSpecificationSetup : IReportsSpecificationSetup
    {
        protected readonly ReportSpecificationSetupPropertyList valueProperties;
        public IEnumerable<ReportSpecificationSetupProperty> ValueProperties =>
            valueProperties;

        protected readonly List<ReportSpecificationSetupClass> classProperties;
        public IEnumerable<ReportSpecificationSetupClass> ClassProperties =>
            classProperties;

        protected readonly List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> classTableProperties;
        public IEnumerable<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> ClassTableProperties =>
            classTableProperties;

        protected readonly List<ReportSpecificationSetupList> listProperties;
        public IEnumerable<ReportSpecificationSetupList> ListProperties =>
            listProperties;

        public ReportsSpecificationSetup()
        {
            valueProperties = new ReportSpecificationSetupPropertyList();
            classProperties = new List<ReportSpecificationSetupClass>();
            classTableProperties = new List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>>();
            listProperties = new List<ReportSpecificationSetupList>();
        }
    }
}
