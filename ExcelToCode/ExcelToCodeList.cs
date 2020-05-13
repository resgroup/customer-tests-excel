using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeList : ExcelToCodeBase
    {
        public ExcelToCodeList(ExcelToCodeState excelToCodeState)
            : base(excelToCodeState)
        {
        }

        internal bool CanParse()
        {
            CheckMissingListOf();

            return IsList();
        }

        internal void Parse()
        {
            var startCellReference = excel.CellReferenceA1Style();
            var excelGivenLeft = excel.CurrentCell();

            using (excel.AutoRestoreMoveRight())
            {
                CheckMissingWithItemForList(startCellReference);

                var excelGivenRight = excel.CurrentCellRaw();
                var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;
                var cSharpMethodName = converter.GivenListPropertyNameExcelNameToCodeName(excelGivenLeft);
                var cSharpClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);

                using (excel.AutoRestoreMoveDown())
                {
                    log.VisitGivenListPropertyDeclaration(
                        converter.GivenListPropertyNameExcelNameToCodeVariableName(excelGivenLeft),
                        excelGivenRightString);

                    code.BlankLine();
                    using (code.OutputAndOpenAutoClosingBracket($".{cSharpMethodName}"))
                    {
                        code.Add($"\"{cSharpClassName}\", ");
                        code.Add($"new FluentList<{cSharpClassName}>()");
                        while (excel.CurrentCell() == converter.WithItem)
                        {
                            excel.MoveDown();

                            AddItemToList(cSharpClassName);
                        }
                    }
                    log.VisitGivenListPropertyFinalisation();
                }
            }
        }

        void AddItemToList(string cSharpClassName)
        {
            using (excel.AutoRestoreMoveRight())
            {
                using (code.OutputAndOpenAutoClosingBracket($".FluentAdd"))
                {
                    code.Add($"new {cSharpClassName}()");

                    while (!string.IsNullOrEmpty(excel.CurrentCell()))
                    {
                        DoProperty();
                        excel.MoveDown();
                    }
                }
            }
        }

        bool IsList() =>
            excel
            .CurrentCell()
            .EndsWith(converter.ListOf, StringComparison.InvariantCultureIgnoreCase);

        void CheckMissingListOf()
        {
            if (LooksLikeAListButIsnt())
                AddErrorToCodeAndLog($"It looks like you might be trying to set up a list property, starting at cell {excel.CellReferenceA1Style()}. If this is the case, please make sure that cell {excel.CellReferenceA1Style()} ends with '{converter.ListOf}'");
        }

        bool LooksLikeAListButIsnt() =>
            IsList() == false
            && excel.PeekBelowRight() == converter.WithItem;

        void CheckMissingWithItemForList(string listStartCellReference)
        {
            using (excel.AutoRestoreMoveDown())
            {
                if (excel.CurrentCell() != converter.WithItem)
                    throw new ExcelToCodeException($"The list property starting at {listStartCellReference} is not formatted correctly. Cell {excel.CellReferenceA1Style()} should be '{converter.WithItem}', but is '{excel.CurrentCell()}'");
            }
        }
    }
}
