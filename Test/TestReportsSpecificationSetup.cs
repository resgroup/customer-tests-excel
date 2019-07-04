using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.Test
{
    public class TestReportsSpecificationSetup : ReportsSpecificationSetup
    {
        public TestReportsSpecificationSetup AddValueProperty(ReportSpecificationSetupProperty valueProperty)
        {
            valueProperties.Add(valueProperty);
            return this;
        }

        public TestReportsSpecificationSetup AddValueProperty(string propertyName, object propertyValue)
        {
            valueProperties.Add(new ReportSpecificationSetupProperty(propertyName, propertyValue));
            return this;
        }

        public TestReportsSpecificationSetup AddValueProperty(string propertyName)
        {
            valueProperties.Add(propertyName, null);
            return this;
        }

        public TestReportsSpecificationSetup AddListProperty(ReportSpecificationSetupList listProperty)
        {
            listProperties.Add(listProperty);
            return this;
        }
    }
}
