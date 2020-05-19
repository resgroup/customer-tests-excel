using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTestsExcel.Assertions;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    class EqualityAssertionTest
    {
        private void Check(bool shouldPass, object expected, object actual, string message)
        {
            // we just return actual regardless of the generic type of the assertion, so just use an int and then basically ignore it
            var sut = new EqualityAssertion<int>((i) => actual, expected);

            Assert.AreEqual(shouldPass, sut.Passed(0), message);
        }

        [Test]
        public void Precision()
        {
            Check(shouldPass: true, expected: 1, actual: 1, message: "integer 1 should equal integer 1");
            Check(shouldPass: true, expected: "cedd", actual: "cedd", message: "'cedd' should equal 'cedd'");
            Check(shouldPass: false, expected: "Cedd", actual: "cedd", message: "string comparison should be case sensitive");
            Check(shouldPass: false, expected: "1", actual: 1, message: "'1' should not equal integer 1");
        }

    }
}
