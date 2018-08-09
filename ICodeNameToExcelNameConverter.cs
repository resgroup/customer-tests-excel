using System;
namespace CustomerTestsExcel
{
    // This interface is to handle the 2 way conversion to and from Excel and code
    // It defines the method of changing stuff
    //  eg a property name into a name in excel and vice versa (eg "_" coud be replaced with " ", "CamelCasing" could be replaced with "Camel Casing" etc and vice versa)
    // it defines the parsing grammar so that it is used by both the writer from c# out to excel and the reader from excel to c#
    //  eg "Given A" is used in Excel
    public interface ICodeNameToExcelNameConverter
    {
        string Specification { get; }
        string Given { get; }
        string Properties { get; }
        string When { get; }
        string WhenValidating { get; }
        string WhenCalculating { get; }
        string Assert { get; }

        string ExcelFileNameToCodeNamespacePart(string workBookName);
        string CodeNamespaceToExcelFileName(string cSharpAssemblyName);

        string ExcelClassNameToCodeName(string excelSpecificationName);
        string CodeClassNameToExcelName(string cSharpClassName);

        string CodeSpecificationClassNameToExcelName(string cSharpClassName);
        string ExcelSpecificationNameToCodeSpecificationClassName(string excelSpecificationName);

        string GivenPropertyNameCodeNameToExcelName(string cSharpPropertyName, bool isChild, int? indexInParent);
        string GivenPropertyNameExcelNameToCodeName(string excelPropertyName);

        string ActionCodeNameToExcelName(string actionName);

        string AssertPropertyCodeNameToExcelName(string cSharpAssertName);
        string AssertPropertyExcelNameToCodeName(string excelAssertName);

        object AssertValueCodeNameToExcelName(object cSharpAssertValue);
        string AssertValueExcelNameToCodeName(string excelPropertyName, object excelAssertValue);

        string AssertionSubPropertyCodePropertyNameToExcelName(string cSharpAssertPropertyName);
        string AssertionSubPropertyExcelNameToCodeMethodName(string excelAssertPropertyName);

        string AssertionSubPropertyCodeClassNameToExcelName(string cSharpAssertClassName);
        string AssertionSubPropertyExcelNameToCodeClassName(string excelAssertClassName);

        object PropertyValueCodeToExcel(string csharpNameNamespace, object cSharpPropertyValue);
        string PropertyValueExcelToCode(string excelPropertyName, object excelPropertyValue);

        string AssertionOperatorCodeNameToExcelName(AssertionOperator assertionOperator);

        string AssertionSubClassExcelNameToCodeName(string excelClassName);
    }
}
