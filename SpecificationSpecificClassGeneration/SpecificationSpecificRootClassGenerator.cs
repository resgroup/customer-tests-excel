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

            var simplePropertyDeclarations = SimplePropertyDeclarations(excelGivenClass);

            var simpleProperties = SimpleProperties(excelGivenClass);

            var complexPropertyDeclarations = ComplexPropertyDeclarations(excelGivenClass);

            var complexProperties = ComplexProperties(excelGivenClass);

            var listPropertyDeclarations = ListPropertyDeclarations(excelGivenClass);

            var listPropertyMockSetups = ListPropertyInitialisation(excelGivenClass);

            var listPropertyFunctions = ListPropertyFunctions(excelGivenClass);

            return
$@"{usingStatements}

namespace {testNamespace}.GeneratedSpecificationSpecific
{{
    public partial class {SpecificationSpecificClassName} : ReportsSpecificationSetup
    {{
{string.Join(NewLine, simplePropertyDeclarations)}

{string.Join(NewLine, complexPropertyDeclarations)}

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

        IEnumerable<string> SimplePropertyDeclarations(GivenClass excelGivenClass)
        {
            return excelGivenClass
                .Properties
                .Where(excelProperty => IsSimpleProperty(excelProperty.Type))
                .Select(SimplePropertyDeclaration);
        }

        string SimplePropertyDeclaration(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);

            return $"        public string {parameterName} {{ get; private set; }}";
        }

        IEnumerable<string> SimpleProperties(GivenClass excelGivenClass)
        {
            return excelGivenClass
                .Properties
                .Where(excelProperty => IsSimpleProperty(excelProperty.Type))
                .Select(SimplePropertySetter);
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

        string SimplePropertySetter(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);
            var parameterType = CsharpPropertyTypeName(excelGivenProperty.Type);
            var interfacePropertyName = excelGivenProperty.Name;

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({parameterType} {parameterName})
        {{
            valueProperties.Add(GetCurrentMethod(), {parameterName});

            this.{parameterName} = {parameterName};

            return this;
        }}{NewLine}";
        }

        IEnumerable<string> ComplexPropertyDeclarations(GivenClass excelGivenClass)
        {
            return excelGivenClass
                .Properties
                .Where(excelProperty => excelProperty.Type == ExcelPropertyType.Object)
                .Select(ComplexPropertyDeclaration);
        }

        string ComplexPropertyDeclaration(IGivenClassProperty excelGivenProperty)
        {
            var variableName = CamelCase(excelGivenProperty.Name);
            var variableType = $"SpecificationSpecific{excelGivenProperty.ClassName}";

            return $"        public {variableType} {variableName} {{ get; private set; }}";
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
            var classVariableName = CamelCase(excelGivenProperty.Name);
            var parameterName = CamelCase(excelGivenProperty.Name);
            var propertyClassName = $"SpecificationSpecific{excelGivenProperty.ClassName}";

            return
$@"        internal {SpecificationSpecificClassName} {functionName}({propertyClassName} {parameterName})
        {{
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            this.{classVariableName} = {parameterName};

            return this;
        }}{NewLine}";
        }

        IEnumerable<string> ListPropertyDeclarations(GivenClass excelGivenClass) =>
            ListProperties(excelGivenClass).Select(ListPropertyDeclaration);

        string ListPropertyDeclaration(IGivenClassProperty excelProperty)
        {
            var listClassName = $"SpecificationSpecific{excelProperty.ClassName}";
            var listPropertyName = ListPropertyName(excelProperty);

            return $"        readonly List<{listClassName}> {listPropertyName};";
        }

        IEnumerable<string> ListPropertyInitialisation(GivenClass excelGivenClass) =>
            ListProperties(excelGivenClass).Select(ListPropertyInitialisation);

        string ListPropertyInitialisation(IGivenClassProperty excelProperty)
        {
            var listClassName = $"SpecificationSpecific{excelProperty.ClassName}";
            var listPropertyName = ListPropertyName(excelProperty);

            return $"            {listPropertyName} = new List<{listClassName}>();";
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

        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_list_of(List<{listClassName}> {listParameterName}, string listType)
        {{
            listProperties.Add(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, {listParameterName}));

            this.{listPropertyName}.AddRange({listParameterName});

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