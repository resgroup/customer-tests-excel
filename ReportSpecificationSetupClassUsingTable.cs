using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface IReportSpecificationSetupClassUsingTable<out T>
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

        public void Add(T row)
        {
            rowPropertyValues.Add(new ReportSpecificationSetupClassUsingTableRow<T>(row));
        }
    }
}
