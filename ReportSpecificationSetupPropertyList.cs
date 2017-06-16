using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CustomerTestsExcel
{
    public class ReportSpecificationSetupPropertyList : List<ReportSpecificationSetupProperty>
    {
        // use when the property value might need namespacing (eg enums can be external etc etc)
        public void Add(MethodBase setupMethod, object propertyValue)
        {
            Add(new ReportSpecificationSetupProperty(setupMethod, propertyValue));
        }

        // use when the property value wll not need namespacing (eg is null, a clr base type etc etc)
        public void Add(string propertyName, object propertyValue)
        {
            Add(new ReportSpecificationSetupProperty(propertyName, propertyValue));
        }
    }
}
