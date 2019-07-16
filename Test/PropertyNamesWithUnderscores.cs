using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class PropertyNamesWithUnderscores : TestBase
    {
        [Test]
        public void CodeNameToExcelNameConverterHonoursUnderscoresWhenConvertingCsharpGivenPropertyNameToExcel()
        {
            const string csharpPropertyNameWithUnderscores = "Example_Property_Name_of";

            var excelPropertyName = new CodeNameToExcelNameConverter(ANY_STRING).GivenPropertyNameCodeNameToExcelName(csharpPropertyNameWithUnderscores);

            Assert.AreEqual("Example_Property_Name of", excelPropertyName);
        }

        [Test]
        public void CodeNameToExcelNameConverterHonoursUnderscoresWhenRoundTrippingPropertyNames()
        {
            const string excelPropertyNameWithUnderscores = "Example_Property_Name of";

            var converter = new CodeNameToExcelNameConverter(ANY_STRING);

            var csharpPropertyName = converter.GivenPropertyNameExcelNameToCodeName(excelPropertyNameWithUnderscores);

            var convertedExcelPropertyName = converter.GivenPropertyNameCodeNameToExcelName(csharpPropertyName);

            Assert.AreEqual(excelPropertyNameWithUnderscores, convertedExcelPropertyName);
        }

        [Test]
        public void CodeNameToExcelNameConverterHonoursUnderscoresWhenRoundTrippingChildPropertyNames()
        {
            const int childIndex = 1;
            string excelPropertyNameWithUnderscores = $"Example_Property_Name({childIndex}) of";

            var converter = new CodeNameToExcelNameConverter(ANY_STRING);

            var csharpPropertyName = converter.GivenPropertyNameExcelNameToCodeName(excelPropertyNameWithUnderscores);

            var convertedExcelPropertyName = converter.GivenPropertyNameCodeNameToExcelName(csharpPropertyName);

            Assert.AreEqual(excelPropertyNameWithUnderscores, convertedExcelPropertyName);
        }
    }
}
