using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using CustomerTestsExcel.CodeOutputWriters;

namespace CustomerTestsExcel.Assertions
{
    public class ParentAssertion<Parent, Child> : IAssertion<Parent> where Child : class
    {
        protected Expression<Func<Parent, Child>> Property { get; }

        public ParentAssertion(Expression<Func<Parent, Child>> property, IEnumerable<IAssertion<Child>> childAssertions)
        {
            Property = property;
            ChildAssertions = childAssertions;
        }

        public bool Passed(Parent sut)
        {
            Child child = GetChild(sut);

            bool result = false;
            if (child != null)
            {
                result = ChildAssertions.Min(c => c.Passed(child));
            }

            return result;
        }

        public void Write(Parent sut, bool passed, ITestOutputWriter writer)
        {
            Child child = GetChild(sut);

            writer.StartAssertionSubProperties(PropertyName, child != null, typeof(Child).Name, passed);

            if (child != null)
            {
                foreach (var assertion in ChildAssertions)
                {
                    bool childPassed = assertion.Passed(child);
                    assertion.Write(child, childPassed, writer);
                }
            }

            writer.EndAssertionSubProperties();
        }

        private Child GetChild(Parent sut)
        {
            var expression = Property.Compile();
            try
            {
                return expression(sut);
            }
            catch
            {
                return null;
            }
        }

        private string PropertyName => new ParseAssertionProperty(Property).PropertyName;

        private IEnumerable<IAssertion<Child>> ChildAssertions { get; set; }
    }
}
