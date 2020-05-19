using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CustomerTestsExcel.Assertions;
using System.Reflection;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public class HTMLTestOutputWriter : ITestOutputWriter
    {
        protected StringBuilder writer;
        protected int assemblyDivClassIndex;
        protected int specificationDivClassIndex;
        protected string specificationName;
        protected readonly IHumanFriendlyFormatter formatter;

        public string Html => writer.ToString();

        public HTMLTestOutputWriter(IHumanFriendlyFormatter formatter)
        {
            this.formatter = formatter ?? throw new ArgumentNullException("formatter");
        }

        private void WriteHeader(string specificationNamespace)
        {
            writer = new StringBuilder();

            writer.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            writer.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
            writer.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>");
            writer.AppendLine(@"<head>");
            writer.Append(@"<link rel='stylesheet' type='text/css' href='");
            for (int i = 0; i < GetDirectoryStructure(specificationNamespace).Count(); i++) writer.Append(@"..\");
            writer.Append(@"Specification.css' media='screen'/>");
            writer.AppendLine();
            writer.AppendLine(string.Format(@"<title>Test Results for {0}</title>", specificationNamespace));
            writer.AppendLine("<meta http-equiv='cache-control' content='no-cache' />");
            writer.AppendLine("<meta http-equiv='pragma' content='no-cache' />");
            writer.AppendLine(@"</head>");
            writer.AppendLine(@"<body>");
            writer.Append(@"<div class='assembly ");
            assemblyDivClassIndex = writer.Length;
            writer.AppendLine(@"'>");
            writer.AppendLine(@"<div class='assemblyName'><h1>Testing Assembly " + specificationNamespace + "</h1></div>");
            writer.AppendLine(@"<div class='dateStarted'>Tests started at " + DateTime.Now.ToString() + "</div>");

        }

        public void StartSpecification(string specificationNamespace, string specificationName, string specificationDescription)
        {
            WriteHeader(specificationNamespace);

            this.specificationName = specificationName;

            writer.Append(@"<div class='specification ");
            specificationDivClassIndex = writer.Length;
            writer.AppendLine(@"'>");

            writer.AppendLine(@"<div class='specificationName'><h2>Specification: " + specificationDescription + @"</h2></div>");
        }

        public void StartGiven()
        {
            writer.AppendLine(@"<div class='given'>");
        }

        public void StartClass(string className)
        {
            writer.AppendLine(@"<div class='givenClassName'><h3>Given a " + formatter.FormatSpecificationSpecificClassName(className) + "</h3></div>");
        }

        public void StartSubClass(string className)
        {
            writer.AppendLine(@"<div class='givenSubClass'>");
            writer.AppendLine(@"<div class='givenSubClassName'>" + formatter.FormatSpecificationSpecificClassName(className) + "</div>");
        }

        public void EndSubClass()
        {
            writer.AppendLine(@"</div>");
            writer.AppendLine(@"</div>");
        }

        public void StartGivenProperties()
        {
            writer.AppendLine(@"<div class='withProperties'>");
            writer.AppendLine(@"<div>With Properties</div>");
            writer.AppendLine(@"<div class='givenPropertyList'>");
        }

        public void GivenProperty(ReportSpecificationSetupProperty property)
        {
            writer.AppendLine(@"<div class='givenProperty'><span class='propertyName'>" + formatter.FormatMethodName(property.PropertyName) +
                "</span> <span class='propertyValue code'>" + formatter.FormatValue(property.PropertyValue) + "</span></div>");
        }

        public void GivenClassProperty(string propertyName, bool isNull)
        {
            writer.Append(@"<div class='givenProperty'><span class='propertyName'>");
            writer.Append(formatter.FormatMethodName(propertyName));
            writer.Append("</span>");
            if (isNull) writer.Append(@"<span>null</span>");
            writer.AppendLine("</div>");
        }

        public void EndGivenProperties()
        {
            writer.AppendLine(@"</div>");
        }

        public void EndGiven()
        {
            writer.AppendLine(@"</div>");
        }

        public void EndClass()
        {
            writer.AppendLine(@"</div>");
        }

        public void StartClassTable(string propertyName, string className)
        {
            writer.AppendLine($"<div class='givenProperty'><span class='propertyName'>{formatter.FormatMethodName(propertyName)}</span></div><div class='givenSubClass'>");
        }

        public void ClassTablePropertyNamesHeaderRow(IEnumerable<string> propertyNames)
        {
            writer.AppendLine($"<table class='givenProperty'><thead><tr><th>{string.Join("</th><th>", propertyNames.Select(s => formatter.FormatMethodName(s)))}</th></tr></thead><tbody>");
        }

        public void ClassTablePropertyRow(IEnumerable<ReportSpecificationSetupProperty> cells)
        {
            writer.AppendLine($"<tr><td>{string.Join("</td><td>", cells.Select(c => formatter.FormatValue(c.PropertyValue)))}</td></tr>");
        }

        public void EndClassTable()
        {
            writer.AppendLine("</tbody></table></div>");
        }

        public void When(string actionName)
        {
            writer.AppendLine(@"<div class='when'><h3>When " + actionName + "</h3></div>");
        }

        public void StartAssertions()
        {
            writer.AppendLine(@"<div class='assertions'>");
        }

        public void EndAssertions()
        {
            writer.AppendLine(@"</div>");
        }

        public void Assert(
            string assertPropertyName,
            object assertPropertyExpectedValue,
            AssertionOperator assertionOperator,
            object assertPropertyActualValue,
            bool passed,
            IEnumerable<string> assertionSpecifics)
        {
            writer.AppendLine(@"<div class='assertion " + (passed ? "assertionPassed" : "assertionFailed") + "'>");
            writer.AppendLine(@"<span class='assertionProperty'>" + assertPropertyName + "</span>");
            writer.AppendLine(@"<span class='assertionOperator'>" + assertionOperator.ToDescription() + "</span>");
            writer.AppendLine(@"<span class='assertionExpectedValue code'>" + formatter.FormatValue(assertPropertyExpectedValue) + "</span>");
            writer.AppendLine(@"<span class='assertionActualValue code'>" + (passed ? "" : "(Actual: " + formatter.FormatValue(assertPropertyActualValue) + ")") + "</span>");

            foreach (var assertionSpecific in assertionSpecifics)
            {
                writer.AppendLine(@"<span class='code'>" + assertionSpecific + "</span>");
            }

            writer.AppendLine(@"</div>");
        }

        public void StartAssertionSubProperties(string assertPropertyName, bool exists, string cSharpClassName, bool passed)
        {
            writer.AppendLine(@"<div class='assertionSubClass " + (passed ? "assertionPassed" : "assertionFailed") + "'>");
            writer.AppendLine(@"<div class='assertion assertionSubClassName'>" + formatter.FormatMethodName(assertPropertyName) + "</div>");
            writer.AppendLine(@"<div class='assertionSubClassProperties'>");
        }

        public void EndAssertionSubProperties()
        {
            writer.AppendLine(@"</div>");
            writer.AppendLine(@"</div>");
            writer.AppendLine(@"</div>");
        }

        public void CodeValueDoesNotMatchExcelFormula(string assertPropertyName, string excelValue, string csharpValue)
        {
            writer.Append(@"<div>");
            writer.Append(string.Format("The value in the C# code ({0}) does not match the value in the Excel sheet ({1} for {2}", csharpValue, excelValue, assertPropertyName));
            writer.AppendLine(@"</div>");

        }

        public void EndSpecification(string specificationNamespace, bool passed)
        {
            writer.Insert(specificationDivClassIndex, (passed ? "specificationPassed" : "specificationFailed"));
            writer.AppendLine(@"</div>");

            WriteFooter(specificationNamespace, passed);
        }

        private void WriteFooter(string specificationNamespace, bool passed)
        {
            writer.Insert(assemblyDivClassIndex, (passed ? "assemblyPassed" : "assemblyFailed"));
            writer.AppendLine(@"</div>");
            writer.AppendLine(@"<div class='dateEnded'>Tests completed at " + DateTime.Now.ToString() + "</div>");
            writer.AppendLine(@"</body>");
            writer.AppendLine(@"</html>");

            var filename = CreateFileName(specificationNamespace, specificationName);
            Directory.CreateDirectory(Path.GetDirectoryName(filename));

            using (var fileWriter = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.UTF8))
            {
                fileWriter.Write(writer.ToString());
            }

        }

        private string CreateFileName(string assemblyName, string specificationName) =>
            $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{assemblyName.Replace('.', '\\')}\{specificationName}.html";

        private IEnumerable<string> GetDirectoryStructure(string assemblyName)
        {
            return assemblyName.Split('.').Where(s => (s != "RES" && s != "Specification"));
        }

        public void Exception(string exception)
        {
            writer.AppendLine(@"<div class='exception'>Exception " + exception + "</div>");
        }

        public void StartGivenListProperty(ReportSpecificationSetupList list)
        {
            // It's a shame I can't use a TidyUp type solution to ensure elements are close
            // I'll have a think about making this possible
            // Maybe I could return a TidyUp from this function
            writer.AppendLine(@"<div class='givenList'>");
            writer.AppendLine(@"<div class='givenListProperty'>");
            writer.AppendLine($@"<span class='propertyName'>{formatter.FormatMethodName(list.PropertyName)}</span>");
            writer.AppendLine($@"<span class='propertyType code'>{formatter.FormatValue(list.PropertyType)}</span>");
            writer.AppendLine(@"</div>");

        }

        public void StartGivenListPropertyItem(IReportsSpecificationSetup listItem)
        {
            writer.AppendLine(@"<div class='withItem'>");
            writer.AppendLine(@"<div>With Item</div>");
        }

        public void EndGivenListPropertyItem(IReportsSpecificationSetup listItem)
        {
            writer.AppendLine(@"</div>");
        }

        public void EndGivenListProperty(ReportSpecificationSetupList list)
        {
            writer.AppendLine(@"</div>");
        }
    }
}
