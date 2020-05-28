using System;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGenerator : SpecificationSpecificClassGeneratorBase
    {
        Type type;

        public SpecificationSpecificClassGenerator(ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher, GivenClass excelGivenClass) 
            : base(excelCsharpPropertyMatcher, excelGivenClass)
        {
        }

        public string CsharpCode(
            string testNamespace,
            IEnumerable<string> usings,
            Type type)
        {
            this.type = type;

            var usingStatements = UsingStatements(usings);

            var nonMatchingProperties =
                NonMatchingProperties(excelGivenClass.ListProperties, type)
                .Select(NonMatchingProperty);

            var simpleProperties = 
                MatchWithCsharpType(excelGivenClass.SimpleProperties, type)
                .Select(SimplePropertySetterOnMock);

            var complexProperties =
                MatchWithCsharpType(excelGivenClass.ComplexProperties, type)
                .Select(ComplexPropertySetterOnMock);

            var listPropertyDeclarations =
                MatchWithCsharpType(excelGivenClass.ListProperties, type)
                .Select(ListPropertyDeclarationOfMock);

            var listPropertyMockSetups = 
                MatchWithCsharpType(excelGivenClass.ListProperties, type)
                .Select(ListPropertyMockSetup);

            var listPropertyFunctions =
                MatchWithCsharpType(excelGivenClass.ListProperties, type)
                .Select(ListPropertySetterOnMock);

            var code =
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

{string.Join(NewLine, simpleProperties)}

{string.Join(NewLine, complexProperties)}

{string.Join(NewLine, listPropertyFunctions)}
    }}
}}
";
            return RemoveConsecutiveBlankLines(code);
        }

        string SimplePropertySetterOnMock(MatchedProperty matchedProperty)
        {
            var functionName = $"{matchedProperty.ExcelProperty.Name}_of";
            var parameterName = CamelCase(matchedProperty.ExcelProperty.Name);
            var interfacePropertyName = matchedProperty.ExcelProperty.Name;
            var parameterType = matchedProperty.CsharpProperty.PropertyType.Name;

            return
$@"        internal {SpecificationSpecificClassName} {functionName}({parameterType} {parameterName})
        {{
            AddValueProperty(GetCurrentMethod(), {parameterName});

            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({parameterName});

            return this;
        }}{NewLine}";
        }

        string ComplexPropertySetterOnMock(MatchedProperty matchedProperty)
        {
            var functionName = $"{matchedProperty.ExcelProperty.Name}_of";
            var parameterName = CamelCase(matchedProperty.ExcelProperty.Name);
            var interfacePropertyName = matchedProperty.ExcelProperty.Name;
            var propertyClassName = $"SpecificationSpecific{matchedProperty.ExcelProperty.ClassName}";
            var propertyNameOfSutObject = matchedProperty.ExcelProperty.Name;

            return
$@"        internal {SpecificationSpecificClassName} {functionName}({propertyClassName} {parameterName})
        {{
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({parameterName}.{propertyNameOfSutObject});

            return this;
        }}{NewLine}";
        }

        string ListPropertyDeclarationOfMock(MatchedProperty matchedProperty)
        {
            var listClassName = $"SpecificationSpecific{ matchedProperty.ExcelProperty.ClassName}";
            var listPropertyName = ListPropertyName(matchedProperty.ExcelProperty);

            return $"        readonly List<{listClassName}> {listPropertyName} = new List<{listClassName}>();";
        }

        string ListPropertyMockSetup(MatchedProperty matchedProperty)
        {
            var interfacePropertyName = matchedProperty.ExcelProperty.Name;
            var listPropertyName = ListPropertyName(matchedProperty.ExcelProperty);
            var interfaceUnderTestPropertyName = matchedProperty.ExcelProperty.ClassName;

            return $"            {MockVariableName}.Setup(m => m.{interfacePropertyName}).Returns({listPropertyName}.Select(l => l.{interfaceUnderTestPropertyName}));";
        }

        string ListPropertySetterOnMock(MatchedProperty matchedProperty)
        {
            var parameterName = CamelCase(matchedProperty.ExcelProperty.Name);
            var listParameterName = ListPropertyName(matchedProperty.ExcelProperty);
            var listPropertyName = ListPropertyName(matchedProperty.ExcelProperty);
            var listClassName = $"SpecificationSpecific{matchedProperty.ExcelProperty.ClassName}";

            return
$@"        internal {SpecificationSpecificClassName} {matchedProperty.ExcelProperty.Name}_of({listClassName} {parameterName})
        {{
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), {parameterName}));

            this.{listPropertyName}.Add({parameterName});

            return this;
        }}

        internal {SpecificationSpecificClassName} {matchedProperty.ExcelProperty.Name}_table_of(ReportSpecificationSetupClassUsingTable<{listClassName}> {listParameterName})
        {{
            {listParameterName}.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty({listParameterName});

            foreach (var row in {listParameterName}.Rows)
                this.{listPropertyName}.Add(row.Properties);

            return this;
        }}";
        }

        string NonMatchingProperty(IGivenClassProperty givenClassProperty)
        {
            return $"// Could not match {givenClassProperty.Name}, please add this in a custom partial class, or override this file entirely, by creating a file in  a IgnoreOnGeneration subfolder called {givenClassProperty.ClassName}.cs";
        }

        string InterfacePropertyName =>
            excelGivenClass.Name;

        string MockInterfaceName =>
            type.Name;

        string MockVariableName =>
            CamelCase(excelGivenClass.Name);
    }
}