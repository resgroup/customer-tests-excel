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

        internal bool IsList(string excelGivenLeft) =>
            excelGivenLeft.EndsWith(converter.ListOf, StringComparison.InvariantCultureIgnoreCase);

        void DoProperty()
        {
            CheckMissingListOf();
            var startCellReference = excel.CellReferenceA1Style();
            var excelGivenLeft = excel.CurrentCell();

            if (excelToCodeState.Table.CanParse())
            {
                excelToCodeState.Table.Parse();
            }
            else if (excelToCodeState.ComplexProperty.CanParse())
            {
                excelToCodeState.ComplexProperty.Parse();
            }
            else
            {

                using (excel.AutoRestoreMoveRight())
                {
                    var excelGivenRight = excel.CurrentCellRaw();
                    var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                    if (IsList(excelGivenLeft))
                    {
                        Parse(startCellReference, excelGivenLeft, excelGivenRightString);
                    }
                    else
                    {
                        var cSharpMethodName = converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

                        code.Add($".{cSharpMethodName}({converter.PropertyValueExcelToCode(excelGivenLeft, excelGivenRight)})");

                        VisitGivenSimplePropertyOrFunction(
                            excelGivenLeft,
                            excelGivenRight);
                    }
                }
            }
        }

        internal void Parse(string startCellReference, string excelGivenLeft, string excelGivenRightString)
        {
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

        // check to see if it looks like a table, but does not end with converter.ListOf
        void CheckMissingListOf()
        {
            if (LooksLikeAListButIsnt())
                AddErrorToCodeAndLog($"It looks like you might be trying to set up a list property, starting at cell {excel.CellReferenceA1Style()}. If this is the case, please make sure that cell {excel.CellReferenceA1Style()} ends with '{converter.ListOf}'");
        }

        bool LooksLikeAListButIsnt() =>
            IsList(excel.CurrentCell()) == false
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
