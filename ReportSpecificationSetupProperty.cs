using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CustomerTestsExcel
{
    public class ReportSpecificationSetupProperty
    {
        // use when the property value wll not need namespacing (eg is null, a clr base type etc etc)
        public ReportSpecificationSetupProperty(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        // use when the property value might need namespacing (eg enums can be external etc etc)
        public ReportSpecificationSetupProperty(MethodBase setupMethod, object propertyValue)
        {
            string specificionatRootNamespace = setupMethod.DeclaringType.Namespace.Substring(0, setupMethod.DeclaringType.Namespace.LastIndexOf('.'));
            specificionatRootNamespace = specificionatRootNamespace.Substring(0, specificionatRootNamespace.LastIndexOf('.'));
            string propertyNamespace = "";
            if (propertyValue != null)
                propertyNamespace = propertyValue.GetType().Namespace;

            bool needsNamespace = !(propertyNamespace == specificionatRootNamespace
                    ||
                    propertyNamespace == specificionatRootNamespace + ".Base"
                    ||
                    propertyNamespace == specificionatRootNamespace + ".Specification.Stubs");


            PropertyNamespace = (needsNamespace && propertyValue != null) ? propertyValue.GetType().Namespace + "." : "";
            
            PropertyName = setupMethod.Name;
            PropertyValue = propertyValue;
        }

        public string PropertyNamespace { get; protected set; }
        public string PropertyName { get; protected set; }
        public object PropertyValue { get; protected set; }
    }

}
