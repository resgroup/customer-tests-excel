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
            string excelPropertyName,
            string subClassName,
            string fullSubClassName,
            uint startRow,
            uint endRow,
            uint? propertiesStartColumn,
            uint propertiesEndColumn,
            Dictionary<uint, TableHeader> headers)
            : base(
                  excelPropertyName,
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

        public override string ToString() =>
            $"{{ PropertyName: {ExcelPropertyName}, EndRow: {EndRow}, EndColumn: {EndColumn}, IsRoundTrippable: {IsRoundTrippable}, SubClassName: {SubClassName}, StartRow: {StartRow}, FullSubClassName: {FullSubClassName}, PropertiesStartColumn: {PropertiesStartColumn}, PropertiesEndColumn: {PropertiesEndColumn}, Headers: [ {HeadersToString()} ] }}";

        string HeadersToString() =>
            string.Join(",", Headers.Select(h => "{" + h.ToString() + "}"));

    }
}
