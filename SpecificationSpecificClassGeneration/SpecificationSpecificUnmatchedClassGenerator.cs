using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Environment;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificUnmatchedClassGenerator
    {
        GivenClass excelGivenClass;

        public string cSharpCode(
            string testNamespace,
            List<string> usings,
            GivenClass excelGivenClass)
        {
            this.excelGivenClass = excelGivenClass;

            var usingStatements = UsingStatements(usings);

            var functions = Functions(excelGivenClass);

            var simplePropertyDeclarations = SimplePropertyDeclarations(excelGivenClass);

            var simpleProperties = SimpleProperties(excelGivenClass);

            var complexPropertyDeclarations = ComplexPropertyDeclarations(excelGivenClass);

            var complexProperties = ComplexProperties(excelGivenClass);

            var listPropertyDeclarations = ListPropertyDeclarations(excelGivenClass);

            var listPropertyInitialisers = ListPropertyInitialisation(excelGivenClass);

            var listPropertyFunctions = ListPropertyFunctions(excelGivenClass);

            return
$@"{usingStatements}

namespace {testNamespace}.GeneratedSpecificationSpecific
{{
    // A class with the name {excelGivenClass.Name} was found in the Excel tests, but
    // no matching interface could be found in the assembliesUnderTest that were
    // specified on the command line parameters to GenerateCodeFromExcelTest. This file
    // is here so that the generated code can compile, to provide an example of
    // the custom file that you need to create, and point out anything that would stop
    // it matching an interface in your system under test.
    
    // The easiest thing to do is to make the names in the Excel match the names of your
    // Interfaces and their properties, and let the framework generate the Specification 
    // Specific setup classes for you. However you can write custom ones if you would
    // prefer, and they do allow you do some more esoteric things, such as instantiating
    // classes instead of mocking interfaces.
    // You can see examples of excel tests and matching interfaces in the SampleTests
    // and SampleSystemUnderTest folders of the framework repository
    // - https://github.com/resgroup/customer-tests-excel/tree/master/SampleTests
    // - https://github.com/resgroup/customer-tests-excel/tree/master/SampleSystemUnderTest

    // Custom classes should go under a directory called 'IgnoreOnGeneration'.
    // If the custom class filename is the same as this one ({SpecificationSpecificClassName}),
    // then it will be used instead of this function. If it is called something else,
    // say {SpecificationSpecificClassName}Parial, then this class will remain, and
    // the custom class can add to it.

    // Please see VermeulenNearWakeLengthInput.cs for an example of setting up simple and
    // complex properties
    // - https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/GeneratedSpecificationSpecific/VermeulenNearWakeLengthInput.cs
    // Please see Group.cs for an example of setting up list / table properties
    // - https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/GeneratedSpecificationSpecific/Cargo.cs

    public partial class {SpecificationSpecificClassName} : ReportsSpecificationSetup
    {{
{string.Join(NewLine, simplePropertyDeclarations)}

{string.Join(NewLine, complexPropertyDeclarations)}

{string.Join(NewLine, listPropertyDeclarations)}

        public {SpecificationSpecificClassName}()
        {{
{string.Join(NewLine, listPropertyInitialisers)}
        }}

{string.Join(NewLine, functions)}

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
                    // better, but "String" is what the type system gives us
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

        IEnumerable<string> Functions(GivenClass excelGivenClass)
        {
            return excelGivenClass
                .Properties
                .Where(excelProperty => excelProperty.Type == ExcelPropertyType.Function)
                .Select(Function);
        }

        string Function(IGivenClassProperty excelGivenProperty)
        {
            var functionName = excelGivenProperty.Name;

            return 
$@"        // No sensible implementation can be generated for functions, so please 
        // add the function below in a custom class.
        // public void {functionName}() {{ .. }} ";
        }

        IEnumerable<string> SimplePropertyDeclarations(GivenClass excelGivenClass)
        {
            return excelGivenClass
                .Properties
                .Where(excelProperty => excelProperty.Type.IsSimpleProperty())
                .Select(SimplePropertyDeclaration);
        }

        string SimplePropertyDeclaration(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = excelGivenProperty.Name;
            var parameterType = CsharpPropertyTypeName(excelGivenProperty.Type, excelGivenProperty.ExampleValue);

            return $"        public {parameterType} {parameterName} {{ get; private set; }}";
        }

        IEnumerable<string> SimpleProperties(GivenClass excelGivenClass)
        {
            return excelGivenClass
                .Properties
                .Where(excelProperty => excelProperty.Type.IsSimpleProperty())
                .Select(SimplePropertySetter);
        }

        string CsharpPropertyTypeName(ExcelPropertyType type, string propertyValue)
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
                    return propertyValue?.Substring(0, Math.Max(propertyValue.IndexOf('.'), 1)) ?? "Enum /* no value in excel tests for value of this enum, so unable to deduce the type */";
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

        string SimplePropertySetter(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);
            var parameterType = CsharpPropertyTypeName(excelGivenProperty.Type, excelGivenProperty.ExampleValue);
            var classPropertyName = excelGivenProperty.Name;

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({parameterType} {parameterName})
        {{
            AddValueProperty(GetCurrentMethod(), {parameterName});

            this.{classPropertyName} = {parameterName};

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
            var classVariableName = excelGivenProperty.Name;
            var variableType = $"SpecificationSpecific{excelGivenProperty.ClassName}";

            return $"        public {variableType} {classVariableName} {{ get; private set; }}";
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
            var classVariableName = excelGivenProperty.Name;
            var parameterName = CamelCase(excelGivenProperty.Name);
            var propertyClassName = $"SpecificationSpecific{excelGivenProperty.ClassName}";

            return
$@"        internal {SpecificationSpecificClassName} {functionName}({propertyClassName} {parameterName})
        {{
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

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
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            this.{listPropertyName}.Add({parameterName});

            return this;
        }}

        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_list_of(List<{listClassName}> {listParameterName}, string listType)
        {{
            AddListProperty(new ReportSpecificationSetupList(GetCurrentMethod().Name, listType, {listParameterName}));

            this.{listPropertyName}.AddRange({listParameterName});

            return this;
        }}

        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_table_of(ReportSpecificationSetupClassUsingTable<{listClassName}> {listParameterName})
        {{
            {listParameterName}.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty({listParameterName});

            foreach (var row in {listParameterName}.Rows)
                this.{listPropertyName}.Add(row.Properties);

            return this;
        }}";
        }

        IEnumerable<IGivenClassProperty> ListProperties(GivenClass excelGivenClass) =>
            excelGivenClass
            .Properties
            .Where(excelProperty => excelProperty.Type == ExcelPropertyType.List);

        string SpecificationSpecificClassName =>
            $"SpecificationSpecific{excelGivenClass.Name}";

        string CamelCase(string pascalCase) =>
            string.IsNullOrWhiteSpace(pascalCase) ? "" : char.ToLower(pascalCase[0]) + pascalCase.Substring(1);

        string ListPropertyName(IGivenClassProperty excelProperty) =>
            CamelCase(excelProperty.Name) + "s";
    }
}