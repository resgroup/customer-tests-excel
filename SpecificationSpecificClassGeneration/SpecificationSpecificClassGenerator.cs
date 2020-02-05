using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Environment;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    struct ListProperty
    {
        public PropertyInfo CsharpProperty;
        public IGivenClassProperty ExcelProperty;
    }

    public class SpecificationSpecificClassGenerator
    {
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        GivenClass excelGivenClass;
        Type type;

        public SpecificationSpecificClassGenerator(
            ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher)
        {
            this.excelCsharpPropertyMatcher = excelCsharpPropertyMatcher;
        }


        public string cSharpCode(
            string testNamespace,
            List<string> usings,
            Type type,
            GivenClass excelGivenClass)
        {
            this.excelGivenClass = excelGivenClass;
            this.type = type;

            var usingStatements = UsingStatements(usings);

            var simpleProperties = SimpleProperties(type, excelGivenClass);

            var listPropertyDeclarations = ListPropertyDeclarations(type, excelGivenClass);

            var listPropertyMockSetups = ListPropertyMockSetups(type, excelGivenClass);

            var listPropertyFunctions = ListPropertyFunctions(type, excelGivenClass);

            return
$@"{usingStatements}

namespace {testNamespace}
{{
    internal class {SpecificationSpecificClassName} : ReportsSpecificationSetup
    {{
        readonly Mock<{MockInterfaceName}> {MockVariableName};

        public {MockInterfaceName} {InterfacePropertyName} =>
            {MockVariableName}.Object;

{string.Join(NewLine, listPropertyDeclarations)}

        public {SpecificationSpecificClassName}()
        {{
            {MockVariableName} = new Mock<{MockInterfaceName}>();

{string.Join(NewLine, listPropertyMockSetups)}
        }}

{string.Join(NewLine, simpleProperties)}

{string.Join(NewLine, listPropertyFunctions)}
    }}
}}
";
        }

        static string UsingStatements(List<string> usings)
        {
            var allUsings =
                new List<String>
                {
                    "static System.Reflection.MethodBase",
                    "Moq",
                    "CustomerTestsExcel"
                };
            allUsings.AddRange(usings);

            var usingStatements = string.Join(NewLine, allUsings.Select(u => $"using {u};"));
            return usingStatements;
        }

        List<string> SimpleProperties(Type type, GivenClass excelGivenClass)
        {
            var simpleProperties = new List<string>();
            foreach (var cSharpProperty in type.GetProperties())
            {
                var excelProperty =
                    excelGivenClass
                    .Properties
                    .FirstOrDefault(p => excelCsharpPropertyMatcher.PropertiesMatch(cSharpProperty, p));

                // should probably add this as a function on IGivenClassProperty
                var excelPropertyType = excelProperty?.Type;
                if (excelPropertyType == ExcelPropertyType.Boolean
                    || excelPropertyType == ExcelPropertyType.DateTime
                    || excelPropertyType == ExcelPropertyType.Decimal
                    || excelPropertyType == ExcelPropertyType.Enum
                    || excelPropertyType == ExcelPropertyType.Null
                    || excelPropertyType == ExcelPropertyType.Number
                    || excelPropertyType == ExcelPropertyType.String
                    || excelPropertyType == ExcelPropertyType.StringNull
                    || excelPropertyType == ExcelPropertyType.TimeSpan
                    )
                    simpleProperties.Add(PropertySetter(cSharpProperty, excelProperty));

            }

            return simpleProperties;
        }

        string PropertySetter(PropertyInfo propertyInfo, IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);
            var parameterType = propertyInfo.PropertyType.Name;
            var interfacePropertyName = excelGivenProperty.Name;

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({parameterType} {parameterName})
        {{
            valueProperties.Add(GetCurrentMethod(), {parameterName});

            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({parameterName});

            return this;
        }}{NewLine}";
        }

        IEnumerable<string> ListPropertyDeclarations(Type type, GivenClass excelGivenClass) =>
            ListProperties(type, excelGivenClass).Select(ListPropertyDeclaration);

        string ListPropertyDeclaration(ListProperty listProperty)
        {
            var listClassName = $"SpecificationSpecific{ listProperty.ExcelProperty.ClassName}";
            var listPropertyName = ListPropertyName(listProperty.ExcelProperty);

            return $"        readonly List<{listClassName}> {listPropertyName} = new List<{listClassName}>();";
        }

        IEnumerable<string> ListPropertyMockSetups(Type type, GivenClass excelGivenClass) =>
            ListProperties(type, excelGivenClass).Select(ListPropertyMockSetup);

        string ListPropertyMockSetup(ListProperty listProperty)
        {
            var interfacePropertyName = listProperty.ExcelProperty.Name;
            var dotToSpecificType = DotToSpecificType(listProperty.CsharpProperty);
            var listPropertyName = ListPropertyName(listProperty.ExcelProperty);

            return $"            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({listPropertyName}.Select(l => l.value){dotToSpecificType});";
        }

        string DotToSpecificType(PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;

            if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(IList<>)))
                return ".ToList()";

            if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(List<>)))
                return ".ToList()";

            return "";
        }

        IEnumerable<string> ListPropertyFunctions(Type type, GivenClass excelGivenClass)
        {
            return
                ListProperties(type, excelGivenClass)
                .Select(listProperty => ListPropertySetter(listProperty.ExcelProperty));
        }

        string ListPropertySetter(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);
            var listParameterName = ListPropertyName(excelGivenProperty);
            var listPropertyName = ListPropertyName(excelGivenProperty);
            var listClassName = $"SpecificationSpecific{excelGivenProperty.ClassName}";

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({listClassName} {parameterName})
        {{
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            this.{listPropertyName}.Add({parameterName});

            return this;
        }}

        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_table_of(ReportSpecificationSetupClassUsingTable<{listClassName}> {listParameterName})
        {{
            {listParameterName}.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add({listParameterName});

            foreach (var row in {listParameterName}.Rows)
                this.{listPropertyName}.Add(row.Properties);

            return this;
        }}";
        }

        IEnumerable<ListProperty> ListProperties(Type type, GivenClass excelGivenClass)
        {
            foreach (var cSharpProperty in type.GetProperties())
            {
                var excelProperty =
                    excelGivenClass
                    .Properties
                    .FirstOrDefault(p => excelCsharpPropertyMatcher.PropertiesMatch(cSharpProperty, p));

                if (excelProperty?.Type == ExcelPropertyType.List)
                    yield return new ListProperty { CsharpProperty = cSharpProperty, ExcelProperty = excelProperty };
            }
        }

        string InterfacePropertyName =>
       excelGivenClass.Name;

        string MockInterfaceName =>
        type.Name;

        string MockVariableName =>
        CamelCase(excelGivenClass.Name);

        string SpecificationSpecificClassName =>
         $"SpecificationSpecific{excelGivenClass.Name}";

        string CamelCase(string pascalCase) =>
            string.IsNullOrWhiteSpace(pascalCase) ? "" : char.ToLower(pascalCase[0]) + pascalCase.Substring(1);

        string ListPropertyName(IGivenClassProperty excelProperty) =>
            CamelCase(excelProperty.Name) + "s";


    }
}