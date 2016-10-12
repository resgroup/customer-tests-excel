using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RES.Specification
{
    public abstract class BaseAssertion<T> : IAssertion<T>
    {
        protected abstract AssertionOperator Operator { get; }

        protected object Expected { get; private set; }
        readonly Expression<Func<T, object>> _property;

        public BaseAssertion(Expression<Func<T, object>> property, object expected)
        {
            if (property == null) throw new ArgumentNullException("property");
            
            _property = property;
            Expected = expected;
        }

        string PropertyName
        {
            get
            {
                return new ParseAssertionProperty(_property).PropertyName;
            }
        }

        object Actual(T sut)
        {
            var propertyGetter = _property.Compile();
            return propertyGetter(sut);         
        }

        public bool Passed(T sut)
        {
            var actual = Actual(sut);
            return InternalPassed(actual);
            //writer.Assert(PropertyName, Expected, Operator, actual, result);
            //return result;
        }

        public void Write(T sut, bool passed, ITestOutputWriter writer)
        {
            var actual = Actual(sut);
            writer.Assert(PropertyName, Expected, Operator, actual, passed, AssertionSpecifics());
        }

        protected virtual IEnumerable<string> AssertionSpecifics()
        {
            return new List<string>();
        }

        protected abstract bool InternalPassed(object actual);
    }


}
