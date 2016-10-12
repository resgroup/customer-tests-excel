using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RES.Specification
{
    public class EqualityAssertionWithStringFormat<T> : BaseAssertion<T>
    {
        private string _format;

        public EqualityAssertionWithStringFormat(Expression<Func<T, object>> property, object expected, string format)
            : base(property, expected) 
        {
            _format = format;
        }

        protected override AssertionOperator Operator
        {
            get { return AssertionOperator.Equality; }
        }

        protected override bool InternalPassed(object actual)
        {
            if (actual == null) return Expected == null || Expected.ToString().Equals("null", StringComparison.InvariantCultureIgnoreCase);

            return (string.Format("{0:" + _format + "}", actual) == string.Format("{0:" + _format + "}", Expected));
        }

        protected override IEnumerable<string> AssertionSpecifics()
        {
            return new List<string>() { "StringFormat", _format };
        }

    }
}
