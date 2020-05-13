using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeSimpleProperty : ExcelToCodeBase
    {
        public ExcelToCodeSimpleProperty(ExcelToCodeState excelToCodeState)
            : base(excelToCodeState)
        {
        }

        internal void Parse()
        {
            var excelGivenLeft = excel.CurrentCell();

            using (excel.AutoRestoreMoveRight())
            {
                var excelGivenRight = excel.CurrentCellRaw();
                var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                var cSharpMethodName = converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

                code.Add($".{cSharpMethodName}({converter.PropertyValueExcelToCode(excelGivenLeft, excelGivenRight)})");

                VisitGivenSimplePropertyOrFunction(
                    excelGivenLeft,
                    excelGivenRight);
            }
        }

    }
}
