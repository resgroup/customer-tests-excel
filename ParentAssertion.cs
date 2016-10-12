using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RES.Specification
{
    public class ParentAssertion<Parent, Child> : IAssertion<Parent> where Child : class
    {
        protected readonly Expression<Func<Parent, Child>> _property;

        public ParentAssertion(Expression<Func<Parent, Child>> property, IEnumerable<IAssertion<Child>> childAssertions)
        {
            _property = property; 
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
                    assertion.Write(child, assertion.Passed(child), writer);
                }
            }

            writer.EndAssertionSubProperties();
        }

        private Child GetChild(Parent sut)
        {
            var expression = _property.Compile();
            try
            {
                return expression(sut);
            }
            catch
            {
                return null;
            }
        }

        private object Index
        {
            get { return new ParseAssertionProperty(_property).Index; }
        }

        private string PropertyName
        {
            get { return new ParseAssertionProperty(_property).PropertyName; }
        }

        private IEnumerable<IAssertion<Child>> ChildAssertions { get; set; }
    }
}
