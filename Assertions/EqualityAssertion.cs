using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CustomerTestsExcel.Assertions
{
    public class EqualityAssertion<T> : BaseAssertion<T>
    {
        public EqualityAssertion(Expression<Func<T, object>> property, object expected)
            : base(property, expected) { }

        protected override AssertionOperator Operator => 
            AssertionOperator.Equality;

        protected override bool InternalPassed(object actual)
        {
            if (actual == null)
                return Expected == null || Expected.ToString().Equals("null", StringComparison.InvariantCultureIgnoreCase);

            try
            {
                // this will compare double and int as well as possible (eg int 3 will equal double 3, unless there are rounding error type issues)
                // if there are rounding type issues, then use "Percentage Precision" in the Excel spreadsheet to indicate how much difference is allowable
                return (dynamic)actual == (dynamic)Expected;
            }
            catch
            {
                // it is possible that the dynamic comparison fails (it can't compare objects to each other for example), in which case we just return false.
                // the comparisons should all be on primitives, but we have no control over what the system under test returns.
                return false;
            }

        }
    }
}
