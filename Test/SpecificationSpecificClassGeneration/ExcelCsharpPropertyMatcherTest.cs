using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class ExcelCsharpPropertyMatcherTest : TestBase
    {
        //AssertContains(rootClass.Properties, "Null of", ExcelPropertyType.Null);
        //AssertContains(rootClass.Properties, "Enum of", ExcelPropertyType.Enum);
        //AssertContains(rootClass.Properties, "Number of", ExcelPropertyType.Number);
        //AssertContains(rootClass.Properties, "Decimal of", ExcelPropertyType.Decimal);

        [Test]
        public void DecimalExcelPropertyMatchesDecimalCsharpProperty()
        {
            Decimal decimalCsharpProperty = 1m;

            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    decimalCsharpProperty.GetType(),
                    ExcelPropertyType.Decimal));
        }

        [Test]
        public void DateTimeExcelPropertyMatchesDateTimeCsharpProperty()
        {
            DateTime dateTimeCsharpProperty = new DateTime(2000, 1, 1);

            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    dateTimeCsharpProperty.GetType(),
                    ExcelPropertyType.DateTime));
        }

        [Test]
        public void TimeSpanExcelPropertyMatchesTimespanCsharpProperty()
        {
            TimeSpan timeSpanCsharpProperty = TimeSpan.FromSeconds(1);

            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    timeSpanCsharpProperty.GetType(),
                    ExcelPropertyType.TimeSpan));
        }

        [Test]
        public void StringExcelPropertyMatchesStringCsharpProperty()
        {
            string stringCsharpProperty = "";

            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    stringCsharpProperty.GetType(),
                    ExcelPropertyType.String));

            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    stringCsharpProperty.GetType(),
                    ExcelPropertyType.StringNull));
        }

        [Test]
        public void BooleanExcelPropertyMatchesBooleanCsharpProperty()
        {
            bool boolCsharpProperty = true;

            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    boolCsharpProperty.GetType(),
                    ExcelPropertyType.Boolean));
        }
    }
}
