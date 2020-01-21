using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGenerator
    {
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        readonly ICodeNameToExcelNameConverter codeNameToExcelNameConverter;
        GivenClass excelGivenClass;

        public SpecificationSpecificClassGenerator(
            ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher,
            ICodeNameToExcelNameConverter codeNameToExcelNameConverter)
        {
            this.excelCsharpPropertyMatcher = excelCsharpPropertyMatcher;
            this.codeNameToExcelNameConverter = codeNameToExcelNameConverter;
        }


        public string cSharpCode(
            string testNamespace,
            List<string> usings,
            Type type,
            GivenClass excelGivenClass)
        {
            this.excelGivenClass = excelGivenClass;

            var allUsings =
                new List<String>
                {
                    "static System.Reflection.MethodBase",
                    "Moq",
                    "CustomerTestsExcel"
                };
            allUsings.AddRange(usings);

            var usingStatements = string.Join("", allUsings.Select(u => $"using {u};\r\n"));

            var simpleProperties = new List<string>();
            foreach (var cSharpProperty in type.GetProperties())
            {
                var excelProperty = excelGivenClass.Properties.FirstOrDefault(p => excelCsharpPropertyMatcher.PropertiesMatch(cSharpProperty, p));
                if (excelProperty != null)
                    simpleProperties.Add(PropertySetter(cSharpProperty, excelProperty, excelGivenClass));

            }

            var classDefinition =
$@"namespace {testNamespace}
{{
    internal class {SpecificationSpecificClassName} : ReportsSpecificationSetup
    {{
        readonly Mock<{type.Name}> {MockVariableName};

        public {type.Name} {excelGivenClass.Name} =>
            {MockVariableName}.Object;

        public {SpecificationSpecificClassName}()
        {{
            {MockVariableName} = new Mock<{type.Name}>();
        }}

{string.Join("\r\n", simpleProperties)}
    }}
}}
";

            return usingStatements + "\r\n" + classDefinition;
        }

        //string PropertySetter(Type type, GivenClassSimpleProperty excelGivenProperty, GivenClass excelGivenClass)
        string PropertySetter(PropertyInfo propertyInfo, IGivenClassProperty excelGivenProperty, GivenClass excelGivenClass)
        {
            var variableName = CamelCase(excelGivenProperty.Name);

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({propertyInfo.PropertyType.Name} {variableName})
        {{
            valueProperties.Add(GetCurrentMethod(), {variableName});

            {MockVariableName}.Setup(m => m.{propertyInfo.Name}).Returns({variableName});

            return this;
        }}{Environment.NewLine}";
        }

        string MockVariableName =>
            CamelCase(excelGivenClass.Name);

        string SpecificationSpecificClassName =>
             $"SpecificationSpecific{excelGivenClass.Name}";

        string CamelCase(string pascalCase) =>
            string.IsNullOrWhiteSpace(pascalCase) ? "" : char.ToLower(pascalCase[0]) + pascalCase.Substring(1);

    }
}