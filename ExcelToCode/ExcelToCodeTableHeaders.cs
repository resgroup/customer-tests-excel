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
        private readonly string _fullSubClassName;
        public string FullSubClassName { get { return _fullSubClassName; } }

        private readonly string _subClassName;
        public string subClassName { get { return _subClassName; } }

        private readonly uint _startRow;
        public uint StartRow { get { return _startRow; } }

        private readonly uint? _creationalPropertiesStartColumn;
        public uint? CreationalPropertiesStartColumn { get { return _creationalPropertiesStartColumn; } }

        private readonly uint _creationalPropertiesEndColumn;
        public uint CreationalPropertiesEndColumn { get { return _creationalPropertiesEndColumn; } }

        private readonly uint? _propertiesStartColumn;
        public uint? PropertiesStartColumn { get { return _propertiesStartColumn; } }

        private readonly uint _propertiesEndColumn;
        public uint PropertiesEndColumn { get { return _propertiesEndColumn; } }

        private readonly Dictionary<uint, TableHeader> _headers;

        public Dictionary<uint, TableHeader> Headers { get { return _headers; } }

        public SubClassTableHeader(string propertyName, string subClassName, string fullSubClassName, uint startRow, uint endRow, uint? creationalPropertiesStartColumn, uint creationalPropertiesEndColumn, uint? propertiesStartColumn, uint propertiesEndColumn, Dictionary<uint, TableHeader> headers)
            : base(propertyName, endRow, Math.Max(creationalPropertiesEndColumn, propertiesEndColumn))
        {
            _subClassName = subClassName;
            _fullSubClassName = fullSubClassName;
            _startRow = startRow;
            _creationalPropertiesStartColumn = creationalPropertiesStartColumn;
            _creationalPropertiesEndColumn = creationalPropertiesEndColumn;
            _propertiesStartColumn = propertiesStartColumn;
            _propertiesEndColumn = propertiesEndColumn;
            _headers = headers;
        }
    }
}
