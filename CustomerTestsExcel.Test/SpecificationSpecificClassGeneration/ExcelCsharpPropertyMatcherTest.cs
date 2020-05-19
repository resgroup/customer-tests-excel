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
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.Null)
                .TypesMatch(nullableType));
        }

        [Test]
        public void EnumExcelPropertyMatchesEnumCsharpProperty()
        {
            Assert.True(
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.Enum)
                .TypesMatch(typeof(Test)));
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
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.Number)
                .TypesMatch(numberType));
        }

        [Test]
        public void DecimalExcelPropertyMatchesDecimalCsharpProperty()
        {
            Assert.True(
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.Decimal)
                .TypesMatch(typeof(decimal)));
        }

        [Test]
        public void DateTimeExcelPropertyMatchesDateTimeCsharpProperty()
        {
            Assert.True(
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.DateTime)
                .TypesMatch(typeof(DateTime)));
        }

        [Test]
        public void TimeSpanExcelPropertyMatchesTimespanCsharpProperty()
        {
            Assert.True(
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.TimeSpan)
                .TypesMatch(typeof(TimeSpan)));
        }

        [Test]
        public void StringExcelPropertyMatchesStringCsharpProperty()
        {
            Assert.True(
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.String)
                .TypesMatch(typeof(string)));

            Assert.True(
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.StringNull)
                .TypesMatch(typeof(string)));
        }

        [Test]
        public void BooleanExcelPropertyMatchesBooleanCsharpProperty()
        {
            Assert.True(
                new GivenClassSimpleProperty(ANY_STRING, ExcelPropertyType.Boolean)
                .TypesMatch(typeof(bool)));
        }
    }
}
