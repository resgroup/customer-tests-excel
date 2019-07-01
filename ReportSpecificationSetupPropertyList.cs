using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CustomerTestsExcel
{
    public class ReportSpecificationSetupPropertyList : List<ReportSpecificationSetupProperty>
    {
        // This function could be removed now, but it will cause trouble for things that are using it
        // Should probably mark it as deprecated somehow
        public void Add(MethodBase setupMethod, object propertyValue)
        {
            Add(new ReportSpecificationSetupProperty(setupMethod, propertyValue));
        }

        public void Add(string propertyName, object propertyValue)
        {
            Add(new ReportSpecificationSetupProperty(propertyName, propertyValue));
        }

        // use when there is no value
        public void Add(MethodBase setupMethod)
        {
            Add(new ReportSpecificationSetupProperty(setupMethod, new NoValue()));
        }

    }
}
