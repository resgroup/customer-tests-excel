using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class FunctionNames : TestBase
    {
        [Test]
        public void CodeNameToExcelNameConverterDoesntMutateFunctionNamesWhenConvertingCsharpGivenPropertyNameToExcel()
        {
            const string functionNameThatDoesntEndInOf = "Example_Function";

            var excelPropertyName = new CodeNameToExcelNameConverter(ANY_STRING).GivenPropertyNameCodeNameToExcelName(functionNameThatDoesntEndInOf, null);

            Assert.AreEqual("Example_Function", excelPropertyName);
        }

        [Test]
        public void CodeNameToExcelNameConverterThrowsFunctionsMarkedAsChildrenWhenConvertingCsharpGivenChildPropertyNameToExcel()
        {
            const string functionNameThatDoesntEndInOf = "Example_Function";
            const int childIndex = 1;

            Assert.Throws<CodeToExcelException>(() => new CodeNameToExcelNameConverter(ANY_STRING).GivenPropertyNameCodeNameToExcelName(functionNameThatDoesntEndInOf, childIndex));
        }

        [Test]
        public void CodeNameToExcelNameConverterHonoursFunctionsWhenRoundTrippingPropertyNames()
        {
            const string functionNameThatDoesntEndInOf = "Example_Function";

            var converter = new CodeNameToExcelNameConverter(ANY_STRING);

            var csharpPropertyName = converter.GivenPropertyNameExcelNameToCodeName(functionNameThatDoesntEndInOf);

            var convertedExcelPropertyName = converter.GivenPropertyNameCodeNameToExcelName(csharpPropertyName, null);

            Assert.AreEqual(functionNameThatDoesntEndInOf, convertedExcelPropertyName);
        }

    }
}
