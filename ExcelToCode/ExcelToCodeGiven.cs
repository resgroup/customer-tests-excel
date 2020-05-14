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

            if (excelToCodeState.ComplexProperty.CanParse())
                AddErrorToCodeAndLog($"The root object for the test (${sutName}) must have sub properties (the cell below and to the right must be 'With Properties'). This test does not, which means that the c# code generation will not work properly.");


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
