using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections;
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

            var unmatchedProperties = UnmatchedProperties(excelGivenClass);

            var simpleProperties = SimpleProperties(excelGivenClass);

            var complexProperties = ComplexProperties(excelGivenClass);

            var listPropertyDeclarations = ListPropertyDeclarations(excelGivenClass);

            var listPropertyMockSetups = ListPropertyMockSetups(excelGivenClass);

            var listPropertyFunctions = ListPropertyFunctions(excelGivenClass);

            return
$@"{usingStatements}

namespace {testNamespace}.GeneratedSpecificationSpecific
{{
    public partial class {SpecificationSpecificClassName} : ReportsSpecificationSetup
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

{string.Join(NewLine, unmatchedProperties)}

{string.Join(NewLine, simpleProperties)}

{string.Join(NewLine, complexProperties)}

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
                    // this is required for "String". Using "string" would be
                    // better, but String is what the type system gives us
                    "System",
                    "System.Collections.Generic",
                    "System.Linq",
                    "static System.Reflection.MethodBase",
                    "Moq",
                    "CustomerTestsExcel",
                    "CustomerTestsExcel.SpecificationSpecificClassGeneration"
                };
            allUsings.AddRange(usings);

            var usingStatements = string.Join(NewLine, allUsings.Select(u => $"using {u};"));
            return usingStatements;
        }

        IEnumerable<string> SimpleProperties(GivenClass excelGivenClass)
        {
            foreach (var excelProperty in excelGivenClass.Properties)
            {
                var cSharpProperty =
                    type.GetProperties()
                    .FirstOrDefault(c => excelCsharpPropertyMatcher.PropertiesMatch(c, excelProperty));

                if (excelProperty.Type.IsSimpleProperty())
                {
                    yield return SimplePropertySetter(
                        cSharpProperty.PropertyType.Name,
                        excelProperty);
                }
            }
        }

        string SimplePropertySetter(string propertyTypeName, IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);
            var parameterType = propertyTypeName;
            var interfacePropertyName = excelGivenProperty.Name;

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({parameterType} {parameterName})
        {{
            valueProperties.Add(GetCurrentMethod(), {parameterName});

            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({parameterName});

            return this;
        }}{NewLine}";
        }

        IEnumerable<string> ComplexProperties(GivenClass excelGivenClass)
        {
            foreach (var excelProperty in excelGivenClass.Properties)
            {
                var cSharpProperty =
                    type.GetProperties()
                    .FirstOrDefault(c => excelCsharpPropertyMatcher.PropertiesMatch(c, excelProperty));

                if (cSharpProperty != null && excelProperty.Type == ExcelPropertyType.Object)
                    yield return ComplexPropertySetter(excelProperty);
            }
        }

        string ComplexPropertySetter(IGivenClassProperty excelGivenProperty)
        {
            var functionName = $"{excelGivenProperty.Name}_of";
            var parameterName = CamelCase(excelGivenProperty.Name);
            var interfacePropertyName = excelGivenProperty.Name;
            var propertyClassName = $"SpecificationSpecific{excelGivenProperty.ClassName}";
            var propertyNameOfSutObject = excelGivenProperty.Name;

            return
$@"        internal {SpecificationSpecificClassName} {functionName}({propertyClassName} {parameterName})
        {{
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({parameterName}.{propertyNameOfSutObject});

            return this;
        }}{NewLine}";
        }

        IEnumerable<string> ListPropertyDeclarations(GivenClass excelGivenClass) =>
            ListProperties(excelGivenClass).Select(ListPropertyDeclaration);

        string ListPropertyDeclaration(ListProperty listProperty)
        {
            var listClassName = $"SpecificationSpecific{ listProperty.ExcelProperty.ClassName}";
            var listPropertyName = ListPropertyName(listProperty.ExcelProperty);

            return $"        readonly List<{listClassName}> {listPropertyName} = new List<{listClassName}>();";
        }

        IEnumerable<string> ListPropertyMockSetups(GivenClass excelGivenClass) =>
            ListProperties(excelGivenClass).Select(ListPropertyMockSetup);

        string ListPropertyMockSetup(ListProperty listProperty)
        {
            var interfacePropertyName = listProperty.ExcelProperty.Name;
            var listPropertyName = ListPropertyName(listProperty.ExcelProperty);
            var interfaceUnderTestPropertyName = listProperty.ExcelProperty.ClassName;

            return $"            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({listPropertyName}.Select(l => l.{interfaceUnderTestPropertyName}));";
        }

        IEnumerable<string> ListPropertyFunctions(GivenClass excelGivenClass)
        {
            return
                ListProperties(excelGivenClass)
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

        IEnumerable<ListProperty> ListProperties(GivenClass excelGivenClass)
        {
            foreach (var excelProperty in excelGivenClass.Properties)
            {
                var cSharpProperty =
                    type.GetProperties()
                    .FirstOrDefault(c => excelCsharpPropertyMatcher.PropertiesMatch(c, excelProperty));

                if (excelProperty.Type == ExcelPropertyType.List 
                    && cSharpProperty != null)
                    yield return new ListProperty { CsharpProperty = cSharpProperty, ExcelProperty = excelProperty };
            }
        }

        IEnumerable<string> UnmatchedProperties(GivenClass excelGivenClass)
        {
            foreach (var excelProperty in excelGivenClass.Properties)
            {
                var cSharpProperty =
                    type.GetProperties()
                    .FirstOrDefault(c => excelCsharpPropertyMatcher.PropertiesMatch(c, excelProperty));

                if (cSharpProperty == null)
                    yield return $"// Could not find a match for property {excelProperty.Name}, with type of {excelProperty.Type}";
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