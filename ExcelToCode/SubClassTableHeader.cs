using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.ExcelToCode
{
    class SubClassTableHeader : TableHeader
    {
        const bool NOT_ROUND_TRIPPABLE = false;
        public string FullSubClassName { get; }
        public string SubClassName { get; }
        public uint StartRow { get; }
        public uint? PropertiesStartColumn { get; }
        public uint PropertiesEndColumn { get; }
        public Dictionary<uint, TableHeader> Headers { get; }

        public SubClassTableHeader(
            string propertyName, 
            string subClassName, 
            string fullSubClassName, 
            uint startRow, 
            uint endRow, 
            uint? propertiesStartColumn, 
            uint propertiesEndColumn, 
            Dictionary<uint, TableHeader> headers)
            : base(
                  propertyName, 
                  endRow, 
                  propertiesEndColumn,
                  NOT_ROUND_TRIPPABLE)
        {
            SubClassName = subClassName;
            FullSubClassName = fullSubClassName;
            StartRow = startRow;
            PropertiesStartColumn = propertiesStartColumn;
            PropertiesEndColumn = propertiesEndColumn;
            Headers = headers;
        }
    }
}
