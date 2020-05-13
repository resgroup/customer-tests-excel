using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
