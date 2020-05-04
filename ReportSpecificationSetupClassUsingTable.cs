using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface IReportSpecificationSetupClassUsingTable<out T> : IReportSpecificationSetupProperty
        where T : IReportsSpecificationSetup
    {
        string PropertyName { get; set; }
        IEnumerable<IReportSpecificationSetupClassUsingTableRow<T>> Rows { get; }
    }

    public class ReportSpecificationSetupClassUsingTable<T> : IReportSpecificationSetupClassUsingTable<T>
        where T : IReportsSpecificationSetup
    {
        readonly List<IReportSpecificationSetupClassUsingTableRow<T>> rowPropertyValues;
        public string PropertyName { get; set; }
        public string ClassName { get; set; }

        public IEnumerable<IReportSpecificationSetupClassUsingTableRow<T>> Rows { get { return rowPropertyValues; } }

        public ReportSpecificationSetupClassUsingTable()
        {
            rowPropertyValues = new List<IReportSpecificationSetupClassUsingTableRow<T>>();
        }

        public ReportSpecificationSetupClassUsingTable<T> Add(T row)
        {
            rowPropertyValues.Add(new ReportSpecificationSetupClassUsingTableRow<T>(row));
            return this;
        }

        public void Callback(
            Action<ReportSpecificationSetupProperty> valuePropertyCallback,
            Action<ReportSpecificationSetupClass> classPropertyCallback,
            Action<IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>> classTablePropertyCallback,
            Action<ReportSpecificationSetupList> listPropertyCallback)
        {
            classTablePropertyCallback(this as IReportSpecificationSetupClassUsingTable<IReportsSpecificationSetup>);
        }
    }
}
