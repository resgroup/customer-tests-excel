using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeComplexProperty : ExcelToCodeBase
    {
        ExcelToCodeTable excelToCodeTable;

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
        }

        internal bool HasGivenSubProperties() =>
            excel.PeekBelow() == converter.WithProperties;

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

        void CreateObject(string excelPropertyName, string excelClassName)
        {
            log.VisitGivenComplexPropertyDeclaration(
                converter.GivenPropertyNameExcelNameToSutName(excelPropertyName),
                excelClassName);

            CreateObjectWithoutVisiting(excelClassName);

            log.VisitGivenComplexPropertyFinalisation();
        }

        void CreateObjectWithoutVisiting(string excelClassName)
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
                        //todo: DoProperty();
                        excel.MoveDown();
                    }
                }
            }
        }

        //void DoProperty()
        //{
        //    CheckMissingListOf();
        //    var startCellReference = excel.CellReferenceA1Style();
        //    var excelGivenLeft = excel.CurrentCell();

        //    if (excelToCodeTable.CanParse())
        //    {
        //        excelToCodeTable.Parse();
        //    }
        //    else
        //    {

        //        using (excel.AutoRestoreMoveRight())
        //        {
        //            var excelGivenRight = excel.CurrentCellRaw();
        //            var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

        //            if (IsList(excelGivenLeft))
        //            {
        //                CheckMissingWithItemForList(startCellReference);

        //                var cSharpMethodName = converter.GivenListPropertyNameExcelNameToCodeName(excelGivenLeft);
        //                var cSharpClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);
        //                string cSharpListVariableName = ListVariableNameFromMethodName(excelGivenLeft);
        //                string cSharpListItemVariableName = ListItemVariableNameFromMethodName(excelGivenLeft);

        //                code.BlankLine();
        //                using (excel.AutoRestoreMoveDown())
        //                {
        //                    log.VisitGivenListPropertyDeclaration(
        //                        converter.GivenListPropertyNameExcelNameToCodeVariableName(excelGivenLeft),
        //                        excelGivenRightString);

        //                    using (code.OutputAndOpenAutoClosingBracket($".{cSharpMethodName}"))
        //                    {
        //                        code.Add($"\"{cSharpClassName}\", ");
        //                        code.Add($"new FluentList<{cSharpClassName}>()");
        //                        while (excel.CurrentCell() == converter.WithItem)
        //                        {
        //                            excel.MoveDown();

        //                            // Add an item to the list
        //                            using (excel.AutoRestoreMoveRight())
        //                            {
        //                                using (code.OutputAndOpenAutoClosingBracket($".FluentAdd"))
        //                                {
        //                                    code.Add($"new {cSharpClassName}()");

        //                                    while (!string.IsNullOrEmpty(excel.CurrentCell()))
        //                                    {
        //                                        DoProperty();
        //                                        excel.MoveDown();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    log.VisitGivenListPropertyFinalisation();
        //                }
        //            }
        //            else if (HasGivenSubProperties())
        //            {
        //                var cSharpMethodName = converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

        //                code.BlankLine();

        //                using (code.OutputAndOpenAutoClosingBracket($".{cSharpMethodName}"))
        //                    CreateObject(excelGivenLeft, excelGivenRightString);
        //            }
        //            else
        //            {
        //                var cSharpMethodName = converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

        //                code.Add($".{cSharpMethodName}({converter.PropertyValueExcelToCode(excelGivenLeft, excelGivenRight)})");

        //                VisitGivenSimplePropertyOrFunction(
        //                    excelGivenLeft,
        //                    excelGivenRight);
        //            }
        //        }
        //    }
        //}



    }
}
