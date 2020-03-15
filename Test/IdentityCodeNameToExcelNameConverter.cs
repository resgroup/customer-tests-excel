using CustomerTestsExcel.Assertions;
using CustomerTestsExcel.ExcelToCode;

namespace CustomerTestsExcel.Test
{
    // This class aims to do no formatting or anything, to make the relationship between
    // the excel and the generated code more direct, which makes testing simpler / clearer.
    // It actually isn't used at the moment, as ExcelToCode also does some of its own
    // formatting, which means that it isn't that useful. I think this means that the
    // interface and ExcelToCode need a little refactor to clarify their responsibilities.
    internal class IdentityCodeNameToExcelNameConverter : ICodeNameToExcelNameConverter
    {
        // these are copied straight from CodeNameToExcelNameConverter, which
        // seems like a code smell. I think this means that ICodeNameToExcelNameConverter
        // fails the Interface Segregation Principle, and so should be split up into
        // multiple, more specific, interfaces.
        public string Specification =>
            "Specification";

        public string Given =>
            "Given a";

        public string WithProperties =>
            "With Properties";

        public string When =>
            "When";

        public string Assert =>
            "Assert";

        public string TableOf =>
            "table of";

        public string ListOf =>
            "list of";

        public string WithItem =>
            "With Item";

        public string AssertionClassPrefixAddedByGenerator =>
            "";

        public string ActionCodeNameToExcelName(string actionName) => actionName;
        public string ActionExcelNameToCodeName(string excelActionName) => excelActionName;
        public string AssertionOperatorCodeNameToExcelName(AssertionOperator assertionOperator) => assertionOperator.ToDescription();
        public string AssertionSubClassExcelNameToCodeName(string excelClassName) => excelClassName;
        public string AssertionSubPropertyCodeClassNameToExcelName(string cSharpAssertClassName) => cSharpAssertClassName;
        public string AssertionSubPropertyCodePropertyNameToExcelName(string cSharpAssertPropertyName) => cSharpAssertPropertyName;
        public string AssertionSubPropertyExcelNameToCodeClassName(string excelAssertClassName) => excelAssertClassName;
        public string AssertionSubPropertyExcelNameToCodeMethodName(string excelAssertPropertyName) => excelAssertPropertyName;
        public string AssertPropertyCodeNameToExcelName(string cSharpAssertName) => cSharpAssertName;
        public string AssertPropertyExcelNameToCodeName(string excelAssertName) => excelAssertName;
        public object AssertValueCodeNameToExcelName(object cSharpAssertValue) => cSharpAssertValue;
        public string AssertValueExcelNameToCodeName(string excelPropertyName, object excelAssertValue) => excelAssertValue.ToString();
        public string CodeClassNameToExcelName(string cSharpClassName) => cSharpClassName;
        public string CodeNamespaceToExcelFileName(string cSharpAssemblyName) => cSharpAssemblyName;
        public string CodeSpecificationClassNameToExcelName(string cSharpClassName) => cSharpClassName;
        public string ExcelClassNameToCodeName(string excelSpecificationName) => excelSpecificationName;
        public string ExcelFileNameToCodeNamespacePart(string workBookName) => workBookName;
        public ExcelPropertyType ExcelPropertyTypeFromCellValue(object excelPropertyValue) => ExcelPropertyType.Null;
        public string ExcelSpecificationNameToCodeSpecificationClassName(string excelSpecificationName) => excelSpecificationName;
        public string GivenListPropertyNameCodeNameToExcelName(string cSharpPropertyName) => cSharpPropertyName;
        public string GivenListPropertyNameExcelNameToCodeName(string excelPropertyName) => excelPropertyName;
        public string GivenListPropertyNameExcelNameToCodeVariableName(string excelPropertyName) => excelPropertyName;
        public string GivenPropertyNameCodeNameToExcelName(string cSharpPropertyName) => cSharpPropertyName;
        public string GivenPropertyNameExcelNameToCodeName(string excelPropertyName) => excelPropertyName;
        public string GivenTablePropertyNameCodeNameToExcelName(string cSharpPropertyName) => cSharpPropertyName;
        public string GivenTablePropertyNameExcelNameToCodeName(string excelPropertyName) => excelPropertyName;
        public string GivenTablePropertyNameExcelNameToCodeVariableName(string excelPropertyName) => excelPropertyName;
        public object PropertyValueCodeToExcel(string csharpNameNamespace, object cSharpPropertyValue) => cSharpPropertyValue;
        public string PropertyValueExcelToCode(string excelPropertyName, object excelPropertyValue) => excelPropertyValue.ToString();
        public string RemoveExcelOfPostfix(string excelPropertyName) => excelPropertyName;
    }
}

