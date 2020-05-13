using System;
using System.Collections.Generic;
using System.Linq;

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

        bool IsList() =>
            excel
            .CurrentCell()
            .EndsWith(converter.ListOf, StringComparison.InvariantCultureIgnoreCase);

        void DoProperty()
        {
            if (excelToCodeState.Table.CanParse())
                excelToCodeState.Table.Parse();
            else if (excelToCodeState.ComplexProperty.CanParse())
                excelToCodeState.ComplexProperty.Parse();
            else if (excelToCodeState.List.CanParse())
                excelToCodeState.List.Parse();
            else
                excelToCodeState.SimpleProperty.Parse();
        }

        internal void Parse()
        {
            var startCellReference = excel.CellReferenceA1Style();
            var excelGivenLeft = excel.CurrentCell();

            using (excel.AutoRestoreMoveRight())
            {
                var excelGivenRight = excel.CurrentCellRaw();
                var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                CheckMissingWithItemForList(startCellReference);

                var cSharpMethodName = converter.GivenListPropertyNameExcelNameToCodeName(excelGivenLeft);
                var cSharpClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);

                code.BlankLine();
                using (excel.AutoRestoreMoveDown())
                {
                    log.VisitGivenListPropertyDeclaration(
                        converter.GivenListPropertyNameExcelNameToCodeVariableName(excelGivenLeft),
                        excelGivenRightString);

                    using (code.OutputAndOpenAutoClosingBracket($".{cSharpMethodName}"))
                    {
                        code.Add($"\"{cSharpClassName}\", ");
                        code.Add($"new FluentList<{cSharpClassName}>()");
                        while (excel.CurrentCell() == converter.WithItem)
                        {
                            excel.MoveDown();

                            // Add an item to the list
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
                    }
                    log.VisitGivenListPropertyFinalisation();
                }
            }
        }

        // check to see if it looks like a table, but does not end with converter.ListOf
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
