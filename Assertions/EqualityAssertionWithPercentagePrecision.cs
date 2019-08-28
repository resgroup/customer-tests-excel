using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CustomerTestsExcel.Assertions
{
    public class EqualityAssertionWithPercentagePrecision<T> : BaseAssertion<T>
    {
        double _percentagePrecision;

        public EqualityAssertionWithPercentagePrecision(Expression<Func<T, object>> property, double? expected, double percentagePrecision) : base(property, expected)
        {
            _percentagePrecision = percentagePrecision;
        }

        protected override AssertionOperator Operator => 
            AssertionOperator.Equality;

        protected override bool InternalPassed(object actual)
        {
            if (actual == null) return Expected == null || Expected.ToString().Equals("null", StringComparison.InvariantCultureIgnoreCase);

            double actualDouble;
            try
            {
                actualDouble = Convert.ToDouble(actual);
            }
            catch (Exception e)
            {
                throw new Exception("EqualityAssertionWithPercentagePrecision must be used on a property that can be converted to a double, actual value passed was " + actual.ToString(), e);
            }

            double difference = Math.Abs(actualDouble - (double)Expected);

            return difference <= Math.Abs((double)Expected * _percentagePrecision);
        }

        protected override IEnumerable<string> AssertionSpecifics()
            => new[] { "PercentagePrecision", _percentagePrecision.ToString() };

    }
}
