using System;
using System.Collections.Generic;
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

        float PercentMatchingProperties(Type interfaceType, GivenClass excelClass)
        {
            if (excelClass.Properties.Any())
            {
                // this gets things in ancestor interfaces as well as directly on the interface
                var properties =
                    (new Type[] { interfaceType })
                    .Concat(interfaceType.GetInterfaces())
                    .SelectMany(i => i.GetProperties());

                var methods =
                    (new Type[] { interfaceType })
                    .Concat(interfaceType.GetInterfaces())
                    .SelectMany(i => i.GetMethods());

                return
                    (float) excelClass.Properties.Count(
                        excelProperty =>
                            MatchesAnyCsharpPropertyOrFunction(
                                excelProperty,
                                properties,
                                methods)
                    )
                    / (float) excelClass.Properties.Count();
            }
            else
                return 1;
        }

        bool MatchesAnyCsharpPropertyOrFunction(
            IGivenClassProperty excelProperty,
            IEnumerable<PropertyInfo> properties,
            IEnumerable<MethodInfo> methods
            )
        {
            return
                properties.Any(
                    cSharpProperty =>
                        excelCsharpPropertyMatcher.PropertiesMatch(cSharpProperty, excelProperty)
                )
                || methods.Any(
                    cSharpMethod =>
                        excelCsharpPropertyMatcher.MethodsMatch(cSharpMethod, excelProperty)
                );
        }
    }
}