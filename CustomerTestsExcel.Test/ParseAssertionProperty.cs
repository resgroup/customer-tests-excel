using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class ParseAssertionProperty
    {

        interface AssertionInterface
        {
            int SimpleProperty { get; set; }
            int ParameterlessMethod();
            int OneIntParameterMethod(int index);
        }

        [Test]
        public void SimpleProperty()
        {
            Expression<Func<AssertionInterface, int>> expression = assertionClass => assertionClass.SimpleProperty;

            var sut = new Assertions.ParseAssertionProperty(expression);

            Assert.AreEqual("SimpleProperty", sut.PropertyName);
        }

        [Test]
        public void ParameterlessMethod()
        {
            Expression<Func<AssertionInterface, int>> expression = assertionClass => assertionClass.ParameterlessMethod();

            var sut = new Assertions.ParseAssertionProperty(expression);

            Assert.AreEqual("ParameterlessMethod()", sut.PropertyName);
        }

        [Test]
        public void OneIntParameterMethod()
        {
            Expression<Func<AssertionInterface, int>> expression = assertionClass => assertionClass.OneIntParameterMethod(0);

            var sut = new Assertions.ParseAssertionProperty(expression);

            Assert.AreEqual("OneIntParameterMethod(0)", sut.PropertyName);
        }

    }
}
