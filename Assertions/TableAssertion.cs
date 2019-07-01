using CustomerTestsExcel.CodeOutputWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerTestsExcel.Assertions
{
    public class TableAssertion<Parent, Child> : IAssertion<Parent> where Child : class
    {
        protected Expression<Func<Parent, IEnumerable<Child>>> Property { get; }
        string PropertyName => new ParseAssertionProperty(Property).PropertyName;
        IEnumerable<IEnumerable<IAssertion<Child>>> TableRows { get; set; }

        public TableAssertion(Expression<Func<Parent, IEnumerable<Child>>> property, IEnumerable<IEnumerable<IAssertion<Child>>> tableRows)
        {
            Property = property;
            TableRows = tableRows;
        }

        public bool Passed(Parent sut)
        {
            var child = GetChildren(sut);

            if (child == null)
                return false;

            bool passed = true;
            var childEnumerator = child.GetEnumerator();

            foreach (var tableRow in TableRows)
            {
                childEnumerator.MoveNext();

                if (childEnumerator.Current == null)
                    return false;

                foreach (var tableCell in tableRow)
                {
                    passed = passed && tableCell.Passed(childEnumerator.Current);
                }
            }

            return passed;
        }

        public void Write(Parent sut, bool passed, ITestOutputWriter writer)
        {
            var child = GetChildren(sut);

            writer.StartAssertionSubProperties(PropertyName, child != null, typeof(Child).Name, Passed(sut));

            var childEnumerator = child.GetEnumerator();
            foreach (var tableRow in TableRows)
            {
                childEnumerator.MoveNext();

                if (childEnumerator.Current == null)
                    break;

                foreach (var tableCell in tableRow)
                {
                    tableCell.Write(childEnumerator.Current, tableCell.Passed(childEnumerator.Current), writer);
                }
            }

            writer.EndAssertionSubProperties();
        }

        IEnumerable<Child> GetChildren(Parent sut)
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
    }
}