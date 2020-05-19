using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using CustomerTestsExcel.CodeOutputWriters;

namespace CustomerTestsExcel.Assertions
{
    public abstract class BaseAssertion<T> : IAssertion<T>
    {
        protected abstract AssertionOperator Operator { get; }
        protected object Expected { get; }
        public Expression<Func<T, object>> Property { get; }

        public BaseAssertion(Expression<Func<T, object>> property, object expected)
        {
            if (property == null) throw new ArgumentNullException("property");

            Property = property;
            Expected = expected;
        }

        public bool Passed(T sut)
        {
            var actual = Actual(sut);
            return InternalPassed(actual);
        }

        public void Write(T sut, bool passed, ITestOutputWriter writer)
        {
            var actual = Actual(sut);
            writer.Assert(PropertyName, Expected, Operator, actual, passed, AssertionSpecifics());
        }

        protected virtual IEnumerable<string> AssertionSpecifics() =>
            new List<string>();

        protected abstract bool InternalPassed(object actual);

        string PropertyName => 
            new ParseAssertionProperty(Property).PropertyName;

        object Actual(T sut)
        {
            var propertyGetter = Property.Compile();
            return propertyGetter(sut);
        }

    }


}
