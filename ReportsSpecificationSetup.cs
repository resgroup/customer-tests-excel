using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class ReportsSpecificationSetup : IReportsSpecificationSetup
    {
        protected readonly ReportSpecificationSetupPropertyList properties;
        public IReadOnlyList<IReportSpecificationSetupProperty> Properties =>
            properties;

        protected readonly ReportSpecificationSetupPropertyList valueProperties;
        public IReadOnlyList<ReportSpecificationSetupProperty> ValueProperties =>
            valueProperties;

        protected readonly List<ReportSpecificationSetupClass> classProperties;
        public IReadOnlyList<ReportSpecificationSetupClass> ClassProperties =>
            classProperties;

        protected readonly List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> classTableProperties;
        public IReadOnlyList<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> ClassTableProperties =>
            classTableProperties;

        protected readonly List<ReportSpecificationSetupList> listProperties;
        public IReadOnlyList<ReportSpecificationSetupList> ListProperties =>
            listProperties;

        public ReportsSpecificationSetup()
        {
            valueProperties = new ReportSpecificationSetupPropertyList();
            classProperties = new List<ReportSpecificationSetupClass>();
            classTableProperties = new List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>>();
            listProperties = new List<ReportSpecificationSetupList>();
        }

        public bool AnythingToSetup() =>
            ValueProperties.Any()
            || ClassProperties.Any()
            || ClassTableProperties.Any()
            || ListProperties.Any();

    }
}
