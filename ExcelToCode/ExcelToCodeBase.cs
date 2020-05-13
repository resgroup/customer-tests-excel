using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeBase
    {
        protected readonly ExcelToCodeState excelToCodeState;
        public LogState log;
        protected ExcelState excel;
        protected CodeState code;
        protected readonly ICodeNameToExcelNameConverter converter;
        protected string sutName;

        public ExcelToCodeBase(ExcelToCodeState excelToCodeState)
        {
            converter = excelToCodeState.Converter;
            log = excelToCodeState.Log;
            code = excelToCodeState.Code;
            excel = excelToCodeState.Excel;
            this.excelToCodeState = excelToCodeState;
        }

        public void AddVisitor(IExcelToCodeVisitor visitor) =>
            log.AddVisitor(visitor);

        public void VisitGivenSimplePropertyOrFunction(
            string excelGivenLeft,
            object excelGivenRight)
        {
            if (excelGivenLeft.EndsWith(" of"))
            {
                log.VisitGivenSimpleProperty(
                    new GivenSimpleProperty(
                        converter.GivenPropertyNameExcelNameToSutName(excelGivenLeft),
                        converter.PropertyValueExcelToCode(excelGivenLeft, excelGivenRight),
                        converter.ExcelPropertyTypeFromCellValue(excelGivenRight)));
            }
            else
            {
                log.VisitGivenFunction(
                    new GivenFunction(excelGivenLeft));
            }
        }

        protected string SUTClassName()
        {
            if (sutName == null) throw new Exception("Trying to read _sutName before it has been set");

            return sutName;
        }

        protected string CSharpSUTSpecificationSpecificClassName() =>
            converter.ExcelClassNameToCodeName(SUTClassName());

        protected string CSharpSUTVariableName() =>
            VariableCase(SUTClassName());

        protected string VariableCase(string camelCase) =>
            // it is assumed to already be in camel case, this means making the first letter lower case
            // this isn't a two way process (eg the conversion process doesn't care what the string is) so this isn't in the _namer
            string.IsNullOrWhiteSpace(camelCase) ? "" : char.ToLower(camelCase[0]) + camelCase.Substring(1);

        protected static string LeadingComma(int index) =>
            (index == 0) ? " " : ",";

        protected void AddErrorToCodeAndLog(string message)
        {
            // this will appear at the relevant point in the generated code
            code.Add($"// {message}");

            // this can be used elsewhere, such as in the console output of the test generation
            log.errors.Add(message);
        }
    }
}
