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

        public ReportsSpecificationSetup()
        {
            valueProperties = new List<ReportSpecificationSetupProperty>();
            properties = new List<IReportSpecificationSetupProperty>();
        }

        public void AddValueProperty(MethodBase setupMethod, object propertyValue) =>
            AddValueProperty(new ReportSpecificationSetupProperty(setupMethod, propertyValue));

        // use when there is no value
        public void AddValueProperty(MethodBase setupMethod) =>
            AddValueProperty(new ReportSpecificationSetupProperty(setupMethod, new NoValue()));

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
    }
}
