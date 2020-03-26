using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Environment;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificRootClassGenerator
    {
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        GivenClass excelGivenClass;

        public SpecificationSpecificRootClassGenerator(
            ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher)
        {
            this.excelCsharpPropertyMatcher = excelCsharpPropertyMatcher;
        }


        public string cSharpCode(
            string testNamespace,
            List<string> usings,
            GivenClass excelGivenClass)
        {
            this.excelGivenClass = excelGivenClass;

            var usingStatements = UsingStatements(usings);

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
{string.Join(NewLine, listPropertyDeclarations)}

        public {SpecificationSpecificClassName}()
        {{
{string.Join(NewLine, listPropertyMockSetups)}
        }}

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
                if (IsSimpleProperty(excelProperty.Type))
                {
                    yield return SimplePropertySetter(
                        CsharpPropertyTypeName(excelProperty.Type),
                        excelProperty);
                }
            }
        }

        string CsharpPropertyTypeName(ExcelPropertyType type)
        {
            switch (type)
            {
                case ExcelPropertyType.Null:
                    return typeof(object).Name;
                case ExcelPropertyType.StringNull:
                    return typeof(string).Name;
                case ExcelPropertyType.Number:
                    return typeof(float).Name;
                case ExcelPropertyType.Decimal:
                    return typeof(decimal).Name;
                case ExcelPropertyType.String:
                    return typeof(string).Name;
                case ExcelPropertyType.DateTime:
                    return typeof(DateTime).Name;
                case ExcelPropertyType.TimeSpan:
                    return typeof(TimeSpan).Name;
                case ExcelPropertyType.Enum:
                    return typeof(Enum).Name;
                case ExcelPropertyType.Boolean:
                    return typeof(bool).Name;
                case ExcelPropertyType.Object:
                    return typeof(object).Name;
                case ExcelPropertyType.List:
                    return typeof(IEnumerable).Name;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            };
        }

        static bool IsSimpleProperty(ExcelPropertyType excelPropertyType)
        {
            return excelPropertyType == ExcelPropertyType.Boolean
                                || excelPropertyType == ExcelPropertyType.DateTime
                                || excelPropertyType == ExcelPropertyType.Decimal
                                || excelPropertyType == ExcelPropertyType.Enum
                                || excelPropertyType == ExcelPropertyType.Null
                                || excelPropertyType == ExcelPropertyType.Number
                                || excelPropertyType == ExcelPropertyType.String
                                || excelPropertyType == ExcelPropertyType.StringNull
                                || excelPropertyType == ExcelPropertyType.TimeSpan;
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
            return excelGivenClass
            .Properties
            .Where(excelProperty => excelProperty.Type == ExcelPropertyType.Object)
            .Select(ComplexPropertySetter);
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

        string ListPropertyDeclaration(IGivenClassProperty excelProperty)
        {
            var listClassName = $"SpecificationSpecific{excelProperty.ClassName}";
            var listPropertyName = ListPropertyName(excelProperty);

            return $"        readonly List<{listClassName}> {listPropertyName} = new List<{listClassName}>();";
        }

        IEnumerable<string> ListPropertyMockSetups(GivenClass excelGivenClass) =>
            ListProperties(excelGivenClass).Select(ListPropertyMockSetup);

        string ListPropertyMockSetup(IGivenClassProperty excelProperty)
        {
            var interfacePropertyName = excelProperty.Name;
            var listPropertyName = ListPropertyName(excelProperty);
            var interfaceUnderTestPropertyName = excelProperty.ClassName;

            return $"            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({listPropertyName}.Select(l => l.{interfaceUnderTestPropertyName}));";
        }

        IEnumerable<string> ListPropertyFunctions(GivenClass excelGivenClass) =>
            ListProperties(excelGivenClass)
            .Select(excelProperty => ListPropertySetter(excelProperty));

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

        IEnumerable<IGivenClassProperty> ListProperties(GivenClass excelGivenClass) =>
            excelGivenClass
            .Properties
            .Where(excelProperty => excelProperty.Type == ExcelPropertyType.List);

        string InterfacePropertyName =>
            excelGivenClass.Name;

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