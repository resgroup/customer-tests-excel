using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RES.Specification
{
    public interface ITableHeader
    {
        string PropertyName { get; }
        bool Equals(ITableHeader header);
    }

    public class TableHeader
    {
        protected readonly string _propertyName;
        public string PropertyName { get { return _propertyName; } }

        public TableHeader(string propertyName)
        {
            _propertyName = propertyName;
        }
    }

    public class PropertyTableHeader : TableHeader, ITableHeader
    {
        public PropertyTableHeader(string propertyName) : base(propertyName) { }

        public bool Equals(ITableHeader other)
        {
            return (GetType().Equals(other.GetType()) && (other as PropertyTableHeader).PropertyName == PropertyName);
        }
    }

    public class SubClassTableHeader : TableHeader, ITableHeader
    {
        protected readonly string _className;
        public string ClassName { get { return _className; } }

        protected readonly IEnumerable<ITableHeader> _creationalHeaders;
        public IEnumerable<ITableHeader> CreationalHeaders { get { return _creationalHeaders; } }

        protected readonly IEnumerable<ITableHeader> _headers;
        public IEnumerable<ITableHeader> Headers { get { return _headers; } }

        public SubClassTableHeader(string propertyName, string className, IEnumerable<ITableHeader> creationalHeaders, IEnumerable<ITableHeader> headers)
            : base(propertyName)
        {
            _className = className;
            _creationalHeaders = creationalHeaders;
            _headers = headers;
        }

        public bool Equals(ITableHeader other)
        {
            if (GetType().Equals(other.GetType()) == false) return false;

            var subClassOther = other as SubClassTableHeader;

            if (subClassOther.PropertyName != PropertyName || subClassOther.ClassName != ClassName) return false;

            if (EqualHeaders(Headers, subClassOther.Headers) == false) return false;

            if (EqualHeaders(CreationalHeaders, subClassOther.CreationalHeaders) == false) return false;

            return true;
        }

        private bool EqualHeaders(IEnumerable<ITableHeader> ours, IEnumerable<ITableHeader> theirs)
        {
            if (ours.Count() != theirs.Count()) return false;

            var oursEnumerator = ours.GetEnumerator();
            var theirsEnumerator = theirs.GetEnumerator();

            while (oursEnumerator.MoveNext())
            {
                theirsEnumerator.MoveNext();

                if (oursEnumerator.Current.Equals(theirsEnumerator.Current) == false) return false;
            }

            return true;
        }

    }

    public class RunSpecification<T>
        where T : IReportsSpecificationSetup
    {
        public string Message { get { return _message.StringBuilder.ToString(); } }

        protected readonly ITestOutputWriter _writer;
        protected readonly StringBuilderTextLineWriter _message;

        public RunSpecification(ITestOutputWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            _message = new StringBuilderTextLineWriter();
            _writer = new CombinedTestOutputWriter(new List<ITestOutputWriter>() { writer, new StringTestOutputWriter(new HumanFriendlyFormatter(), _message) });
        }

        public bool Run(ISpecification<T> specification)
        {
            bool passed;
            try
            {
                passed = RunOne(specification);
            }
            catch (Exception ex)
            {
                passed = false;
                _writer.Exception(ex.Message);
            }

            return passed;
        }

        protected bool RunOne(ISpecification<T> specification)
        {
            T sut;
            var specificationNamespace = specification.GetType().Namespace;

            // arrange
            _writer.StartSpecification(specificationNamespace, specification.GetType().Name, specification.Description());

            _writer.StartGiven();
            sut = specification.Given();
            WriteRootClass(sut);
            _writer.EndGiven();

            // act (it is also possible for this to return an Expression and for us to write out a string representation of this to ensure that the code and the text do not diverge)
            _writer.When(specification.When(sut));

            // assert, write out a string representation of the Expressions's
            _writer.StartAssertions();

            bool allPassed = true;
            foreach (var assertion in specification.Assertions())
            {
                bool passed = assertion.Passed(sut);
                allPassed = allPassed && passed;
                assertion.Write(sut, passed, _writer);
            }

            _writer.EndAssertions();

            _writer.EndSpecification(specificationNamespace, allPassed);

            return allPassed;
        }

        private void WriteRootClass(IReportsSpecificationSetup properties)
        {
            _writer.StartClass(ClassName(properties));

            WriteClass(properties);

            _writer.EndClass();
        }

        private void WriteSubClass(IReportsSpecificationSetup properties)
        {
            _writer.StartSubClass(ClassName(properties));

            WriteClass(properties);

            _writer.EndSubClass();
        }

        private void WriteClass(IReportsSpecificationSetup properties)
        {
            WriteClassProperties(properties.CreationalProperties, _writer.StartCreationalProperties, _writer.EndCreationalProperties);

            WriteClassProperties(properties, _writer.StartGivenProperties, _writer.EndGivenProperties);
        }

        private void WriteClassProperties(IReportsSpecificationSetup properties, Action start, Action end)
        {
            // TODO: let properties class decide if it needs doing
            if (properties.ValueProperties.Any() || properties.ClassProperties.Any() || properties.ClassTableProperties.Any())
            {
                using (new TidyUp(start, end))
                {
                    foreach (var property in properties.ValueProperties) _writer.GivenProperty(property);

                    foreach (var classProperty in properties.ClassProperties)
                    {
                        _writer.GivenClassProperty(classProperty.PropertyName, classProperty.IsChild, classProperty.IndexInParent, classProperty.Properties == null);

                        if (classProperty.Properties != null) WriteSubClass(classProperty.Properties);
                    }

                    foreach (var classTableProperty in properties.ClassTableProperties)
                    {
                        if (classTableProperty.Rows.Count() > 0)
                        {
                            _writer.StartClassTable(classTableProperty.PropertyName, ClassName(classTableProperty.Rows.First().Properties));
                            _writer.ClassTablePropertyNamesHeaderRow(classTableProperty.Rows.First().Properties.CreationalProperties.ValueProperties.Select(p => p.PropertyName), classTableProperty.Rows.First().Properties.ValueProperties.Select(p => p.PropertyName));

                            foreach (var row in classTableProperty.Rows) _writer.ClassTablePropertyRow(row.Properties.ValueProperties);

                            _writer.EndClassTable();
                        }
                    }

                }
            }
        }

        private static string ClassName(IReportsSpecificationSetup properties)
        {
            var className = properties.GetType().Name;
            var nameSpace = properties.GetType().Namespace;
            if (nameSpace != typeof(T).Namespace)
                className = nameSpace + "." + className;
            return className;
        }

    }
}
