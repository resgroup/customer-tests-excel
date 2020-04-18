using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.Test
{
    public class TestReportsSpecificationSetup : ReportsSpecificationSetup
    {
        public TestReportsSpecificationSetup FluentAddValueProperty(ReportSpecificationSetupProperty valueProperty)
        {
            AddValueProperty(valueProperty);
            return this;
        }

        public TestReportsSpecificationSetup FluentAddValueProperty(string propertyName, object propertyValue)
        {
            AddValueProperty(new ReportSpecificationSetupProperty(propertyName, propertyValue));
            return this;
        }

        public TestReportsSpecificationSetup FluentAddValueProperty(string propertyName)
        {
            return FluentAddValueProperty(propertyName, null);
        }

        public TestReportsSpecificationSetup FluentAddListProperty(ReportSpecificationSetupList listProperty)
        {
            AddListProperty(listProperty);
            return this;
        }
    }
}
