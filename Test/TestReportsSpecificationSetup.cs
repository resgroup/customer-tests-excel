using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.Test
{
    public class TestReportsSpecificationSetup : ReportsSpecificationSetup
    {
        public void AddValueProperty(ReportSpecificationSetupProperty valueProperty) =>
            valueProperties.Add(valueProperty);

        public void AddValueProperty(string propertyName, object propertyValue) =>
            valueProperties.Add(new ReportSpecificationSetupProperty(propertyName, propertyValue));

        public void AddValueProperty(string propertyName) =>
            valueProperties.Add(propertyName, null);

        public void AddListProperty(ReportSpecificationSetupList listProperty) =>
            listProperties.Add(listProperty);
    }
}
