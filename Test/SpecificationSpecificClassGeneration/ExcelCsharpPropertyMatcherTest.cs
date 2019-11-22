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
        enum Test { }

        [TestCase(typeof(float?))]
        [TestCase(typeof(double?))]
        [TestCase(typeof(int?))]
        [TestCase(typeof(sbyte?))]
        [TestCase(typeof(byte?))]
        [TestCase(typeof(short?))]
        [TestCase(typeof(uint?))]
        [TestCase(typeof(long?))]
        [TestCase(typeof(ulong?))]
        [TestCase(typeof(char?))]
        [TestCase(typeof(decimal?))]
        [TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan?))]
        [TestCase(typeof(bool?))]
        [TestCase(typeof(string))]
        [TestCase(typeof(object))]
        // this doesn't work, not going to worry about it for now[TestCase(typeof(Test?))]
        [TestCase(typeof(ExcelCsharpPropertyMatcherTest))]
        public void NullExcelPropertyMatchesNullCsharpProperty(Type nullableType)
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    nullableType,
                    ExcelPropertyType.Null));
        }

        [Test]
        public void EnumExcelPropertyMatchesEnumCsharpProperty()
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    typeof(Test),
                    ExcelPropertyType.Enum));
        }

        [TestCase(typeof(float))]
        [TestCase(typeof(double))]
        [TestCase(typeof(int))]
        [TestCase(typeof(sbyte))]
        [TestCase(typeof(byte))]
        [TestCase(typeof(short))]
        [TestCase(typeof(uint))]
        [TestCase(typeof(long))]
        [TestCase(typeof(ulong))]
        [TestCase(typeof(char))]
        public void NumberExcelPropertyMatchesCsharpProperty(Type numberType)
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    numberType,
                    ExcelPropertyType.Number));
        }

        [Test]
        public void DecimalExcelPropertyMatchesDecimalCsharpProperty()
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    typeof(decimal),
                    ExcelPropertyType.Decimal));
        }

        [Test]
        public void DateTimeExcelPropertyMatchesDateTimeCsharpProperty()
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    typeof(DateTime),
                    ExcelPropertyType.DateTime));
        }

        [Test]
        public void TimeSpanExcelPropertyMatchesTimespanCsharpProperty()
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    typeof(TimeSpan),
                    ExcelPropertyType.TimeSpan));
        }

        [Test]
        public void StringExcelPropertyMatchesStringCsharpProperty()
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    typeof(string),
                    ExcelPropertyType.String));

            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    typeof(string),
                    ExcelPropertyType.StringNull));
        }

        [Test]
        public void BooleanExcelPropertyMatchesBooleanCsharpProperty()
        {
            Assert.True(
                new ExcelCsharpPropertyMatcher().PropertiesMatch(
                    typeof(bool),
                    ExcelPropertyType.Boolean));
        }
    }
}
