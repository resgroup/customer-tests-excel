using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGeneratorBase
    {
        protected readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        protected GivenClass excelGivenClass;

        public SpecificationSpecificClassGeneratorBase(
            ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher,
            GivenClass excelGivenClass)
        {
            this.excelCsharpPropertyMatcher = excelCsharpPropertyMatcher ?? throw new ArgumentNullException(nameof(excelCsharpPropertyMatcher));
            this.excelGivenClass = excelGivenClass ?? throw new ArgumentNullException(nameof(excelGivenClass));
        }

        protected static string UsingStatements(IEnumerable<string> usings)
        {
            var allUsings =
                new List<string>
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

        protected IEnumerable<MatchedProperty> MatchWithCsharpType(
            IEnumerable<IGivenClassProperty> excelProperties,
            Type type)
        {
            return 
                excelProperties
                .Select(excelProperty => MatchWithCsharpType(excelProperty, type))
                .Where(mp => mp.CsharpProperty != null);
        }

        protected IEnumerable<IGivenClassProperty> NonMatchingProperties(
            IEnumerable<IGivenClassProperty> excelProperties,
            Type type)
        {
            return 
                excelProperties
                .Select(excelProperty => MatchWithCsharpType(excelProperty, type))
                .Where(mp => mp.CsharpProperty == null)
                .Select(mp => mp.ExcelProperty);
        }

        MatchedProperty MatchWithCsharpType(
            IGivenClassProperty excelProperty,
            Type interfaceType)
        {
            // this gets things in ancestor interfaces as well as directly on the interface
            var properties =
                (new Type[] { interfaceType })
                .Concat(interfaceType.GetInterfaces())
                .SelectMany(i => i.GetProperties());

            return new MatchedProperty()
            {
                ExcelProperty = excelProperty,
                CsharpProperty =
                    properties
                    .FirstOrDefault(c => excelCsharpPropertyMatcher.PropertiesMatch(c, excelProperty))
            };
        }

        protected string SimplePropertyDeclarationOnSelf(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = excelGivenProperty.Name;
            // TODO fix up this cast to GivenClassSimpleProperty once using separate lists for each property type
            var parameterType = CsharpPropertyTypeName(excelGivenProperty.Type, (excelGivenProperty as GivenClassSimpleProperty).ExampleValue, (excelGivenProperty as GivenClassSimpleProperty).Nullable);

            return $"        public {parameterType} {parameterName} {{ get; private set; }}";
        }

        protected string SimplePropertySetterOnSelf(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);
            // TODO fix up this cast to GivenClassSimpleProperty once using separate lists for each property type
            var parameterType = CsharpPropertyTypeName(excelGivenProperty.Type, (excelGivenProperty as GivenClassSimpleProperty).ExampleValue, (excelGivenProperty as GivenClassSimpleProperty).Nullable);
            var classPropertyName = excelGivenProperty.Name;

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({parameterType} {parameterName})
        {{
            AddValueProperty(GetCurrentMethod(), {parameterName});

            this.{classPropertyName} = {parameterName};

            return this;
        }}{NewLine}";
        }

        protected string ComplexPropertyDeclarationOnSelf(IGivenClassProperty excelGivenProperty)
        {
            var classVariableName = excelGivenProperty.Name;
            // TODO fix up this cast to GivenClassSimpleProperty once using separate lists for each property type
            var variableType = $"SpecificationSpecific{(excelGivenProperty as GivenClassComplexProperty).ClassName}";

            return $"        public {variableType} {classVariableName} {{ get; private set; }}";
        }

        protected string ComplexPropertySetterOnSelf(IGivenClassProperty excelGivenProperty)
        {
            var functionName = $"{excelGivenProperty.Name}_of";
            var classVariableName = excelGivenProperty.Name;
            var parameterName = CamelCase(excelGivenProperty.Name);
            // TODO fix up this cast to GivenClassSimpleProperty once using separate lists for each property type
            var propertyClassName = $"SpecificationSpecific{(excelGivenProperty as GivenClassComplexProperty).ClassName}";

            return
$@"        internal {SpecificationSpecificClassName} {functionName}({propertyClassName} {parameterName})
        {{
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            this.{classVariableName} = {parameterName};

            return this;
        }}{NewLine}";
        }

        protected string ListPropertyDeclarationOnSelf(IGivenClassProperty excelGivenProperty)
        {
            // TODO fix up this cast to GivenClassSimpleProperty once using separate lists for each property type
            var listClassName = $"SpecificationSpecific{(excelGivenProperty as GivenClassComplexListProperty).ClassName}";
            var listPropertyName = ListPropertyName(excelGivenProperty);

            return $"        readonly List<{listClassName}> {listPropertyName};";
        }

        protected string ListPropertyInitialisationOnSelf(IGivenClassProperty excelGivenProperty)
        {
            // TODO fix up this cast to GivenClassSimpleProperty once using separate lists for each property type
            var listClassName = $"SpecificationSpecific{(excelGivenProperty as GivenClassComplexListProperty).ClassName}";
            var listPropertyName = ListPropertyName(excelGivenProperty);

            return $"            {listPropertyName} = new List<{listClassName}>();";
        }

        protected string ListPropertySetterOnSelf(IGivenClassProperty excelGivenProperty)
        {
            var parameterName = CamelCase(excelGivenProperty.Name);
            var listParameterName = ListPropertyName(excelGivenProperty);
            var listPropertyName = ListPropertyName(excelGivenProperty);
            // TODO fix up this cast to GivenClassSimpleProperty once using separate lists for each property type
            var listClassName = $"SpecificationSpecific{(excelGivenProperty as GivenClassComplexListProperty).ClassName}";

            return
$@"        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_of({listClassName} {parameterName})
        {{
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            this.{listPropertyName}.Add({parameterName});

            return this;
        }}

        internal {SpecificationSpecificClassName} {excelGivenProperty.Name}_list_of(string listType, List<{listClassName}> {listParameterName})
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

        string CsharpPropertyTypeName(ExcelPropertyType type, string propertyValue, bool nullable)
        {
            var questionMark = nullable ? "?" : "";

            switch (type)
            {
                case ExcelPropertyType.Null:
                    return typeof(object).Name;
                case ExcelPropertyType.Number:
                    return "Double" + questionMark;
                case ExcelPropertyType.Decimal:
                    return "Deciml" + questionMark;
                case ExcelPropertyType.String:
                    return typeof(string).Name;
                case ExcelPropertyType.DateTime:
                    return "DateTime" + questionMark;
                case ExcelPropertyType.TimeSpan:
                    return "TimeSpan" + questionMark;
                case ExcelPropertyType.Enum:
                    return propertyValue?.Substring(0, Math.Max(propertyValue.IndexOf('.'), 1)) ?? "Enum /* no value in excel tests for value of this enum, so unable to deduce the type */";
                case ExcelPropertyType.Boolean:
                    return "bool" + questionMark;
                case ExcelPropertyType.Object:
                    return typeof(object).Name;
                case ExcelPropertyType.List:
                    return typeof(IEnumerable).Name;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            };
        }

        protected string CamelCase(string pascalCase) =>
            string.IsNullOrWhiteSpace(pascalCase) ? "" : char.ToLower(pascalCase[0]) + pascalCase.Substring(1);

        protected string SpecificationSpecificClassName =>
           $"SpecificationSpecific{excelGivenClass.Name}";

        protected string ListPropertyName(IGivenClassProperty excelProperty) =>
           CamelCase(excelProperty.Name) + "s";

        protected string RemoveConsecutiveBlankLines(string code) =>
           code
           .Replace(NewLine + NewLine + NewLine, NewLine + NewLine)
           .Replace(NewLine + NewLine + NewLine, NewLine + NewLine)
           .Replace(NewLine + NewLine + NewLine, NewLine + NewLine)
           .Replace(NewLine + NewLine + NewLine, NewLine + NewLine);
    }
}