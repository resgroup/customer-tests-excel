using System;
using System.Linq;
using System.Reflection;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class ClassMatch
    {
        public bool Matches { get; }
        public float PercentMatchingProperties { get; }

        public ClassMatch(bool matches, float percentMatchingProperties)
        {
            Matches = matches;
            PercentMatchingProperties = percentMatchingProperties;
        }
    }

    public class ExcelCsharpClassMatcher
    {
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;

        public ExcelCsharpClassMatcher(ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher)
        {
            this.excelCsharpPropertyMatcher = excelCsharpPropertyMatcher ?? throw new ArgumentNullException(nameof(excelCsharpPropertyMatcher));
        }

        public ClassMatch Matches(Type type, GivenClass simpleExcelClass)
        {
            return new ClassMatch(
                MatchesAtAll(type, simpleExcelClass),
                PercentMatchingProperties(type, simpleExcelClass)
            );
        }

        static bool MatchesAtAll(Type type, GivenClass simpleExcelClass)
        {
            return 
                type.IsInterface
                && ClassNameMatcher.NamesMatch(type.Name, simpleExcelClass.Name);
        }

        float PercentMatchingProperties(Type type, GivenClass simpleExcelClass)
        {
            if (simpleExcelClass.Properties.Any())
                return
                    simpleExcelClass.Properties.Count(
                        excelProperty =>
                            MatchesAnyCsharpPropertyOrFunction(type, excelProperty)
                    )
                    / simpleExcelClass.Properties.Count();
            else
                return 1;
        }
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