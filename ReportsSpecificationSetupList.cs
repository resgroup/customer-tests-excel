using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class ReportSpecificationSetupList
    {
        public string PropertyName { get; }
        public string PropertyType { get; }
        public IEnumerable<IReportsSpecificationSetup> Items { get; }

        public ReportSpecificationSetupList(
            string propertyName,
            string propertyType,
            IEnumerable<IReportsSpecificationSetup> items)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (string.IsNullOrWhiteSpace(propertyType))
                throw new ArgumentNullException(nameof(propertyType));

            Items = items ?? throw new ArgumentNullException(nameof(items));
            PropertyName = propertyName;
            PropertyType = propertyType;

        }
    }
}
