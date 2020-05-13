namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeComplexProperty : ExcelToCodeBase
    {
        public ExcelToCodeComplexProperty(ExcelToCodeState excelToCodeState)
            : base(excelToCodeState)
        {
        }

        internal bool CanParse() =>
            HasGivenSubProperties();

        internal void Parse()
        {
            var excelGivenLeft = excel.CurrentCell();

            var cSharpMethodName = converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

            code.BlankLine();

            using (excel.AutoRestoreMoveRight())
            {
                var excelGivenRight = excel.CurrentCellRaw();
                var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                using (code.OutputAndOpenAutoClosingBracket($".{cSharpMethodName}"))
                    CreateObject(excelGivenLeft, excelGivenRightString);
            }
        }

        bool HasGivenSubProperties() =>
            excel.PeekBelowRight() == converter.WithProperties;

        void CreateObject(string excelPropertyName, string excelClassName)
        {
            log.VisitGivenComplexPropertyDeclaration(
                converter.GivenPropertyNameExcelNameToSutName(excelPropertyName),
                excelClassName);

            CreateObjectWithoutVisiting(excelClassName);

            log.VisitGivenComplexPropertyFinalisation();
        }

        internal void CreateObjectWithoutVisiting(string excelClassName)
        {
            excel.MoveDown(); // this is a bit mysterious

            string cSharpClassName = converter.ExcelClassNameToCodeName(excelClassName);

            code.Add($"new {cSharpClassName}()");

            SetObjectProperties();

            excel.MoveUp(); // this is a bit mysterious
        }

        void SetObjectProperties()
        {
            if (excel.CurrentCell() == converter.WithProperties)
            {
                using (excel.AutoRestoreMoveRight())
                {
                    excel.MoveDown();
                    while (!string.IsNullOrEmpty(excel.CurrentCell()))
                    {
                        DoProperty();
                        excel.MoveDown();
                    }
                }
            }
        }
    }
}
