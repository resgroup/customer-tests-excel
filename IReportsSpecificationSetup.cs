using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface IReportsSpecificationSetup
    {
        IReportsSpecificationSetup CreationalProperties { get; }
        IEnumerable<ReportSpecificationSetupProperty> ValueProperties { get; }
        IEnumerable<ReportSpecificationSetupClass> ClassProperties { get; }
        IEnumerable<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> ClassTableProperties { get; }
    }

    public interface IReportsSpecificationSetup<IBusiness> : IReportsSpecificationSetup
    {
        IBusiness BusinessInterface { get; }
    }
}
