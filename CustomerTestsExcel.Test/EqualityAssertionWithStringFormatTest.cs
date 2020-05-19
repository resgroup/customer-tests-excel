using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTestsExcel.Assertions;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    class EqualityAssertionWithStringFormatTest
    {
        private void Check(bool shouldPass, object expected, object actual, string format, string message)
        {
            // we just return actual regardless of the generic type of the assertion, so just use an int and then basically ignore it
            var sut = new EqualityAssertionWithStringFormat<int>((i) => actual, expected, format);

            Assert.AreEqual(shouldPass, sut.Passed(0), message);
        }

        [Test]
        public void Numbers()
        {
            Check(shouldPass: true, expected: 1.00515, actual: 1.0098763, format: "N2", message: "string comparision of numbers the same to two decimal places should be equal with format N2");
            Check(shouldPass: false, expected: 1.00515, actual: 1.0098763, format: "N3", message: "string comparision of numbers the same to two decimal places should be different with format N3");
        }

        [Test]
        public void Dates()
        {
            Check(shouldPass: true, expected: new DateTime(2014, 1, 1), actual: new DateTime(2014, 2, 2), format: "yyyy", message: "string comparision of dates with the same year should be equal with format yyyy");
            Check(shouldPass: false, expected: new DateTime(2014, 1, 1), actual: new DateTime(2014, 2, 2), format: "yyyy MM", message: "string comparision of dates with the same year but different months should be equal with format yyyy MM");
        }

    }
}
