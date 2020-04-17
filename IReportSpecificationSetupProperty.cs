using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface IReportSpecificationSetupProperty
    {
        void Callback(
            Action<ReportSpecificationSetupProperty> valuePropertyCallback,
            Action<ReportSpecificationSetupClass> classPropertyCallback,
            Action<IReportsSpecificationSetup> classTablePropertyCallback,
            Action<ReportSpecificationSetupList> listPropertyCallback
        );
    }
}
