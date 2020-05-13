using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeGiven : ExcelToCodeBase
    {
        public ExcelToCodeGiven(ExcelToCodeState excelToCodeState)
            : base(excelToCodeState)
        {
        }

        internal void DoGiven(string sutName)
        {
            excel.MoveDownToToken(converter.Given);

            using (excel.AutoRestoreMoveRight())
            {
                code.BlankLine();
                code.Add($"public override {CSharpSUTSpecificationSpecificClassName(sutName)} Given()");
                using (code.AutoCloseCurlyBracket())
                    CreateRootObject(sutName);
            }

            excel.MoveUp();
        }

        void CreateRootObject(string excelClassName)
        {
            log.VisitGivenRootClassDeclaration(excelClassName);

            code.Add("return");
            using (code.AutoCloseIndent())
            {
                excelToCodeState.ComplexProperty.CreateObjectWithoutVisiting(excelClassName);
            }
            code.Add(";");

            log.VisitGivenRootClassFinalisation();
        }

    }
}
