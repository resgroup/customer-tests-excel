using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerTestsExcel.ExcelToCode
{
    public interface IExcelToCodeVisitor
    {
        void VisitSimpleGivenProperty(string propertyOrFunctionName, string cSharpCodeRepresentation, ExcelPropertyType excelPropertyType);
    }
}
