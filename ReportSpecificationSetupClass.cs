using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomerTestsExcel
{
    public class ReportSpecificationSetupClass : IReportSpecificationSetupProperty
    {
        public ReportSpecificationSetupClass(
            MethodBase setupMethod,
            IReportsSpecificationSetup properties)
            : this(
                  setupMethod.Name,
                properties)
        {
        }

        public ReportSpecificationSetupClass(
            string propertyName,
            IReportsSpecificationSetup properties)
        {
            PropertyName = propertyName;
            Properties = properties;
        }

        public string PropertyName { get; protected set; }
        public IReportsSpecificationSetup Properties { get; protected set; }

        public void Callback(
                Action<ReportSpecificationSetupProperty> valuePropertyCallback,
                Action<ReportSpecificationSetupClass> classPropertyCallback,
                Action<IReportsSpecificationSetup> classTablePropertyCallback,
                Action<ReportSpecificationSetupList> listPropertyCallback) =>
            classPropertyCallback(this);

    }
}
