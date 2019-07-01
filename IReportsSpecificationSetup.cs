using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface IReportsSpecificationSetup
    {
        IEnumerable<ReportSpecificationSetupProperty> ValueProperties { get; }
        IEnumerable<ReportSpecificationSetupClass> ClassProperties { get; }
        IEnumerable<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> ClassTableProperties { get; }
    }
}
