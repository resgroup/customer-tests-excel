using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeComplexProperty : ExcelToCodeBase
    {
        readonly ExcelToCodeTable excelToCodeTable;

        public ExcelToCodeComplexProperty(
            ICodeNameToExcelNameConverter converter,
            LogState log,
            CodeState code,
            ExcelState excel)
            : base(
                  converter,
                  log,
                  code,
                  excel
                  )
        {
            excelToCodeTable = new ExcelToCodeTable(
                  converter,
                  log,
                  code,
                  excel
                );
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

        // todo: remove this
        bool IsList(string excelGivenLeft) =>
            excelGivenLeft.EndsWith(converter.ListOf, StringComparison.InvariantCultureIgnoreCase);

        // todo: remove this
        void CheckMissingListOf()
        {
            if (LooksLikeAListButIsnt())
                AddErrorToCodeAndLog($"It looks like you might be trying to set up a list property, starting at cell {excel.CellReferenceA1Style()}. If this is the case, please make sure that cell {excel.CellReferenceA1Style()} ends with '{converter.ListOf}'");
        }

        // todo: remove this
        bool LooksLikeAListButIsnt() =>
            IsList(excel.CurrentCell()) == false
            && excel.PeekBelowRight() == converter.WithItem;

        // todo: remove this
        void CheckMissingWithItemForList(string listStartCellReference)
        {
            using (excel.AutoRestoreMoveDown())
            {
                if (excel.CurrentCell() != converter.WithItem)
                    throw new ExcelToCodeException($"The list property starting at {listStartCellReference} is not formatted correctly. Cell {excel.CellReferenceA1Style()} should be '{converter.WithItem}', but is '{excel.CurrentCell()}'");
            }
        }

        // todo: remove this
        string ListVariableNameFromMethodName(string excelGivenLeft) =>
            VariableCase(converter.GivenListPropertyNameExcelNameToCodeVariableName(excelGivenLeft)) + "List";

        // todo: remove this
        string ListItemVariableNameFromMethodName(string excelGivenLeft) =>
            VariableCase(converter.GivenListPropertyNameExcelNameToCodeVariableName(excelGivenLeft));

        // probably want to move this in to this class as well
        // there is probably also no reason why the root class has to be a complex object
        // presumably a list of a table should be valid as well
        // this might not work at the moment, with the setup class generation and other things
        // but we should probably output an error / warning explaining the case
        //void CreateRootObject(string excelClassName)
        //{
        //    log.VisitGivenRootClassDeclaration(excelClassName);

        //    code.Add("return");
        //    using (code.AutoCloseIndent())
        //    {
        //        CreateObjectWithoutVisiting(excelClassName);
        //    }
        //    code.Add(";");

        //    log.VisitGivenRootClassFinalisation();
        //}

        internal void CreateObject(string excelPropertyName, string excelClassName)
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

            SetVariableProperties();

            excel.MoveUp(); // this is a bit mysterious
        }

        void SetVariableProperties()
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

        void DoProperty()
        {
            CheckMissingListOf();
            var startCellReference = excel.CellReferenceA1Style();
            var excelGivenLeft = excel.CurrentCell();

            if (excelToCodeTable.CanParse())
            {
                excelToCodeTable.Parse();
            }
            else if (CanParse())
            {
                Parse();
            }
            else
            {

                using (excel.AutoRestoreMoveRight())
                {
                    var excelGivenRight = excel.CurrentCellRaw();
                    var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                    if (IsList(excelGivenLeft))
                    {
                        CheckMissingWithItemForList(startCellReference);

                        var cSharpMethodName = converter.GivenListPropertyNameExcelNameToCodeName(excelGivenLeft);
                        var cSharpClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);
                        string cSharpListVariableName = ListVariableNameFromMethodName(excelGivenLeft);
                        string cSharpListItemVariableName = ListItemVariableNameFromMethodName(excelGivenLeft);

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
    }
}
