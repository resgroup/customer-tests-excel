using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CustomerTestsExcel.Indentation;
using CustomerTestsExcel.CodeOutputWriters;

namespace CustomerTestsExcel
{
    public class RunSpecification<T>
        where T : IReportsSpecificationSetup
    {
        public string Message { get { return message.StringBuilder.ToString(); } }

        protected readonly ITestOutputWriter writer;
        protected readonly StringBuilderTextLineWriter message;

        public RunSpecification()
        {
            message = new StringBuilderTextLineWriter();
            this.writer = new StringTestOutputWriter(new HumanFriendlyFormatter(), message);
        }

        public RunSpecification(ITestOutputWriter writer) : this()
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            this.writer = new CombinedTestOutputWriter(new List<ITestOutputWriter>() { writer, this.writer });
        }

        public bool Run(ISpecification<T> specification)
        {
            bool passed;
            try
            {
                passed = TryRun(specification);
            }
            catch (Exception ex)
            {
                passed = false;
                writer.Exception(ex.ToString());
            }

            return passed;
        }

        protected bool TryRun(ISpecification<T> specification)
        {
            T sut;
            var specificationNamespace = specification.GetType().Namespace;

            // arrange
            writer.StartSpecification(specificationNamespace, specification.GetType().Name, specification.Description());

            writer.StartGiven();
            sut = specification.Given();
            WriteRootClass(sut);
            writer.EndGiven();

            // act (it is also possible for this to return an Expression and for us to write out a string representation of this to ensure that the code and the text do not diverge)
            writer.When(specification.When(sut));

            // assert, write out a string representation of the Expressions's
            writer.StartAssertions();

            bool allPassed = true;
            foreach (var assertion in specification.Assertions())
            {
                bool passed = assertion.Passed(sut);
                allPassed = allPassed && passed;
                assertion.Write(sut, passed, writer);
            }

            writer.EndAssertions();

            writer.EndSpecification(specificationNamespace, allPassed);

            return allPassed;
        }

        void WriteRootClass(IReportsSpecificationSetup properties)
        {
            writer.StartClass(ClassName(properties));

            WriteClass(properties);

            writer.EndClass();
        }

        void WriteSubClass(IReportsSpecificationSetup properties)
        {
            writer.StartSubClass(ClassName(properties));

            WriteClass(properties);

            writer.EndSubClass();
        }

        void WriteClass(IReportsSpecificationSetup properties)
        {
            if (!properties.AnythingToSetup())
                return;

            using (new TidyUp(writer.StartGivenProperties, writer.EndGivenProperties))
            {
                WriteProperties(properties);
            }

        }

        private void WriteProperties(IReportsSpecificationSetup properties)
        {
            WriteValueProperties(properties);

            WriteClassProperties(properties);

            WriteClassTableProperties(properties);

            WriteListProperties(properties);
        }

        private void WriteListProperties(IReportsSpecificationSetup properties)
        {
            foreach (var property in properties.ListProperties)
            {

                writer.StartGivenListProperty(property);

                foreach (var listItem in property.Items)
                {
                    writer.StartGivenListPropertyItem(listItem);
                    WriteProperties(listItem);
                    writer.EndGivenListPropertyItem(listItem);
                }

                writer.EndGivenListProperty(property);
            }
        }

        private void WriteValueProperties(IReportsSpecificationSetup properties)
        {
            foreach (var property in properties.ValueProperties) writer.GivenProperty(property);
        }

        private void WriteClassProperties(IReportsSpecificationSetup properties)
        {
            foreach (var classProperty in properties.ClassProperties)
            {
                writer.GivenClassProperty(classProperty.PropertyName, classProperty.Properties == null);

                if (classProperty.Properties != null) WriteSubClass(classProperty.Properties);
            }
        }

        private void WriteClassTableProperties(IReportsSpecificationSetup properties)
        {
            foreach (var classTableProperty in properties.ClassTableProperties)
            {
                if (classTableProperty.Rows.Any())
                {
                    writer.StartClassTable(classTableProperty.PropertyName, ClassName(classTableProperty.Rows.First().Properties));
                    writer.ClassTablePropertyNamesHeaderRow(classTableProperty.Rows.First().Properties.ValueProperties.Select(p => p.PropertyName));

                    foreach (var row in classTableProperty.Rows) writer.ClassTablePropertyRow(row.Properties.ValueProperties);

                    writer.EndClassTable();
                }
            }
        }

        static string ClassName(IReportsSpecificationSetup properties)
        {
            // Add a namespace for properties that reference classes in other assemblies.
            var className = properties.GetType().Name;
            var nameSpace = properties.GetType().Namespace;

            if (typeof(T).Assembly.FullName != properties.GetType().Assembly.FullName)
                return nameSpace + "." + className;

            return className;
        }

    }
}
