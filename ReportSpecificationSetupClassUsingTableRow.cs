﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface IReportSpecificationSetupClassUsingTableRow<out T>
        where T : IReportsSpecificationSetup
    {
        T Properties { get; }
    }

    public class ReportSpecificationSetupClassUsingTableRow<T> : IReportSpecificationSetupClassUsingTableRow<T>
        where T : IReportsSpecificationSetup
    {
        public ReportSpecificationSetupClassUsingTableRow(T properties, int? indexInParent = null)
        {
            Properties = properties;
            IndexInParent = indexInParent;
        }

        public int? IndexInParent { get; protected set; }
        public T Properties { get; protected set; }
    }
}
