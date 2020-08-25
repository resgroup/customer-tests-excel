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
                .ComplexListProperties
                .Select(ListPropertyDeclarationOnSelf);


            var listPropertyInitialisers =
                excelGivenClass
                .ComplexListProperties
                .Select(ListPropertyInitialisationOnSelf);

            var listPropertyFunctions =
                excelGivenClass
                .ComplexListProperties
                .Select(ListPropertySetterOnSelf);

            var code =
$@"{usingStatements}

namespace {testNamespace}.Setup
{{
    // This is a generated class that matches the root class of an excel test.

    // It should create all the things you need for the 'Given' section of the
    // test, but you will need to add a method for the 'When' section, and a 
    // property for the 'Assert' section.
    // You can see an example at the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/Setup/Vermeulen%20Near%20Wake%20Length/SpecificationSpecificVermeulenNearWakeLengthCalculatorPartial.cs
    // The 'Calculate()' method matches up with 'When', 'Calculate' from the 
    // test, and the 'VermeulenNearWakeLengths' property matches up with the 
    // 'Assert', 'VermeulenNearWakeLengths' from the test.
    // You can see the associated Excel test on the link below
    // https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/ExcelTests/Vermeulen%20Near%20Wake%20Length.xlsx

    // Custom classes should go in the 'Setup' folder.
    // If the custom class filename is '{SpecificationSpecificClassName}Override.cs',
    // then it will be used instead of this function. If it is called something else,
    // say '{SpecificationSpecificClassName}Partial.cs', then this class will remain, and
    // the custom class can add to it.

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
            return RemoveConsecutiveBlankLines(code);
        }

        string Function(IGivenClassProperty excelGivenProperty)
        {
            var functionName = excelGivenProperty.Name;

            return
$@"        // No sensible implementation can be generated for functions, so please 
        // add the function below in a custom class.
        // Custom classes should go in the 'Setup' folder.
        // If the custom class filename is '{SpecificationSpecificClassName}Override.cs',
        // then it will be used instead of this file. If it is called something else,
        // say {SpecificationSpecificClassName}Partial.cs, then this class will remain, and
        // the custom class can add to it.
        // public void {functionName}() {{ .. }} 
";
        }

    }
}