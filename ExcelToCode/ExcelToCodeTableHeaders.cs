using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.ExcelToCode
{
    abstract class TableHeader
    {
        private readonly string _propertyName;
        public string PropertyName { get { return _propertyName; } }

        protected readonly uint _endRow;
        public uint EndRow { get { return _endRow; } }

        private readonly uint _endColumn;
        public uint EndColumn { get { return _endColumn; } }

        public TableHeader(string propertyName, uint endRow, uint endColumn)
        {
            _propertyName = propertyName;
            _endRow = endRow;
            _endColumn = endColumn;
        }
    }

    class PropertyTableHeader : TableHeader
    {
        public PropertyTableHeader(string propertyName, uint endRow, uint endColumn)
            : base(propertyName, endRow, endColumn)
        {
        }
    }

    class SubClassTableHeader : TableHeader
    {
        public string FullSubClassName { get; }
        public string SubClassName { get; }
        public uint StartRow { get; }
        public uint? PropertiesStartColumn { get; }
        public uint PropertiesEndColumn { get; }
        public Dictionary<uint, TableHeader> Headers { get; }

        public SubClassTableHeader(string propertyName, string subClassName, string fullSubClassName, uint startRow, uint endRow, uint? propertiesStartColumn, uint propertiesEndColumn, Dictionary<uint, TableHeader> headers)
            : base(propertyName, endRow, propertiesEndColumn)
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
