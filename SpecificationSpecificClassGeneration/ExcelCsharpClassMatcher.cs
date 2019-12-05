using System;
using System.Linq;
using System.Reflection;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class ExcelCsharpClassMatcher
    {
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;

        public ExcelCsharpClassMatcher(ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher)
        {
            this.excelCsharpPropertyMatcher = excelCsharpPropertyMatcher ?? throw new ArgumentNullException(nameof(excelCsharpPropertyMatcher));
        }

        public bool Matches(Type type, GivenClass simpleExcelClass) =>
            ClassNameMatcher.NamesMatch(type.Name, simpleExcelClass.Name)
            && PropertiesOrFunctionsMatch(type, simpleExcelClass);


        bool PropertiesOrFunctionsMatch(Type type, GivenClass simpleExcelClass) =>
            simpleExcelClass.Properties.All(
                excelProperty =>
                    MatchesAnyCsharpPropertyOrFunction(type, excelProperty)
            );

        bool MatchesAnyCsharpPropertyOrFunction(
            Type type,
            IGivenClassProperty excelProperty)
        {
            return
                type.GetProperties().Any(
                    cSharpProperty =>
                        excelCsharpPropertyMatcher.PropertiesMatch(cSharpProperty, excelProperty)
                )
                || type.GetMethods().Any(
                    cSharpMethod =>
                        excelCsharpPropertyMatcher.MethodsMatch(cSharpMethod, excelProperty)
                );
        }
    }
}