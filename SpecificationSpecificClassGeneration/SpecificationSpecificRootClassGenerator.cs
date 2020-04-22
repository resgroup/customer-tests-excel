using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificRootClassGenerator : SpecificationSpecificClassGeneratorBase
    {
        public SpecificationSpecificRootClassGenerator(ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher, GivenClass excelGivenClass)
            : base(excelCsharpPropertyMatcher, excelGivenClass)
        {
        }

        public string CsharpCode(
            string testNamespace,
            IEnumerable<string> usings)
        {
            var usingStatements = UsingStatements(usings);

            var functions =
                excelGivenClass
                .Functions
                .Select(Function);

            var simplePropertyDeclarations =
                excelGivenClass
                .SimpleProperties
                .Select(SimplePropertyDeclarationOnSelf);

            var simpleProperties =
                excelGivenClass
                .SimpleProperties
                .Select(SimplePropertySetterOnSelf);

            var complexPropertyDeclarations =
                excelGivenClass
                .ComplexProperties
                .Select(ComplexPropertyDeclarationOnSelf);

            var complexProperties =
                excelGivenClass
                .ComplexProperties
                .Select(ComplexPropertySetterOnSelf);

            var listPropertyDeclarations =
                excelGivenClass
                .ListProperties
                .Select(ListPropertyDeclarationOnSelf);


            var listPropertyInitialisers =
                excelGivenClass
                .ListProperties
                .Select(ListPropertyInitialisationOnSelf);

            var listPropertyFunctions =
                excelGivenClass
                .ListProperties
                .Select(ListPropertySetterOnSelf);

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

        string Function(IGivenClassProperty excelGivenProperty)
        {
            var functionName = excelGivenProperty.Name;

            return 
$@"        // No sensible implementation can be generated for functions, so please 
        // add the function below in a custom class.
        // Custom classes should go under a directory called 'IgnoreOnGeneration'.
        // If the custom class filename is the same as this one ({SpecificationSpecificClassName}),
        // then it will be used instead of this file. If it is called something else,
        // say {SpecificationSpecificClassName}Partial, then this class will remain, and
        // the custom class can add to it.
        // public void {functionName}() {{ .. }} ";
        }

    }
}