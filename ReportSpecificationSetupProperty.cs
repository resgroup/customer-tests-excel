using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CustomerTestsExcel
{
    public class ReportSpecificationSetupProperty : IReportSpecificationSetupProperty
    {
        // This function could be removed now, but it will cause trouble for things that are using it
        // Should probably mark it as deprecated somehow
        public ReportSpecificationSetupProperty(string propertyName, object propertyValue)
        {
            PropertyNamespace = "";
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public ReportSpecificationSetupProperty(MethodBase setupMethod, object propertyValue)
        {
            PropertyNamespace = "";
            PropertyName = setupMethod.Name;
            PropertyValue = propertyValue;
        }

        public string PropertyNamespace { get; protected set; }
        public string PropertyName { get; protected set; }
        public object PropertyValue { get; protected set; }

        public void Callback(
                Action<ReportSpecificationSetupProperty> valuePropertyCallback, 
                Action<ReportSpecificationSetupClass> classPropertyCallback, 
                Action<IReportsSpecificationSetup> classTablePropertyCallback, 
                Action<ReportSpecificationSetupList> listPropertyCallback) =>
            valuePropertyCallback(this);
    }

}
