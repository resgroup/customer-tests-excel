using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomerTestsExcel
{
    public class ReportSpecificationSetupClass
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
    }
}
