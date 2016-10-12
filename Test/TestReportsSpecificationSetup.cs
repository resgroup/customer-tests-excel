using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification.Test
{
    public class TestReportsSpecificationSetup : ReportsSpecificationSetup
    {
        public void AddValueProperty(ReportSpecificationSetupProperty valueProperty)
        {
            _valueProperties.Add(valueProperty);
        }

        public void AddValueProperty(string propertyName, object propertyValue)
        {
            _valueProperties.Add(new ReportSpecificationSetupProperty(propertyName, propertyValue));
        }

        public void AddValueProperty(string propertyName)
        {
            _valueProperties.Add(propertyName, null);
        }
    }
}
