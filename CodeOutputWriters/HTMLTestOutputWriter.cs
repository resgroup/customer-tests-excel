using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CustomerTestsExcel.Assertions;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public class HTMLTestOutputWriter : ITestOutputWriter
    {
        protected StringBuilder _writer;
        protected int assemblyDivClassIndex;
        protected int specificationDivClassIndex;
        protected string _specificationName;
        protected readonly IHumanFriendlyFormatter _formatter;

        public HTMLTestOutputWriter(IHumanFriendlyFormatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException("formatter");

            _formatter = formatter;
        }

        private void WriteHeader(string specificationNamespace)
        {
            _writer = new StringBuilder();

            _writer.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            _writer.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
            _writer.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>");
            _writer.AppendLine(@"<head>");
            _writer.Append(@"<link rel='stylesheet' type='text/css' href='");
            for (int i = 0; i < GetDirectoryStructure(specificationNamespace).Count(); i++) _writer.Append(@"..\");
            _writer.Append(@"Specification.css' media='screen'/>");
            _writer.AppendLine();
            _writer.AppendLine(string.Format(@"<title>Test Results for {0}</title>", specificationNamespace));
            _writer.AppendLine("<meta http-equiv='cache-control' content='no-cache' />");
            _writer.AppendLine("<meta http-equiv='pragma' content='no-cache' />");
            _writer.AppendLine(@"</head>");
            _writer.AppendLine(@"<body>");
            _writer.Append(@"<div class='assembly ");
            assemblyDivClassIndex = _writer.Length;
            _writer.AppendLine(@"'>");
            _writer.AppendLine(@"<div class='assemblyName'><h1>Testing Assembly " + specificationNamespace + "</h1></div>");
            _writer.AppendLine(@"<div class='dateStarted'>Tests started at " + DateTime.Now.ToString() + "</div>");

        }

        public void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription)
        {
            WriteHeader(specificationNamespace);

            _specificationName = specificationName;

            _writer.Append(@"<div class='specification ");
            specificationDivClassIndex = _writer.Length;
            _writer.AppendLine(@"'>");

            _writer.AppendLine(@"<div class='specificationName'><h2>Specification: " + specificationDescription + @"</h2></div>");
        }

        public void StartGiven()
        {
            _writer.AppendLine(@"<div class='given'>");
        }

        public void StartClass(string className)
        {
            _writer.AppendLine(@"<div class='givenClassName'><h3>Given a " + _formatter.FormatSpecificationSpecificClassName(className) + "</h3></div>");
        }

        public void StartSubClass(string className)
        {
            _writer.AppendLine(@"<div class='givenSubClass'>");
            _writer.AppendLine(@"<div class='givenSubClassName'>" + _formatter.FormatSpecificationSpecificClassName(className) + "</div>");
        }

        public void EndSubClass()
        {
            _writer.AppendLine(@"</div>");
            _writer.AppendLine(@"</div>");
        }

        public void StartGivenProperties()
        {
            _writer.AppendLine(@"<div class='withProperties'>");
            _writer.AppendLine(@"<div>With Properties</div>");
            _writer.AppendLine(@"<div class='givenPropertyList'>");
        }

        public void GivenProperty(ReportSpecificationSetupProperty property)
        {
            _writer.AppendLine(@"<div class='givenProperty'><span class='propertyName'>" + _formatter.FormatMethodName(property.PropertyName) +
                "</span> <span class='propertyValue code'>" + _formatter.FormatValue(property.PropertyValue) + "</span></div>");
        }

        public void GivenClassProperty(string propertyName, bool isChild, int? indexInParent, bool isNull)
        {
            _writer.Append(@"<div class='givenProperty'><span class='propertyName'>");
            _writer.Append(_formatter.FormatMethodName(propertyName));
            _writer.Append("</span>");
            if (isNull) _writer.Append(@"<span>null</span>");
            _writer.AppendLine("</div>");
        }

        public void EndGivenProperties()
        {
            _writer.AppendLine(@"</div>");
        }

        public void EndGiven()
        {
            _writer.AppendLine(@"</div>");
        }

        public void EndClass()
        {
            _writer.AppendLine(@"</div>");
        }

        public void StartClassTable(string propertyName, string className)
        {
            _writer.AppendLine($"<div class='givenProperty'><span class='propertyName'>{_formatter.FormatMethodName(propertyName)}</span></div><div class='givenSubClass'>");
        }

        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames)
        {
            _writer.AppendLine($"<table class='givenProperty'><thead><tr><th>{string.Join("</th><th>", propertyNames.Select(s => _formatter.FormatMethodName(s)))}</th></tr></thead><tbody>");
        }

        public void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells)
        {
            _writer.AppendLine($"<tr><td>{string.Join("</td><td>", cells.Select(c => _formatter.FormatValue(c.PropertyValue)))}</td></tr>");
        }

        public void EndClassTable()
        {
            _writer.AppendLine("</tbody></table></div>");
        }

        public void When(string actionName)
        {
            _writer.AppendLine(@"<div class='when'><h3>When " + actionName + "</h3></div>");
        }

        public void StartAssertions()
        {
            _writer.AppendLine(@"<div class='assertions'>");
        }

        public void EndAssertions()
        {
            _writer.AppendLine(@"</div>");
        }

        public void Assert(
            string assertPropertyName,
            object assertPropertyExpectedValue,
            AssertionOperator assertionOperator,
            object assertPropertyActualValue,
            bool passed,
            IEnumerable<string> assertionSpecifics)
        {
            _writer.AppendLine(@"<div class='assertion " + (passed ? "assertionPassed" : "assertionFailed") + "'>");
            _writer.AppendLine(@"<span class='assertionProperty'>" + assertPropertyName + "</span>");
            _writer.AppendLine(@"<span class='assertionOperator'>" + assertionOperator.ToDescription() + "</span>");
            _writer.AppendLine(@"<span class='assertionExpectedValue code'>" + _formatter.FormatValue(assertPropertyExpectedValue) + "</span>");
            _writer.AppendLine(@"<span class='assertionActualValue code'>" + (passed ? "" : "(Actual: " + _formatter.FormatValue(assertPropertyActualValue) + ")") + "</span>");

            foreach (var assertionSpecific in assertionSpecifics)
            {
                _writer.AppendLine(@"<span class='code'>" + assertionSpecific + "</span>");
            }

            _writer.AppendLine(@"</div>");
        }

        public void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed)
        {
            _writer.AppendLine(@"<div class='assertionSubClass " + (passed ? "assertionPassed" : "assertionFailed") + "'>");
            _writer.AppendLine(@"<div class='assertion assertionSubClassName'>" + _formatter.FormatMethodName(assertPropertyName) + "</div>");
            _writer.AppendLine(@"<div class='assertionSubClassProperties'>");
        }

        public void EndAssertionSubProperties()
        {
            _writer.AppendLine(@"</div>");
            _writer.AppendLine(@"</div>");
            _writer.AppendLine(@"</div>");
        }

        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue)
        {
            _writer.Append(@"<div>");
            _writer.Append(string.Format("The value in the C# code ({0}) does not match the value in the Excel sheet ({1} for {2}", csharpValue, excelValue, assertPropertyName));
            _writer.AppendLine(@"</div>");

        }

        public void EndSpecification(string specificationNamespace, bool passed)
        {
            _writer.Insert(specificationDivClassIndex, (passed ? "specificationPassed" : "specificationFailed"));
            _writer.AppendLine(@"</div>");

            WriteFooter(specificationNamespace, passed);
        }

        private void WriteFooter(string specificationNamespace, bool passed)
        {
            _writer.Insert(assemblyDivClassIndex, (passed ? "assemblyPassed" : "assemblyFailed"));
            _writer.AppendLine(@"</div>");
            _writer.AppendLine(@"<div class='dateEnded'>Tests completed at " + DateTime.Now.ToString() + "</div>");
            _writer.AppendLine(@"</body>");
            _writer.AppendLine(@"</html>");

            var filename = CreateFileName(specificationNamespace, _specificationName);
            Directory.CreateDirectory(Path.GetDirectoryName(filename));

            using (var fileWriter = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.UTF8))
            {
                fileWriter.Write(_writer.ToString());
            }

        }

        private string CreateFileName(string assemblyName, string specificationName)
        {
            return
                @"\\kl-web-001\CustomerTests\" +
                assemblyName.Replace("RES.", "").Replace(".Specification", "").Replace('.', '\\') +
                @"\" +
                specificationName +
                ".html";
        }

        private IEnumerable<string> GetDirectoryStructure(string assemblyName)
        {
            return assemblyName.Split('.').Where(s => (s != "RES" && s != "Specification"));
        }

        public void Exception(string exception)
        {
            _writer.AppendLine(@"<div class='exception'>Exception " + exception + "</div>");
        }

        public void StartGivenListProperty(ReportSpecificationSetupList list) => throw new NotImplementedException();
        public void StartGivenListPropertyItem(IReportsSpecificationSetup listItem) => throw new NotImplementedException();
        public void EndGivenListPropertyItem(IReportsSpecificationSetup listItem) => throw new NotImplementedException();
        public void EndGivenListProperty(ReportSpecificationSetupList list) => throw new NotImplementedException();
    }
}
