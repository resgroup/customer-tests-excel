using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification
{
    public class ReportSpecificationSetupClass
    {
        public ReportSpecificationSetupClass(string propertyName, IReportsSpecificationSetup properties, bool isChild = false, int? indexInParent = null)
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
