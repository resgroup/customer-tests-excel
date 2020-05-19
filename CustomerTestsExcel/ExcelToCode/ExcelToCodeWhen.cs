using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeWhen : ExcelToCodeBase
    {
        public ExcelToCodeWhen(ExcelToCodeState excelToCodeState)
            : base(excelToCodeState)
        {
        }

        internal void DoWhen(string sutName)
        {
            var sutClassName = CSharpSUTSpecificationSpecificClassName(sutName);
            var sutVariableName = CSharpSUTVariableName(sutName);

            excel.MoveDownToToken(converter.When);

            using (excel.AutoRestoreMoveRight())
            {
                code.BlankLine();
                code.Add($"public override string When({sutClassName} {sutVariableName})");

                using (code.Scope())
                {
                    code.Add($"{sutVariableName}.{converter.ActionExcelNameToCodeName(excel.CurrentCell())}();");
                    code.Add($"return \"{excel.CurrentCell()}\";");
                }

                code.BlankLine();
            }

            excel.MoveDown();
        }

    }
}
