using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerTestsExcel.ExcelToCode
{
    public enum ExcelPropertyType
    {
        Null,
        StringNull,
        Number,
        Decimal,
        String,
        DateTime,
        TimeSpan,
        Enum,
        Boolean,
        Object,
        List,
        Function
    }

    public static class ExcelPropertyTypeExtensions
    {
        public static bool IsSimpleProperty(this ExcelPropertyType excelPropertyType)
        {
            return excelPropertyType == 
                ExcelPropertyType.Boolean
                || excelPropertyType == ExcelPropertyType.DateTime
                || excelPropertyType == ExcelPropertyType.Decimal
                || excelPropertyType == ExcelPropertyType.Enum
                || excelPropertyType == ExcelPropertyType.Null
                || excelPropertyType == ExcelPropertyType.Number
                || excelPropertyType == ExcelPropertyType.String
                || excelPropertyType == ExcelPropertyType.TimeSpan;
        }

        public static bool IsPrimitive(this ExcelPropertyType excelPropertyType)
        {
            return excelPropertyType ==
                ExcelPropertyType.Boolean
                || excelPropertyType == ExcelPropertyType.DateTime
                || excelPropertyType == ExcelPropertyType.Decimal
                || excelPropertyType == ExcelPropertyType.Enum
                || excelPropertyType == ExcelPropertyType.Number
                || excelPropertyType == ExcelPropertyType.String
                || excelPropertyType == ExcelPropertyType.TimeSpan;
        }

        public static bool IsPossiblyNullablePrimitive(this ExcelPropertyType excelPropertyType)
        {
            return excelPropertyType ==
                ExcelPropertyType.Boolean
                || excelPropertyType == ExcelPropertyType.DateTime
                || excelPropertyType == ExcelPropertyType.Decimal
                || excelPropertyType == ExcelPropertyType.Enum
                || excelPropertyType == ExcelPropertyType.Number
                || excelPropertyType == ExcelPropertyType.TimeSpan;
        }
    }
}
