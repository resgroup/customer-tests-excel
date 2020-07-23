using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class LogState
    {
        public readonly List<string> errors;
        public IReadOnlyList<string> Errors => errors;

        public readonly List<string> issuesPreventingRoundTrip;
        public IReadOnlyList<string> IssuesPreventingRoundTrip => issuesPreventingRoundTrip;

        public readonly List<string> warnings;
        public IReadOnlyList<string> Warnings => warnings;

        public readonly List<IExcelToCodeVisitor> visitors;

        public LogState()
        {
            errors = new List<string>();
            issuesPreventingRoundTrip = new List<string>();
            warnings = new List<string>();
            visitors = new List<IExcelToCodeVisitor>();
        }

        internal void Initialise()
        {
            // maybe want to clear the visitors as well
            errors.Clear();
            warnings.Clear();
            issuesPreventingRoundTrip.Clear();
        }

        internal void VisitGivenSimpleProperty(VisitedGivenSimpleProperty givenSimpleProperty)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenSimpleProperty(givenSimpleProperty));
        }

        internal void VisitGivenFunction(VisitedGivenFunction givenFunction)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenFunction(givenFunction));
        }

        public void AddVisitor(IExcelToCodeVisitor visitor) =>
            visitors.Add(visitor);

        public void VisitGivenRootClassDeclaration(string excelClassName)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenRootClassDeclaration(excelClassName));
        }

        public void VisitGivenRootClassFinalisation() =>
            visitors.ForEach(v => v.VisitGivenRootClassFinalisation());

        public void VisitGivenComplexPropertyDeclaration(
            string sutPropertyName,
            string excelClassName)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenComplexPropertyDeclaration(
                        new VisitedGivenComplexProperty(
                            sutPropertyName,
                            excelClassName)));
        }

        public void VisitGivenComplexPropertyFinalisation() =>
            visitors.ForEach(v => v.VisitGivenComplexPropertyFinalisation());

        public void VisitGivenListPropertyDeclaration(
            string codePropertyName,
            string excelClassName)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenListPropertyDeclaration(
                        new VisitedGivenListProperty(
                            codePropertyName,
                            excelClassName)));
        }

        public void VisitGivenListPropertyFinalisation() =>
            visitors.ForEach(v => v.VisitGivenListPropertyFinalisation());

        public void VisitGivenTablePropertyDeclaration(
            string codePropertyName,
            string excelClassName,
            IEnumerable<TableHeader> tableHeaders)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenTablePropertyDeclaration(
                        new VisitedGivenTableProperty(
                            codePropertyName,
                            excelClassName),
                        tableHeaders));
        }

        public void VisitGivenTablePropertyRowDeclaration(uint row)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenTablePropertyRowDeclaration(row));
        }

        public void VisitGivenTablePropertyCellDeclaration(TableHeader tableHeader, uint row, uint column)
        {
            visitors.ForEach(
                v =>
                    v.VisitGivenTablePropertyCellDeclaration(
                        tableHeader,
                        row,
                        column));
        }

        public void VisitGivenTablePropertyCellFinalisation() =>
            visitors.ForEach(v => v.VisitGivenTablePropertyCellFinalisation());

        public void VisitGivenTablePropertyRowFinalisation() =>
            visitors.ForEach(v => v.VisitGivenTablePropertyRowFinalisation());

        public void VisitGivenTablePropertyFinalisation() =>
            visitors.ForEach(v => v.VisitGivenTablePropertyFinalisation());


        internal void AddIssuePreventingRoundTrip(string issue) =>
            issuesPreventingRoundTrip.Add(issue);

        public void AddError(string message) =>
            errors.Add(message);
    }
}
