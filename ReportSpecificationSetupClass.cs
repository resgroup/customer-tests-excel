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
            IReportsSpecificationSetup properties,
            bool isChild = false,
            int? indexInParent = null)
            : this(
                  setupMethod.Name,
                  properties,
                  isChild,
                  indexInParent)
        {
        }

        public ReportSpecificationSetupClass(
            string propertyName,
            IReportsSpecificationSetup properties,
            bool isChild = false,
            int? indexInParent = null)
        {
            PropertyName = propertyName;
            Properties = properties;
            IsChild = isChild;
            IndexInParent = indexInParent;
        }

        public int? IndexInParent { get; protected set; }
        public bool IsChild { get; protected set; }
        public string PropertyName { get; protected set; }
        public IReportsSpecificationSetup Properties { get; protected set; }
    }
}
