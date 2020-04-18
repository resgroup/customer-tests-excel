using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomerTestsExcel
{
    public class ReportsSpecificationSetup : IReportsSpecificationSetup
    {
        readonly List<IReportSpecificationSetupProperty> properties;
        public IReadOnlyList<IReportSpecificationSetupProperty> Properties =>
            properties;

        protected readonly List<ReportSpecificationSetupProperty> valueProperties;
        public IReadOnlyList<ReportSpecificationSetupProperty> ValueProperties =>
            valueProperties;

        //protected readonly List<ReportSpecificationSetupClass> classProperties;
        //public IReadOnlyList<ReportSpecificationSetupClass> ClassProperties =>
        //    classProperties;

        //protected readonly List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> classTableProperties;
        //public IReadOnlyList<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> ClassTableProperties =>
        //    classTableProperties;

        //protected readonly List<ReportSpecificationSetupList> listProperties;
        //public IReadOnlyList<ReportSpecificationSetupList> ListProperties =>
        //    listProperties;

        public ReportsSpecificationSetup()
        {
            valueProperties = new List<ReportSpecificationSetupProperty>();
            properties = new List<IReportSpecificationSetupProperty>();
            //classProperties = new List<ReportSpecificationSetupClass>();
            //classTableProperties = new List<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>>();
            //listProperties = new List<ReportSpecificationSetupList>();
        }

        public void AddValueProperty(MethodBase setupMethod, object propertyValue)
        {
            AddValueProperty(new ReportSpecificationSetupProperty(setupMethod, propertyValue));
        }

        // use when there is no value
        public void AddValueProperty(MethodBase setupMethod)
        {
            AddValueProperty(new ReportSpecificationSetupProperty(setupMethod, new NoValue()));
        }

        public void AddValueProperty(ReportSpecificationSetupProperty valueProperty)
        {
            valueProperties.Add(valueProperty);
            properties.Add(valueProperty);
        }

        public void AddClassProperty(ReportSpecificationSetupClass classProperty) =>
            properties.Add(classProperty);

        public void AddClassTableProperty(IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup> classTableProperty) =>
            properties.Add(classTableProperty);

        public void AddListProperty(ReportSpecificationSetupList listProperty) =>
            properties.Add(listProperty);

        //public bool AnythingToSetup() =>
        //    ValueProperties.Any()
        //    || ClassProperties.Any()
        //    || ClassTableProperties.Any()
        //    || ListProperties.Any();

    }
}
