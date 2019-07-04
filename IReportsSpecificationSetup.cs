using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface IReportsSpecificationSetup
    {
        IReadOnlyList<ReportSpecificationSetupProperty> ValueProperties { get; }
        IReadOnlyList<ReportSpecificationSetupClass> ClassProperties { get; }
        IReadOnlyList<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> ClassTableProperties { get; }
        IReadOnlyList<ReportSpecificationSetupList> ListProperties { get; }

        bool AnythingToSetup();
    }
}
